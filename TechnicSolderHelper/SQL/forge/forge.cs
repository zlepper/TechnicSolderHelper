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

namespace TechnicSolderHelper.forge
{
    public class FileInfo
    {
        public string branch { get; set; }
        public string buildnum { get; set; }
        public string buildtype { get; set; }
        public string ext { get; set; }
        public string jobbuildver { get; set; }
        public string jobname { get; set; }
        public string jobver { get; set; }
        public string mcver { get; set; }
        public string url { get; set; }
    }

    public class Build
    {
        public int build { get; set; }
        public List<FileInfo> files { get; set; }
        public string info { get; set; }
        public string version { get; set; }
    }

    public class Promotions
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Promotion
    {
        public List<Promotions> files { get; set; }
        public string name { get; set; }
    }

    public class Subsection
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Forge
    {
        public string adfly_id { get; set; }
        public List<Build> builds { get; set; }
        public List<Promotion> promotions { get; set; }
        public List<Subsection> subsections { get; set; }
    }





    public class ForgeSQLHelper : SQL.SQLHelper
    {

        protected readonly String CreateTableString;
        public ForgeSQLHelper()
            : base("Forge", "forge")
        {
            CreateTableString = "CREATE TABLE IF NOT EXISTS 'forge' ('totalversion' TEXT,'build' INTEGER UNIQUE, 'mcversion' TEXT, 'version' TEXT, 'downloadurl' TEXT, 'type' TEXT, PRIMARY KEY(totalversion));";
            executeDatabaseQuery(CreateTableString);
        }

        public void addVersion(String build, String mcversion, String version, String type, String downloadURL)
        {
            String totalversion = mcversion + "-" + version + "-" + build + "-" + type;

            String sql = String.Format("INSERT OR REPLACE INTO {0}('totalversion', 'build', 'mcversion', 'version', 'downloadurl', 'type') VALUES('{1}','{2}','{3}','{4}', '{5}','{6}');", this.TableName, totalversion, build, mcversion, version, downloadURL, type);
            //Debug.WriteLine(sql);
            executeDatabaseQuery(sql);
        }

        public List<String> getMCVersions()
        {
            String sql = String.Format("SELECT DISTINCT mcversion FROM {0} ORDER BY mcversion ASC;", this.TableName);
            List<String> mcversion = new List<string>();
			if (isUnix ()) {
				using (SqliteConnection db = new SqliteConnection (ConnectionString)) {
					db.Open ();
					using (SqliteCommand cmd = new SqliteCommand (sql, db)) {
						using (SqliteDataReader reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								mcversion.Add (reader ["mcversion"].ToString ());
							}

							return mcversion;
						}
					}
				}
			} else {
				using (SQLiteConnection db = new SQLiteConnection (ConnectionString)) {
					db.Open ();
					using (SQLiteCommand cmd = new SQLiteCommand (sql, db)) {
						using (SQLiteDataReader reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								mcversion.Add (reader ["mcversion"].ToString ());
							}

							return mcversion;
						}
					}
				}
			}
        }

        public List<String> getForgeVersions(String mcversion)
        {
            String sql = String.Format("SELECT DISTINCT build FROM {0} WHERE mcversion LIKE '{1}' ORDER BY mcversion ASC;", this.TableName, mcversion);
            List<String> forgeVersions = new List<string>();
			if (isUnix ()) {
				using (SqliteConnection db = new SqliteConnection (ConnectionString)) {
					db.Open ();
					using (SqliteCommand cmd = new SqliteCommand (sql, db)) {
						using (SqliteDataReader reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								forgeVersions.Add (reader ["build"].ToString ());
							}

							return forgeVersions;
						}
					}
				}
			} else {
				using (SQLiteConnection db = new SQLiteConnection (ConnectionString)) {
					db.Open ();
					using (SQLiteCommand cmd = new SQLiteCommand (sql, db)) {
						using (SQLiteDataReader reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								forgeVersions.Add (reader ["build"].ToString ());
							}

							return forgeVersions;
						}
					}
				}
			}
        }

        public Number getForgeInfo(String forgebuild)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE build LIKE '{1}';", this.TableName, forgebuild);
            Debug.WriteLine(sql);
			if (isUnix ()) {
				using (SqliteConnection db = new SqliteConnection (ConnectionString)) {
					db.Open ();
					using (SqliteCommand cmd = new SqliteCommand (sql, db)) {
						using (SqliteDataReader reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								Number build = new Number ();
								build.build = int.Parse (reader ["build"].ToString ());
								build.mcversion = reader ["mcversion"].ToString ();
								build.version = reader ["version"].ToString ();
								build.downloadurl = reader ["downloadurl"].ToString ();
								return build;
							}
						}
					}
				}
				return new Number ();
			} else {
				using (SQLiteConnection db = new SQLiteConnection (ConnectionString)) {
					db.Open ();
					using (SQLiteCommand cmd = new SQLiteCommand (sql, db)) {
						using (SQLiteDataReader reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								Number build = new Number ();
								build.build = int.Parse (reader ["build"].ToString ());
								build.mcversion = reader ["mcversion"].ToString ();
								build.version = reader ["version"].ToString ();
								build.downloadurl = reader ["downloadurl"].ToString ();
								return build;
							}
						}
					}
				}
				return new Number ();
			}

        }

        public void FindAllForgeVersion()
        {
            WebClient wb = new WebClient();
            String forgejsonweb = "http://files.minecraftforge.net/minecraftforge/json";
            String jsonfile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\forge.json";
			if (globalfunctions.isUnix ()) {
				jsonfile.Replace ("\\", "/");
			}
            wb.DownloadFile(forgejsonweb, jsonfile);
            Debug.WriteLine("Downloaded json file");
            String json = "";
            
            forgejsonweb = "http://files.minecraftforge.net/maven/net/minecraftforge/forge/json";
            wb.DownloadFile(forgejsonweb, jsonfile);
            json = "";
            using (StreamReader r = new StreamReader(jsonfile))
            {
                json = r.ReadToEnd();
            }
            Debug.WriteLine("readjson");
            forgemaven mavenunjsonend = JsonConvert.DeserializeObject<forgemaven>(json);
            int concurrentgone = 0;
            int i = 1;
            while (concurrentgone <= 100) {
                if (mavenunjsonend.number.ContainsKey(i))
                {
                    Debug.WriteLine(mavenunjsonend.number[i].build.ToString());
                    String jobversion = mavenunjsonend.number[i].jobver;
                    String mcversion = mavenunjsonend.number[i].mcversion;
                    String build = mavenunjsonend.number[i].build.ToString();
                    String version = mavenunjsonend.number[i].version;
                    String downloadURL = mavenunjsonend.webpath + "/" + mcversion + "-" + version + "/forge-" + mcversion + "-" + version + "-";
                    if (i < 183)
                    {
                        downloadURL += "client.";
                    }
                    else
                    {
                        downloadURL += "universal.";
                    }
                    if (i < 752)
                    {
                        downloadURL += "zip";

                    }
                    else
                    {
                        downloadURL += "jar";
                    }
                    Debug.WriteLine(downloadURL);
                    this.addVersion(build, mcversion, version, "universal", downloadURL);
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
