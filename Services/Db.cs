using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using LFS.Helpers;

namespace LFS.Services
{
    public sealed class Db : SQLiteProvider
    {
        #region Singelton stuff
        static Db instance = null;
        static readonly object padlock = new object();

        public static Db Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Db();
                    }
                    return instance;
                }
            }
        }

        #endregion

        const string WORDS_TABLE = "words";

        public Db()
            : base("database.db")
        {
            if (!TableExists(WORDS_TABLE))
            {
                CreateTableWords();
            }
        }

        private bool CreateTableWords()
        {
            string query = String.Format("create table {0} (id integer primary key, word varchar(255), added_time integer not null," +
                "last_seen_time integer not null, view_count integer not null);", WORDS_TABLE);
            return (ExecuteNonQuery(query) > 0);
        }

        public bool HasWord(string word)
        {
            return HasAny(String.Format("select * from {0} where word='{1}'", WORDS_TABLE, word)); 
        }

        public bool AddWord(string word)
        {
            var data = new Dictionary<string, string>();
            data.Add("word", word);
            data.Add("added_time", UnixDate.Now.ToString());
            data.Add("last_seen_time", UnixDate.Now.ToString());
            data.Add("view_count", "1");
            return Insert(WORDS_TABLE, data);
        }

        public bool DeleteWord(string word)
        {
            return Delete(WORDS_TABLE, String.Format("word='{0}'", word));
        }

        public List<string> GetAllWords()
        {
            var words = new List<string>();
            var data = GetDataTable(String.Format("select * from {0}", WORDS_TABLE));
            foreach (DataRow r in data.Rows)
            {
                words.Add(r["word"].ToString());
            }

            return words;
        }

        /// <summary>
        /// Return count of minutes from last show of this word
        /// </summary>
        public int LastSeenWord(string word)
        {
            var data = GetDataTable(String.Format("select * from {0} where word='{1}';", WORDS_TABLE, word));
            int time = Helper.ToInt32(data.Rows[0]["added_time"].ToString());

            // Minutes
            return (UnixDate.Now - time) / 60;
        }

        public int ViewCountOfWord(string word)
        {
            var data = GetDataTable(String.Format("select view_count from {0} where word='{1}';", WORDS_TABLE, word));
            return Helper.ToInt32(data.Rows[0]["view_count"].ToString());
        }

        public bool UpdateLastSeenTime(string word, double time)
        {
            var data = new Dictionary<string, string>();
            data.Add("last_seen_time", time.ToString());
            string where = String.Format("word='{0}'", word);

            return Update(WORDS_TABLE, data, where);
        }

        public bool UpdateViewCountOfWord(string word)
        {
            var data = new Dictionary<string, string>();
            int newCount = ViewCountOfWord(word) + 1;
            data.Add("view_count", newCount.ToString());
            string where = String.Format("word='{0}'", word);

            return Update(WORDS_TABLE, data, where);
        }
    }
}
