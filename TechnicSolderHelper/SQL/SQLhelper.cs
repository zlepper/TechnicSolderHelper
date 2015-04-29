using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Mono.Data.Sqlite;

namespace TechnicSolderHelper.SQL
{
    public abstract class SqlHelper
    {
        protected SqlHelper(String tableName)
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper"));
            String databaseName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper", "SolderHelper.db");
            try
            {
                if (!File.Exists(databaseName))
                {
                    if (IsUnix())
                    {
                        SqliteConnection.CreateFile(databaseName);
                    }
                    else
                    {
                        SQLiteConnection.CreateFile(databaseName);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            if (IsUnix())
            {
                SqliteConnectionStringBuilder c = new SqliteConnectionStringBuilder { DataSource = databaseName };
                ConnectionString = c.ConnectionString;
            }
            else
            {
                SQLiteConnectionStringBuilder c = new SQLiteConnectionStringBuilder { DataSource = databaseName };
                ConnectionString = c.ConnectionString;
            }
            TableName = tableName;
        }
        protected readonly String TableName;
        protected readonly String ConnectionString;

        protected static Boolean IsUnix()
        {
            return Environment.OSVersion.ToString().ToLower().Contains("unix");
        }

        protected void ExecuteDatabaseQuery(String sql, Boolean async = false)
        {
            if (IsUnix())
            {
                try
                {
                    using (SqliteConnection db = new SqliteConnection(ConnectionString))
                    {
                        db.Open();
                        using (SqliteCommand cmd = new SqliteCommand(sql, db))
                        {
                            if (async)
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
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                /*try
                {*/
                    using (SQLiteConnection db = new SQLiteConnection(ConnectionString))
                    {
                        db.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, db))
                        {
                            if (async)
                            {
                                cmd.ExecuteNonQueryAsync();
                            }
                            else
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
               /* }
                catch (Exception)
                {
                    // ignored
                }*/
            }
        }

        public virtual void ResetTable()
        {
            String sql = String.Format("DROP TABLE {0};", TableName);
            ExecuteDatabaseQuery(sql);
        }

        public static String CalculateMd5(string file)
        {
            using (var md5 = MD5.Create())
            {
                while (true)
                {
                    try
                    {
                        using (var stream = File.OpenRead(file))
                        {
                            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                        }
                    }
                    catch
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}