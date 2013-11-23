using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace LFS.Services
{
    public class SQLiteProvider
    {
        private string dbConnection;

        public SQLiteProvider(string db)
        {
            dbConnection = String.Format("Data Source={0}", db);
        }

        protected bool HasAny(string query)
        {
            var data = GetDataTable(query);
            return data.Rows.Count > 0;
        }

        protected bool TableExists(string table)
        {
            using (var conn = new SQLiteConnection(dbConnection))
            {
                conn.Open();
                var data = conn.GetSchema("Tables").Select(String.Format("Table_name = '{0}'", table));
                return (data.Length > 0);
            }
        }

        public int ExecuteNonQuery(string query)
        {
            using (var conn = new SQLiteConnection(dbConnection))
            using (var cmd = new SQLiteCommand(conn))
            {
                conn.Open();
                cmd.CommandText = query;
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetDataTable(string query)
        {
            using (var conn = new SQLiteConnection(dbConnection))
            using (var cmd = new SQLiteCommand(conn))
            {
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    cmd.CommandText = query;
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }

                return dt;
            }
        }

        public string ExecuteScalar(string query)
        {
            using (var conn = new SQLiteConnection(dbConnection))
            using (var cmd = new SQLiteCommand(conn))
            {
                conn.Open();
                cmd.CommandText = query;
                object value = cmd.ExecuteScalar();
                return value != null ? value.ToString() : "";
            }
        }

        public bool Insert(string tableName, Dictionary<string, string> data)
        {
            string columns = "";
            string values = "";
            bool returnCode = true;

            foreach (KeyValuePair<string, string> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception fail)
            {
                System.Windows.MessageBox.Show(fail.Message);
                returnCode = false;
            }

            return returnCode;
        }

        protected bool Delete(string table, string where)
        {
            bool returnCode = true;

            try
            {
                ExecuteNonQuery(String.Format("delete from {0} where {1};", table, where));
            }
            catch (Exception fail)
            {
                System.Windows.MessageBox.Show(fail.Message);
                returnCode = false;
            }

            return returnCode;
        }

        protected bool DropTable(string table)
        {
            bool returnCode = true;

            try
            {
                ExecuteNonQuery(String.Format("drop table {0}", table));
            }
            catch (Exception fail)
            {
                System.Windows.MessageBox.Show(fail.Message);
                returnCode = false;
            }

            return returnCode;
        }

        protected bool Update(string table, Dictionary<string, string> data, string where)
        {
            string vals = "";
            bool returnCode = true;

            if (data.Count > 0)
            {
                foreach (KeyValuePair<string, string> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }

            try
            {
                ExecuteNonQuery(String.Format("update {0} set {1} where {2};", table, vals, where));
            }
            catch (Exception fail)
            {
                System.Windows.MessageBox.Show(fail.Message);
                returnCode = false;
            }

            return returnCode;
        }
        
    }
}
