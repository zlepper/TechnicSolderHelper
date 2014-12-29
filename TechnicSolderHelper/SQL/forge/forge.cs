using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Net;
using Mono.Data.Sqlite;
using Newtonsoft.Json;

namespace TechnicSolderHelper.SQL.forge
{
    public class ForgeSqlHelper : SqlHelper
    {
        public ForgeSqlHelper()
            : base("forge")
        {
            const string createTableString = "CREATE TABLE IF NOT EXISTS 'forge' ('totalversion' TEXT,'build' INTEGER UNIQUE, 'mcversion' TEXT, 'version' TEXT, 'downloadurl' TEXT, 'type' TEXT, PRIMARY KEY(totalversion));";
            ExecuteDatabaseQuery(createTableString);
        }

        private void AddVersion(String build, String mcversion, String version, String type, String downloadUrl)
        {
            String totalversion = mcversion + "-" + version + "-" + build + "-" + type;

            String sql = String.Format("INSERT OR REPLACE INTO {0}('totalversion', 'build', 'mcversion', 'version', 'downloadurl', 'type') VALUES('{1}','{2}','{3}','{4}', '{5}','{6}');", TableName, totalversion, build, mcversion, version, downloadUrl, type);
            //Debug.WriteLine(sql);
            ExecuteDatabaseQuery(sql);
        }

        public List<String> GetMcVersions()
        {
            String sql = String.Format("SELECT DISTINCT mcversion FROM {0} ORDER BY mcversion ASC;", TableName);
            List<String> mcversion = new List<string>();
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
                                mcversion.Add(reader["mcversion"].ToString());
                            }

                            return mcversion;
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
                            mcversion.Add(reader["mcversion"].ToString());
                        }

                        return mcversion;
                    }
                }
            }
        }

        public List<String> GetForgeVersions(String mcversion)
        {
            String sql = String.Format("SELECT DISTINCT build FROM {0} WHERE mcversion LIKE '{1}' ORDER BY mcversion ASC;", TableName, mcversion);
            List<String> forgeVersions = new List<string>();
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
                                forgeVersions.Add(reader["build"].ToString());
                            }

                            return forgeVersions;
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
                            forgeVersions.Add(reader["build"].ToString());
                        }

                        return forgeVersions;
                    }
                }
            }
        }

        public Number GetForgeInfo(String forgebuild)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE build LIKE '{1}';", TableName, forgebuild);
            Debug.WriteLine(sql);
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
                                Number build = new Number
                                {
                                    Build = int.Parse(reader["build"].ToString()),
                                    Mcversion = reader["mcversion"].ToString(),
                                    Version = reader["version"].ToString(),
                                    Downloadurl = reader["downloadurl"].ToString()
                                };
                                return build;
                            }
                        }
                    }
                }
                return new Number();
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
                            Number build = new Number
                            {
                                Build = int.Parse(reader["build"].ToString()),
                                Mcversion = reader["mcversion"].ToString(),
                                Version = reader["version"].ToString(),
                                Downloadurl = reader["downloadurl"].ToString()
                            };
                            return build;
                        }
                    }
                }
            }
            return new Number();
        }

        public void FindAllForgeVersion()
        {
            WebClient wb = new WebClient();
            String forgejsonweb = "http://files.minecraftforge.net/minecraftforge/json";
            String jsonfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "forge.json");

            wb.DownloadFile(forgejsonweb, jsonfile);
            Debug.WriteLine("Downloaded json file");
            String json;

            forgejsonweb = "http://files.minecraftforge.net/maven/net/minecraftforge/forge/json";
            wb.DownloadFile(forgejsonweb, jsonfile);
            using (StreamReader r = new StreamReader(jsonfile))
            {
                json = r.ReadToEnd();
            }
            Debug.WriteLine("readjson");
            Forgemaven mavenunjsonend = JsonConvert.DeserializeObject<Forgemaven>(json);
            int concurrentgone = 0;
            int i = 1;
            while (concurrentgone <= 100)
            {
                if (mavenunjsonend.Number.ContainsKey(i))
                {
                    Debug.WriteLine(mavenunjsonend.Number[i].Build.ToString());
                    String mcversion = mavenunjsonend.Number[i].Mcversion;
                    String build = mavenunjsonend.Number[i].Build.ToString();
                    String version = mavenunjsonend.Number[i].Version;
                    String downloadUrl = mavenunjsonend.Webpath + "/" + mcversion + "-" + version + "/forge-" + mcversion + "-" + version + "-";
                    if (i < 183)
                    {
                        downloadUrl += "client.";
                    }
                    else
                    {
                        downloadUrl += "universal.";
                    }
                    if (i < 752)
                    {
                        downloadUrl += "zip";

                    }
                    else
                    {
                        downloadUrl += "jar";
                    }
                    Debug.WriteLine(downloadUrl);
                    AddVersion(build, mcversion, version, "universal", downloadUrl);
                    concurrentgone = 0;
                }
                else
                {
                    concurrentgone += 1;
                }
                i++;

            }

        }

    }
}
