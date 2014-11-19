using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Mono.Data.Sqlite;

namespace TechnicSolderHelper.SQL
{
    public abstract class SQLHelper
    {
        public SQLHelper(String databaseName, String TableName)
        {
            databaseName += ".db";

            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper"));
            databaseName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", databaseName);
            this.databaseName = databaseName;
            Debug.WriteLine(this.databaseName);
            try
            {
                if (!File.Exists(this.databaseName))
                {
                    if (isUnix())
                    {
                        SqliteConnection.CreateFile(this.databaseName);
                    }
                    else
                    {
                        SQLiteConnection.CreateFile(this.databaseName);
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Database already existing");
            }
            if (isUnix())
            {
                SqliteConnectionStringBuilder c = new SqliteConnectionStringBuilder();
                c.DataSource = this.databaseName;
                this.ConnectionString = c.ConnectionString;
            }
            else
            {
                SQLiteConnectionStringBuilder c = new SQLiteConnectionStringBuilder();
                c.DataSource = this.databaseName;
                this.ConnectionString = c.ConnectionString;
            }
            this.TableName = TableName;
        }

        protected readonly String databaseName;
        protected readonly String TableName;
        protected readonly String ConnectionString;

        public Boolean isUnix()
        {
            return Environment.OSVersion.ToString().ToLower().Contains("unix");
        }

        protected void executeDatabaseQuery(String sql)
        {
            executeDatabaseQuery(sql, false);
        }

        protected void executeDatabaseQueryAsync(String sql)
        {
            executeDatabaseQuery(sql, true);
        }

        protected void executeDatabaseQuery(String sql, Boolean Async)
        {
            if (isUnix())
            {
                try
                {
                    using (SqliteConnection db = new SqliteConnection(this.ConnectionString))
                    {
                        db.Open();
                        using (SqliteCommand cmd = new SqliteCommand(sql, db))
                        {
                            if (Async)
                            {
                                cmd.ExecuteNonQueryAsync();
                            }
                            else
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                }
            }
            else
            {
                try
                {
                    using (SQLiteConnection db = new SQLiteConnection(this.ConnectionString))
                    {
                        db.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, db))
                        {
                            if (Async)
                            {
                                cmd.ExecuteNonQueryAsync();
                            }
                            else
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        public virtual void resetTable()
        {
            String sql = String.Format("DROP TABLE {0};", this.TableName);
            executeDatabaseQuery(sql);
        }

        public static String calculateMD5(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }
    }
}