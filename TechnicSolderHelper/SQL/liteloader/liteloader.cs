using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using Mono.Data.Sqlite;

namespace TechnicSolderHelper.SQL.liteloader
{
    public class Liteloader
    {
        public Meta Meta { get; set; }

        public Dictionary<string, Versions> Versions { get; set; }
    }

    public class Meta
    {
        public String Description { get; set; }

        public String Authors { get; set; }

        public String Url { get; set; }
    }

    public class Versions
    {
        public Dictionary<String, Dictionary<String, Versionclass>> Artefacts { get; set; }
    }

    public class Versionclass
    {
        public String TweakClass { get; set; }

        public String File { get; set; }

        public String Version { get; set; }

        public String Md5 { get; set; }

        public string Timestamp { get; set; }
    }

    public class Liteloaderversion
    {
        public String File { get; set; }

        public String Version{ get; set; }

        public String Md5{ get; set; }

        public String Mcversion{ get; set; }

        public String TweakClass{ get; set; }
    }

    public class LiteloaderSqlHelper : SqlHelper
    {
        protected readonly String CreateTableString;

        public LiteloaderSqlHelper()
            : base("liteloader")
        {
            CreateTableString = "CREATE TABLE IF NOT EXISTS 'liteloader' (file TEXT, version TEXT, md5 TEXT UNIQUE, mcversion TEXT, tweakClass TEXT, PRIMARY KEY(md5));";
            ExecuteDatabaseQuery(CreateTableString);
        }

        public void AddVersion(String file, String version, String md5, String mcversion, String tweakClass)
        {
            String sql = String.Format("INSERT OR REPLACE INTO {0} ('file', 'version', 'md5', 'mcversion', 'tweakClass') VALUES ('{1}','{2}','{3}','{4}','{5}');", TableName, file, version, md5, mcversion, tweakClass);
            Debug.WriteLine(sql);
            ExecuteDatabaseQuery(sql);
        }

        public Liteloaderversion GetInfo(String md5)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE md5 LIKE '{1}';", TableName, md5);
            Liteloaderversion llversion = new Liteloaderversion();
            if (IsUnix())
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
                                llversion.Md5 = reader["md5"].ToString();
                                llversion.Mcversion = reader["mcversion"].ToString();
                                llversion.Version = reader["version"].ToString();
                                llversion.File = reader["file"].ToString();
                                llversion.TweakClass = reader["tweakClass"].ToString();
                            }

                            return llversion;
                        }
                    }
                }
            }
            using (SQLiteConnection db = new SQLiteConnection(ConnectionString))
            {
                db.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, db))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            llversion.Md5 = reader["md5"].ToString();
                            llversion.Mcversion = reader["mcversion"].ToString();
                            llversion.Version = reader["version"].ToString();
                            llversion.File = reader["file"].ToString();
                            llversion.TweakClass = reader["tweakClass"].ToString();
                        }

                        return llversion;
                    }
                }
            }
        }
    }
}

