using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using LFS.Services;
using LFS.Helpers;

namespace LFS.Model
{
    public class SubtitlesModel
    {
        public List<string> words = new List<string>();

        #region Private fields
        private string _fileName;
        private bool _hasAnyMatchedWordsFromDb;
        #endregion

        public SubtitlesModel(string fileName)
        {
            this._fileName = fileName;
        }

        #region Functions to fill @words
        public void MostRareWords()
        {
            var appearance = EatFile();
            words = (from entry in appearance orderby entry.Value ascending select entry.Key).ToList();
        }

        public void MostOftenWords()
        {
            var appearance = EatFile();
            words = (from entry in appearance orderby entry.Value descending select entry.Key).ToList();
        }

        public void MatchDb()
        {
            var appearance = EatFile();
            var wordsFromDb = Db.Instance.GetAllWords();
            words = (from entry in appearance where wordsFromDb.Contains(entry.Key) select entry.Key).ToList();
        }

        #endregion


        #region Private function
        private Dictionary<string, int> EatFile()
        {
            Dictionary<string, int> appearance = new Dictionary<string, int>();
            string text = File.ReadAllText(this._fileName);

            MatchCollection matches = Regex.Matches(text, @"\b[A-z]{4,}\b", RegexOptions.IgnoreCase);
            for (var i = 0; i < matches.Count; i++)
            {
                string word = matches[i].ToString().ToLower();

                // If already exists
                if (appearance.ContainsKey(word))
                {
                    // Increment value
                    appearance[word]++;
                }
                else
                {
                    appearance.Add(word, 0);
                }
            }

            return appearance;
        }
        #endregion

        #region Public function
        public bool HasWord(string word)
        {
            return Db.Instance.HasWord(word);
        }

        public bool AddWord(string word)
        {
            return Db.Instance.AddWord(word);
        }

        public bool DeleteWord(string word)
        {
            return Db.Instance.DeleteWord(word);
        }

        public bool UpdateWordInfo(string word)
        {
            if (Db.Instance.LastSeenWord(word) > 20)
            {
                Db.Instance.UpdateLastSeenTime(word, UnixDate.Now);
                return true;
            }
            return false;
        }

        public bool HasAnyMatchedWordsFromDb
        {
            get 
            {
                var appearance = EatFile();
                var wordsFromDb = Db.Instance.GetAllWords();
                var result = (from entry in appearance where wordsFromDb.Contains(entry.Key) select entry.Key).ToList();
                _hasAnyMatchedWordsFromDb = result.Count > 0;
                return _hasAnyMatchedWordsFromDb;
            }
        }

        #endregion

    }
}
