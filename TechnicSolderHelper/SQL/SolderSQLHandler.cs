using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using TechnicSolderHelper.cryptography;
using TechnicSolderHelper.Confighandler;

namespace TechnicSolderHelper.SQL
{

    public class SolderSqlHandler
    {
        private readonly String _connectionString;
        private readonly String _database;
        private readonly String _prefix;

        public SolderSqlHandler(String address, String username, String password, String database, String prefix = "")
        {
            _connectionString = String.Format("address={0};username={1};password={2};database={3}", address, username, password, database);
            _database = database;
            _prefix = prefix;
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
            _prefix = ch.GetConfig("mysqlPrefix");
            _connectionString = String.Format("address={0};username={1};password={2};database={3}", address, username, password, _database);
        }

        /// <summary>
        /// Checks if the current connection works and can find a solder database
        /// </summary>
        /// <returns>True if the connections worked, otherwise false</returns>
        public void TestConnection()
        {
            List<String> tables = new List<string>
            {
                _prefix + "modversions",
                _prefix + "mods",
                _prefix + "modpacks",
                _prefix + "clients",
                _prefix + "client_modpack",
                _prefix + "builds",
                _prefix + "build_modversion"
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
                            string s = "Tables_in_" + _database;
                            while (reader.Read())
                            {
                                tables.Remove(reader[s].ToString());
                            }
                            if (tables.Count == 0)
                            {
                                MessageBox.Show("The database is alright");
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
            String sql = string.Format("UPDATE {0}.{1} SET md5=@md5 , updated_at=@update WHERE version LIKE @modversion AND mod_id LIKE @modid;", _database, _prefix + "modversions");
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
            String sql = string.Format("SELECT id FROM {0}.{1} WHERE slug LIKE @modpack OR name LIKE @modpack", _database, _prefix + "modpacks");
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
            String sql = string.Format("INSERT INTO {0}.{1}(name, description, author, link, pretty_name, created_at, updated_at) VALUES(@modslug, @descriptionValue, @authorValue, @linkValue, @name, @create, @update);", _database, _prefix + "mods");

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modslug", modslug);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.Parameters.AddWithValue("@descriptionValue",
                        String.IsNullOrWhiteSpace(description) ? "" : description);
                    cmd.Parameters.AddWithValue("@authorValue", String.IsNullOrWhiteSpace(author) ? "" : author);
                    cmd.Parameters.AddWithValue("@linkValue", String.IsNullOrWhiteSpace(link) ? "" : link);
                    if(GetModId(modslug) == -1)
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
            String sql = string.Format("SELECT id FROM {0}.{1} WHERE name LIKE @modname", _database, _prefix + "mods");
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
                            return Convert.ToInt32(reader["id"]);
                        }
                    }
                }
            }
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
            String sql = string.Format("SELECT id FROM {0}.{1} WHERE version LIKE @version AND mod_id LIKE @modslug;", _database, _prefix + "modversions");
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
            if (id == -1) return false;
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
            if (IsModversionOnline(modid, version))
                return;
            String sql = string.Format("INSERT INTO {0}.{1}(mod_id, version, md5, created_at, updated_at) VALUES(@modslug, @version, @md5, @create, @update);", _database, _prefix + "modversions");
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
            String sql = string.Format("INSERT INTO {0}.{1}(name, slug, created_at, updated_at, icon_md5, logo_md5, background_md5, recommended, latest, `order`, hidden, private, icon, logo, background) VALUES(@name, @slug, @create, @update, \"\",\"\",\"\",\"\",\"\", 0,1,0,0,0,0);", _database, _prefix + "modpacks");
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

        public void CreateModpackBuild(int modpackId, String version, String mcVersion, String javaVersion, int memory)
        {
            String sql = string.Format("INSERT INTO {0}.{1}(modpack_id, version, minecraft, is_published, private, created_at, updated_at, minecraft_md5, min_java, min_memory) VALUES(@modpack, @version, @mcVersion, 0, 0, @create, @update, 0, @minJava, @minMemory);", _database, _prefix + "builds");
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
                    cmd.Parameters.AddWithValue("@minJava", String.IsNullOrWhiteSpace(javaVersion) ? "" : javaVersion);
                    cmd.Parameters.AddWithValue("@minMemory", memory);
                    cmd.ExecuteNonQuery();
                }
                sql = string.Format("UPDATE {0}.{1} SET updated_at=@update WHERE id LIKE @id;", _database, _prefix + "modpacks");
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
            String sql = string.Format("SELECT id FROM {0}.{1} WHERE modpack_id LIKE @modpack AND version LIKE @version;", _database, _prefix + "builds");
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
                            return Convert.ToInt32(reader["id"].ToString());
                        }
                    }
                }
            }
            return -1;
        }

        public int GetModversionId(int modId, String version)
        {
            String sql = string.Format("SELECT id FROM {0}.{1} WHERE mod_id LIKE @mod AND version LIKE @version", _database, _prefix + "modversions");
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
                            return Convert.ToInt32(reader["id"].ToString());
                        }
                    }
                }
            }

            return -1;
        }

        private Boolean IsModversionInBuild(int build, int modversionId)
        {
            String sql = string.Format("SELECT id FROM {0}.{1} WHERE modversion_id LIKE @version AND build_id LIKE @build;", _database, _prefix + "build_modversion");
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
                String sql = string.Format("INSERT INTO {0}.{1}(modversion_id, build_id, created_at, updated_at) VALUES(@modslug, @buildid, @created, @updated);", _database, _prefix + "build_modversion");
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@modslug", modversionId);
                        cmd.Parameters.AddWithValue("@buildid", build);
                        cmd.Parameters.AddWithValue("@created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@updated", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    sql = string.Format("UPDATE {0}.{1} SET updated_at=@update WHERE id LIKE @id;", _database, _prefix + "builds");
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", build);
                        cmd.Parameters.AddWithValue("@update", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    int modpackid;
                    sql = string.Format("SELECT modpack_id FROM {0}.{1} WHERE id LIKE @buildid;", _database, _prefix + "builds");
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@buildid", build);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            
                                modpackid = Convert.ToInt32(reader["modpack_id"].ToString());
                            
                        }
                    }
                    sql = string.Format("UPDATE {0}.{1} SET updated_at=@update WHERE id LIKE @modpackid;", _database, _prefix + "modpacks");
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
