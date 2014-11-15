using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Mono.Data.Sqlite;
using Newtonsoft.Json;

namespace TechnicSolderHelper
{
    public class liteloader
    {
        public meta meta { get; set; }

        public Dictionary<String, versions> versions { get; set; }
    }

    public class meta
    {
        public String description { get; set; }

        public String authors { get; set; }

        public String url { get; set; }
    }

    public class versions
    {
        public Dictionary<String, Dictionary<String, versionclass>> artefacts { get; set; }
    }

    public class versionclass
    {
        public String tweakClass { get; set; }

        public String file { get; set; }

        public String version { get; set; }

        public String md5 { get; set; }

        public string timestamp { get; set; }
    }

    public class liteloaderversion
    {
        public String file { get; set; }

        public String version{ get; set; }

        public String md5{ get; set; }

        public String mcversion{ get; set; }

        public String tweakClass{ get; set; }
    }

    public class liteloaderSQLHelper : SQL.SQLHelper
    {
        protected readonly String CreateTableString;

        public liteloaderSQLHelper()
            : base("Forge", "liteloader")
        {
            CreateTableString = "CREATE TABLE IF NOT EXISTS 'liteloader' (file TEXT, version TEXT, md5 TEXT UNIQUE, mcversion TEXT, tweakClass TEXT, PRIMARY KEY(md5));";
            executeDatabaseQuery(CreateTableString);
        }

        public void addVersion(String file, String version, String md5, String mcversion, String tweakClass)
        {
            String sql = String.Format("INSERT OR REPLACE INTO {0} ('file', 'version', 'md5', 'mcversion', 'tweakClass') VALUES ('{1}','{2}','{3}','{4}','{5}');", this.TableName, file, version, md5, mcversion, tweakClass);
            Debug.WriteLine(sql);
            executeDatabaseQuery(sql);
        }

        public liteloaderversion getInfo(String MD5)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE md5 LIKE '{1}';", this.TableName, MD5);
            liteloaderversion llversion = new liteloaderversion();
            if (isUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                llversion.md5 = reader["md5"].ToString();
                                llversion.mcversion = reader["mcversion"].ToString();
                                llversion.version = reader["version"].ToString();
                                llversion.file = reader["file"].ToString();
                                llversion.tweakClass = reader["tweakClass"].ToString();
                            }

                            return llversion;
                        }
                    }
                }
            }
            else
            {
                using (SQLiteConnection db = new SQLiteConnection(ConnectionString))
                {
                    db.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, db))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                llversion.md5 = reader["md5"].ToString();
                                llversion.mcversion = reader["mcversion"].ToString();
                                llversion.version = reader["version"].ToString();
                                llversion.file = reader["file"].ToString();
                                llversion.tweakClass = reader["tweakClass"].ToString();
                            }

                            return llversion;
                        }
                    }
                }
            }
        }
    }
}

