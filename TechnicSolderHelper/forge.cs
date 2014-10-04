using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Diagnostics;

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
            CreateTableString = "CREATE TABLE IF NOT EXISTS 'forge' ('build' INTEGER UNIQUE, 'mcversion' TEXT, 'version' TEXT UNIQUE, 'downloadurl' TEXT, PRIMARY KEY(build));";
            executeDatabaseQuery(CreateTableString);
        }

        public void addVersion(String build, String mcversion, String version, String downloadURL)
        {
            String sql = String.Format("INSERT OR REPLACE INTO {0}('build', 'mcversion', 'version', 'downloadurl') VALUES('{1}','{2}','{3}','{4}');", this.TableName, build, mcversion, version, downloadURL);
            executeDatabaseQuery(sql);
        }

        public List<String> getMCVersions()
        {
            String sql = String.Format("SELECT DISTINCT mcversion FROM {0} ORDER BY mcversion ASC;", this.TableName);
            List<String> mcversion = new List<string>();
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

        public List<String> getForgeVersions(String mcversion)
        {
            String sql = String.Format("SELECT DISTINCT build FROM {0} WHERE mcversion LIKE '{1}' ORDER BY mcversion ASC;", this.TableName, mcversion);
            List<String> forgeVersions = new List<string>();
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

        public void FindAllForgeVersion()
        {
            WebClient wb = new WebClient();
            String forgejsonweb = "http://files.minecraftforge.net/minecraftforge/json";
            String jsonfile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\forge.json";
            wb.DownloadFile(forgejsonweb, jsonfile);
            Debug.WriteLine("Downloaded json file");
            String json = "";
            using (StreamReader r = new StreamReader(jsonfile))
            {
                json = r.ReadToEnd();
            }
            Debug.WriteLine("readjson");

            Forge unjsoned = JsonConvert.DeserializeObject<Forge>(json);
            Debug.WriteLine("Unjsoned");

            foreach (Build build in unjsoned.builds)
            {
                Debug.WriteLine(build.build.ToString());
                /*String version = build.version;
                String b = build.build.ToString();
                String mcversion = build.files[0].mcver;
                String downloadURL = build.files[]*/
                for (int i = 0; i < build.files.Count; i++)
                {
                    if (build.files[i].buildtype.Equals("universal") || build.files[i].buildtype.Equals("client"))
                    {
                        String jobversion = build.files[i].jobver;
                        String buildnum = build.files[i].buildnum;
                        String mcversion = build.files[i].mcver;
                        String downloadURL = build.files[i].url;
                        this.addVersion(buildnum, mcversion, jobversion, downloadURL);
                    }
                }
            }
        }

    }
}
