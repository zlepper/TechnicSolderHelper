using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using TechnicSolderHelper.confighandler;
using TechnicSolderHelper.cryptography;

namespace TechnicSolderHelper.SQL
{

    public class SolderSQLHandler
    {
        /// <summary>
        /// The string used to connect to the address of choice.
        /// </summary>
        private String connectionString;
        private String database;

        public SolderSQLHandler(String address, String username, String password, String database)
        {
            connectionString = String.Format("address={0};username={1};password={2};database={3}", address, username, password, database);
            Debug.WriteLine(connectionString);
            this.database = database;
        }

        public SolderSQLHandler()
        {
            String password = "", username = "", address = "";
            Crypto crypto = new Crypto();
            ConfigHandler ch = new ConfigHandler();
            if (String.IsNullOrWhiteSpace(ch.getConfig("mysqlPassword")))
            {
                ch.setConfig("mysqlPassword", crypto.EncryptToString("password"));
            }
            password = crypto.DecryptString(ch.getConfig("mysqlPassword"));
            username = ch.getConfig("mysqlUsername");
            address = ch.getConfig("mysqlAddress");
            this.database = ch.getConfig("mysqlDatabase");
            connectionString = String.Format("address={0};username={1};password={2};database={3}", address, username, password, database);
            Debug.WriteLine(connectionString);
        }

        /// <summary>
        /// Checks if the current connection works and can find a solder database
        /// </summary>
        /// <returns>True if the connections worked, otherwise false</returns>
        public Boolean testConnection()
        {
            List<String> tables = new List<string>();
            tables.Add("users");
            tables.Add("user_permissions");
            tables.Add("modversions");
            tables.Add("mods");
            tables.Add("modpacks");
            tables.Add("laravel_migrations");
            tables.Add("keys");
            tables.Add("clients");
            tables.Add("client_modpack");
            tables.Add("builds");
            tables.Add("build_modversion");
            try
            {
                String sql = String.Format("SHOW TABLES IN {0};", database);
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Debug.WriteLine("Connection succesful");
                            string s = "Tables_in_" + database;
                            while (reader.Read())
                            {
                                Debug.WriteLine(reader[s]);
                                tables.Remove(reader[s].ToString());
                            }
                            if (tables.Count == 0)
                            {
                                MessageBox.Show("The database is alright.");
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Some tables appears to be missing in the database. Please reconstruct it and try again.");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                MessageBox.Show(e.Message);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.InnerException);
                return false;
            }
        }

        /// <summary>
        /// Checks if a certain modpack exists.
        /// </summary>
        /// <param name="modpackName">The name of the modpack</param>
        /// <returns>True if the modpack exists, otherwise false.</returns>
        public int getModpackID(String modpackName)
        {
            String sql = String.Format("SELECT id FROM {0}.modpacks WHERE slug LIKE @modpack OR name LIKE @modpack", database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modpack", modpackName);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine(Convert.ToInt32(reader["id"]));
                            return Convert.ToInt32(reader["id"]);
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Adds a mod to solder.
        /// </summary>
        /// <param name="slug">The slug. Cannon be null</param>
        /// <param name="description">The description of the mod. Can be null</param>
        /// <param name="author">The name of the mod author. Can be null</param>
        /// <param name="link">The link to the mod. Can be null</param>
        /// <param name="name">The pretty name of the mod. Cannot be null</param>
        public void addModToSolder(String modslug, String description, String author, String link, String name)
        {
            String sql = String.Format("INSERT INTO {0}.mods(name, description, author, link, pretty_name) VALUES(\"{1}\", descriptionValue, authorValue, linkValue, \"{2}\");", database, modslug, name);
            
            if (!String.IsNullOrWhiteSpace(description))
            {
                sql = sql.Replace("descriptionValue", "\"" + description + "\"");
            }
            else
            {
                sql = sql.Replace(" descriptionValue,", String.Empty).Replace(" description,", String.Empty);
            }

            if (!String.IsNullOrWhiteSpace(author))
            {
                sql = sql.Replace("authorValue", "\"" + author + "\"");
            }
            else
            {
                sql = sql.Replace(" authorValue,", String.Empty).Replace(" author,", String.Empty);
            }

            if (!String.IsNullOrWhiteSpace(link))
            {
                sql = sql.Replace("linkValue", "\"" + link + "\"");
            }
            else
            {
                sql = sql.Replace(" linkValue,", String.Empty).Replace(" link,", String.Empty);
            }
            Debug.WriteLine(sql);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery(); 
                }
            }
        }

        /// <summary>
        /// Get the id of a mod on solder.
        /// </summary>
        /// <param name="slug">The modid of the mod. Also known as the slug</param>
        /// <returns>Returns the modid of the mod if found, otherwise returns -1.</returns>
        public int getModID(String slug)
        {
            String sql = String.Format("SELECT id FROM {0}.mods WHERE name LIKE @modname", this.database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modname", slug);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine(Convert.ToInt32(reader["id"]));
                            return Convert.ToInt32(reader["id"]);
                        }
                    }
                }
            }
            Debug.WriteLine("Couldn't find mod");
            return -1;
        }

        /// <summary>
        /// Checks if a certain mod version is already on Solder.
        /// </summary>
        /// <param name="modid">The modid</param>
        /// <param name="version">The version</param>
        /// <returns>Returns true if the mod version is on solder, false if not. </returns>
        public Boolean isModversionOnline(int modid, String version)
        {
            String sql = String.Format("SELECT id FROM {0}.modversions WHERE version LIKE @version AND mod_id LIKE @modid;", database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@modid", modid);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine("Mod version is already online.");
                            Debug.WriteLine(Convert.ToInt32(reader["id"]));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a certain mod version is already on Solder.
        /// </summary>
        /// <param name="modid">The modid</param>
        /// <param name="version">The version</param>
        /// <returns>Returns true if the mod version is on solder, false if not. </returns>
        public Boolean isModversionOnline(String modid, String version)
        {
            int id = getModID(modid);
            return isModversionOnline(id, version);
        }

        /// <summary>
        /// Adds a new mod version to Solder.
        /// </summary>
        /// <param name="modid">The modid</param>
        /// <param name="version">The mod version</param>
        /// <param name="md5">The MD5 value of the zip</param>
        public void addNewModversionToSolder(int modid, String version, String md5)
        {
            if (!isModversionOnline(modid, version))
            {
                String sql = String.Format("INSERT INTO {0}.modversions(mod_id, version, md5) VALUES(@modid, @version, @md5);", database);
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@modid", modid);
                        cmd.Parameters.AddWithValue("@version", version);
                        cmd.Parameters.AddWithValue("@md5", md5);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new mod version to Solder.
        /// </summary>
        /// <param name="modid">The modid</param>
        /// <param name="version">The mod version</param>
        /// <param name="md5">The MD5 value of the zip</param>
        public void addNewModversionToSolder(String modid, String version, String md5)
        {
            int id = getModID(modid);
            addNewModversionToSolder(id, version, md5);
        }

        public void createNewModpack(String modpackName, String modpackSlug)
        {
            String sql = String.Format("INSERT INTO {0}.modpacks(name, slug) VALUES(@name, @slug);", database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", modpackName);
                    cmd.Parameters.AddWithValue("@slug", modpackSlug);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void createModpackBuild(int modpackID, String version, String mcVersion)
        {
            String sql = String.Format("INSERT INTO {0}.builds(modpack_id, version, minecraft, is_published, private) VALUES(@modpack, @version, @mcVersion, 0, 0);", database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modpack", modpackID);
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@mcVersion", mcVersion);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void createModpackBuild(String modpackID, String version, String mcVersion)
        {
            int id = getModID(modpackID);
            createModpackBuild(id, version, mcVersion);
        }

        public int getBuildID(int modpackID, String version)
        {
            String sql = String.Format("SELECT id FROM {0}.builds WHERE modpack_id LIKE @modpack AND version LIKE @version;", database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modpack", modpackID);
                    cmd.Parameters.AddWithValue("@version", version);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine(reader["id"].ToString());
                            return Convert.ToInt32(reader["id"].ToString());
                        }
                    }
                }
            }
            return -1;
        }

        public int getModversionID(int modID, String version)
        {
            String sql = String.Format("SELECT id FROM {0}.modversions WHERE mod_id LIKE @mod AND version LIKE @version", database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@mod", modID);
                    cmd.Parameters.AddWithValue("@version", version);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine(reader["id"].ToString());
                            return Convert.ToInt32(reader["id"].ToString());
                        }
                    }
                }
            }

            return -1;
        }

        private Boolean isModversionInBuild(int build, int modversionID)
        {
            String sql = String.Format("SELECT id FROM {0}.build_modversion WHERE modversion_id LIKE @version AND build_id LIKE @build;", database);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@version", modversionID);
                    cmd.Parameters.AddWithValue("@build", build);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void addModversionToBuild(int build, int modversionID)
        {
            if (!isModversionInBuild(build, modversionID))
            {
                String sql = String.Format("INSERT INTO {0}.build_modversion(modversion_id, build_id) VALUES(@modid, @buildid);", database);
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@modid", modversionID);
                        cmd.Parameters.AddWithValue("@buildid", build);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            
        }
    }
}
