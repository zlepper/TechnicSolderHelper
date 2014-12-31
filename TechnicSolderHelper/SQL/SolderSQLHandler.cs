using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using TechnicSolderHelper.Confighandler;
using TechnicSolderHelper.cryptography;
using TechnicSolderHelper.Properties;

namespace TechnicSolderHelper.SQL
{

    public class SolderSqlHandler
    {
        private readonly String _connectionString;
        private readonly String _database;

        public SolderSqlHandler(String address, String username, String password, String database)
        {
            _connectionString = String.Format("address={0};username={1};password={2};database={3}", address, username, password, database);
            Debug.WriteLine(_connectionString);
            _database = database;
        }

        public SolderSqlHandler()
        {
            Crypto crypto = new Crypto();
            ConfigHandler ch = new ConfigHandler();
            try
            {
                String s = ch.GetConfig("mysqlPassword");
                if (String.IsNullOrWhiteSpace(s))
                {
                    ch.SetConfig("mysqlPassword", crypto.EncryptToString("password"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.InnerException);
                ch.SetConfig("mysqlPassword", crypto.EncryptToString("password"));
            }
            var password = crypto.DecryptString(ch.GetConfig("mysqlPassword"));
            var username = ch.GetConfig("mysqlUsername");
            var address = ch.GetConfig("mysqlAddress");
            _database = ch.GetConfig("mysqlDatabase");
            _connectionString = String.Format("address={0};username={1};password={2};database={3}", address, username, password, _database);
            Debug.WriteLine(_connectionString);
        }

        /// <summary>
        /// Checks if the current connection works and can find a solder database
        /// </summary>
        /// <returns>True if the connections worked, otherwise false</returns>
        public void TestConnection()
        {
            List<String> tables = new List<string>
            {
                "modversions",
                "mods",
                "modpacks",
                "clients",
                "client_modpack",
                "builds",
                "build_modversion"
            };
            try
            {
                String sql = String.Format("SHOW TABLES IN {0};", _database);
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Debug.WriteLine("Connection succesful");
                            string s = "Tables_in_" + _database;
                            while (reader.Read())
                            {
                                Debug.WriteLine(reader[s]);
                                tables.Remove(reader[s].ToString());
                            }
                            if (tables.Count == 0)
                            {
                                MessageBox.Show("The database isalright");
                                return;
                            }
                            MessageBox.Show("Some tables appears to be missing in the database. Please reconstruct it and try again.");
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.InnerException);
            }
        }

        public void UpdateModversionMd5(String modslug, String modversion, String md5)
        {
            int id = GetModId(modslug);
            String sql =
                String.Format(
                    "UPDATE {0}.modversions SET md5=@md5 , updated_at=@update WHERE version LIKE @modversion AND mod_id LIKE @modid;",
                    _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@md5", md5);
                    cmd.Parameters.AddWithValue("@modversion", modversion);
                    cmd.Parameters.AddWithValue("@modid", id);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Checks if a certain modpack exists.
        /// </summary>
        /// <param name="modpackName">The name of the modpack</param>
        /// <returns>True if the modpack exists, otherwise false.</returns>
        public int GetModpackId(String modpackName)
        {
            String sql = String.Format("SELECT id FROM {0}.modpacks WHERE slug LIKE @modpack OR name LIKE @modpack", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
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
        /// <param name="modslug">The slug. Cannot be null</param>
        /// <param name="description">The description of the mod. Can be null</param>
        /// <param name="author">The name of the mod author. Can be null</param>
        /// <param name="link">The link to the mod. Can be null</param>
        /// <param name="name">The pretty name of the mod. Cannot be null</param>
        public void AddModToSolder(String modslug, String description, String author, String link, String name)
        {
            String sql = String.Format("INSERT INTO {0}.mods(name, description, author, link, pretty_name, created_at, updated_at) VALUES(\"{1}\", descriptionValue, authorValue, linkValue, \"{2}\", @create, @update);", _database, modslug, name);
            
            sql = !String.IsNullOrWhiteSpace(description) ? sql.Replace("descriptionValue", "\"" + description + "\"") : sql.Replace(" descriptionValue,", String.Empty).Replace(" description,", String.Empty);

            sql = !String.IsNullOrWhiteSpace(author) ? sql.Replace("authorValue", "\"" + author + "\"") : sql.Replace(" authorValue,", String.Empty).Replace(" author,", String.Empty);

            sql = !String.IsNullOrWhiteSpace(link) ? sql.Replace("linkValue", "\"" + link + "\"") : sql.Replace(" linkValue,", String.Empty).Replace(" link,", String.Empty);
            Debug.WriteLine(sql);

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.ExecuteNonQuery(); 
                }
            }
        }

        /// <summary>
        /// Get the id of a mod on solder.
        /// </summary>
        /// <param name="slug">The modslug of the mod. Also known as the slug</param>
        /// <returns>Returns the modslug of the mod if found, otherwise returns -1.</returns>
        public int GetModId(String slug)
        {
            String sql = String.Format("SELECT id FROM {0}.mods WHERE name LIKE @modname", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
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
        /// <param name="modid">The modslug</param>
        /// <param name="version">The version</param>
        /// <returns>Returns true if the mod version is on solder, false if not. </returns>
        private Boolean IsModversionOnline(int modid, String version)
        {
            String sql = String.Format("SELECT id FROM {0}.modversions WHERE version LIKE @version AND mod_id LIKE @modslug;", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@modslug", modid);
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
        /// <param name="modid">The modslug</param>
        /// <param name="version">The version</param>
        /// <returns>Returns true if the mod version is on solder, false if not. </returns>
        public Boolean IsModversionOnline(String modid, String version)
        {
            int id = GetModId(modid);
            return IsModversionOnline(id, version);
        }

        /// <summary>
        /// Adds a new mod version to Solder.
        /// </summary>
        /// <param name="modid">The modslug</param>
        /// <param name="version">The mod version</param>
        /// <param name="md5">The MD5 value of the zip</param>
        public void AddNewModversionToSolder(int modid, String version, String md5)
        {
            if (IsModversionOnline(modid, version)) return;
            String sql = String.Format("INSERT INTO {0}.modversions(mod_id, version, md5, created_at, updated_at) VALUES(@modslug, @version, @md5, @create, @update);", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modslug", modid);
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@md5", md5);
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Adds a new mod version to Solder.
        /// </summary>
        /// <param name="modid">The modslug</param>
        /// <param name="version">The mod version</param>
        /// <param name="md5">The MD5 value of the zip</param>
        public void AddNewModversionToSolder(String modid, String version, String md5)
        {
            int id = GetModId(modid);
            AddNewModversionToSolder(id, version, md5);
        }

        public void CreateNewModpack(String modpackName, String modpackSlug)
        {
            String sql = String.Format("INSERT INTO {0}.modpacks(name, slug, created_at, updated_at) VALUES(@name, @slug, @create, @update);", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", modpackName);
                    cmd.Parameters.AddWithValue("@slug", modpackSlug);
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateModpackBuild(int modpackId, String version, String mcVersion)
        {
            String sql = String.Format("INSERT INTO {0}.builds(modpack_id, version, minecraft, is_published, private, created_at, updated_at) VALUES(@modpack, @version, @mcVersion, 0, 0, @create, @update);", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modpack", modpackId);
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@mcVersion", mcVersion);
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                sql = String.Format("UPDATE {0}.modpacks SET updated_at=@update WHERE id LIKE @id;", _database);
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", modpackId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int GetBuildId(int modpackId, String version)
        {
            String sql = String.Format("SELECT id FROM {0}.builds WHERE modpack_id LIKE @modpack AND version LIKE @version;", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modpack", modpackId);
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

        public int GetModversionId(int modId, String version)
        {
            String sql = String.Format("SELECT id FROM {0}.modversions WHERE mod_id LIKE @mod AND version LIKE @version", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@mod", modId);
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

        private Boolean IsModversionInBuild(int build, int modversionId)
        {
            String sql = String.Format("SELECT id FROM {0}.build_modversion WHERE modversion_id LIKE @version AND build_id LIKE @build;", _database);
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@version", modversionId);
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

        public void AddModversionToBuild(int build, int modversionId)
        {
            if (!IsModversionInBuild(build, modversionId))
            {
                String sql = String.Format("INSERT INTO {0}.build_modversion(modversion_id, build_id) VALUES(@modslug, @buildid);", _database);
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@modslug", modversionId);
                        cmd.Parameters.AddWithValue("@buildid", build);
                        cmd.ExecuteNonQuery();
                    }
                    sql = String.Format("UPDATE {0}.builds SET updated_at=@update WHERE id LIKE @id;", _database);
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", build);
                        cmd.Parameters.AddWithValue("@update", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    int modpackid;
                    sql = String.Format("SELECT modpack_id FROM {0}.builds WHERE id LIKE @buildid;", _database);
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@buildid", build);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            modpackid = Convert.ToInt32(reader["modpack_id"].ToString());
                        }
                    }
                    sql = String.Format("UPDATE {0}.modpacks SET updated_at=@update WHERE id LIKE @modpackid;",
                        _database);
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@update", DateTime.Now);
                        cmd.Parameters.AddWithValue("@modpackid", modpackid);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            
        }
    }
}
