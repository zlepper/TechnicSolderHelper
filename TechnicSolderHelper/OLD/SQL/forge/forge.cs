using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using System.Text;
using Mono.Data.Sqlite;
using Newtonsoft.Json;

namespace TechnicSolderHelper.OLD.SQL.forge
{
    public class ForgeSqlHelper : SqlHelper
    {
        public ForgeSqlHelper()
            : base("forge")
        {
            const string createTableString = "CREATE TABLE IF NOT EXISTS 'forge' ('totalversion' TEXT,'build' INTEGER UNIQUE, 'mcversion' TEXT, 'version' TEXT, 'downloadurl' TEXT, 'type' TEXT, PRIMARY KEY(totalversion));";
            ExecuteDatabaseQuery(createTableString);
        }

        private void AddVersions(List<string> builds, List<string> mcversions, List<string> versions, List<string> types,
            List<string> downloadUrls)
        {
            String sql = String.Format("INSERT OR REPLACE INTO {0}('totalversion', 'build', 'mcversion', 'version', 'downloadurl', 'type') VALUES(@totalversion, @build, @mcversion, @version, @downloadurl, @type);", TableName);

            if (IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < builds.Count; i++)
                        {
                            sb.Clear();
                            sb.Append(mcversions[i])
                                .Append("-")
                                .Append(versions[i])
                                .Append("-")
                                .Append(builds[i])
                                .Append("-")
                                .Append(types[i]);
                            cmd.Parameters.AddWithValue("@totalversion", sb.ToString());
                            cmd.Parameters.AddWithValue("@build", builds[i]);
                            cmd.Parameters.AddWithValue("@mcversion", mcversions[i]);
                            cmd.Parameters.AddWithValue("@version", versions[i]);
                            cmd.Parameters.AddWithValue("@downloadurl", downloadUrls[i]);
                            cmd.Parameters.AddWithValue("@type", types[i]);
                            cmd.ExecuteNonQuery();
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
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < builds.Count; i++)
                        {
                            sb.Clear();
                            sb.Append(mcversions[i])
                                .Append("-")
                                .Append(versions[i])
                                .Append("-")
                                .Append(builds[i])
                                .Append("-")
                                .Append(types[i]);
                            cmd.Parameters.AddWithValue("@totalversion", sb.ToString());
                            cmd.Parameters.AddWithValue("@build", builds[i]);
                            cmd.Parameters.AddWithValue("@mcversion", mcversions[i]);
                            cmd.Parameters.AddWithValue("@version", versions[i]);
                            cmd.Parameters.AddWithValue("@downloadurl", downloadUrls[i]);
                            cmd.Parameters.AddWithValue("@type", types[i]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public List<String> GetMcVersions()
        {
            String sql = String.Format("SELECT DISTINCT mcversion FROM {0} WHERE mcversion NOT LIKE '1.7.10_pre4' ORDER BY mcversion ASC;", TableName);
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
            List<string> builds = new List<string>(); 
            List<string> mcversions = new List<string>();
            List<string> versions = new List<string>();
            List<string> types = new List<string>();
            List<string> downloadUrls = new List<string>();
            Forgemaven mavenunjsonend;
            HttpClient client = new HttpClient();
            using (Stream s = client.GetStreamAsync("http://files.minecraftforge.net/maven/net/minecraftforge/forge/json").Result)
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                mavenunjsonend = serializer.Deserialize<Forgemaven>(reader);
            }
            int concurrentgone = 0;
            int i = 1;
            while (concurrentgone <= 100)
            {
                if (mavenunjsonend.Number.ContainsKey(i))
                {
                    String mcversion = mavenunjsonend.Number[i].Mcversion;
                    String build = mavenunjsonend.Number[i].Build.ToString();
                    String version = mavenunjsonend.Number[i].Version;
                    String branch = mavenunjsonend.Number[i].Branch;
                    String downloadUrl = null;
                    downloadUrl = string.Format("{0}/{1}-{2}{3}/forge-{1}-{2}{3}-", mavenunjsonend.Webpath, mcversion, version, String.IsNullOrWhiteSpace(branch) ? "" : "-" + branch);
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
                    mcversions.Add(mcversion);
                    builds.Add(build);
                    versions.Add(version);
                    downloadUrls.Add(downloadUrl);
                    types.Add("universal");
                    concurrentgone = 0;
                }
                else
                {
                    concurrentgone += 1;
                }
                i++;

            }
            AddVersions(builds, mcversions, versions, types, downloadUrls);

        }

    }
}
