using System;
using System.Collections.Generic;
using System.Diagnostics;
using ModpackHelper.IO;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Utils.Config;
using MySql.Data.MySqlClient;

namespace ModpackHelper.Shared.Web.Solder
{
    /// <summary>
    /// A client that allows for manipulation of the solder mysql database
    /// </summary>
    public class SolderMySQLClient
    {
        private readonly string connectionString;
        private readonly string database;
        private readonly string prefix;

        /// <summary>
        /// Create a new client to interact with the solder mysql database
        /// </summary>
        public SolderMySQLClient(SolderLoginInfo sli)
        {
            connectionString = sli.GetConnectionString();
            this.database = sli.DatabaseSchema;
            this.prefix = sli.TablePrefix;
        }

        /// <summary>
        /// Create a new client, from the stored configs, to interact with the solder mysql database
        /// </summary>
        public SolderMySQLClient()
        {
            // Load configs
            ConfigHandler ch = new ConfigHandler();
            // Ensure that the data is valid
            if (ch.Configs?.SolderLoginInfo == null || !ch.Configs.SolderLoginInfo.IsValid())
            {
                throw new Exception("Logininfo has not been set yet.");
            }

            database = ch.Configs.SolderLoginInfo.DatabaseSchema;
            prefix = ch.Configs.SolderLoginInfo.TablePrefix;
            connectionString = ch.Configs.SolderLoginInfo.GetConnectionString();
        }

        /// <summary>
        /// Checks if the current connection works and can find a solder database
        /// </summary>
        /// <returns>True if the connections worked, otherwise false</returns>
        public bool TestConnection()
        {
            // Create a list of all the tables that should be in the database
            List<string> tables = new List<string>
            {
                prefix + "modversions",
                prefix + "mods",
                prefix + "modpacks",
                prefix + "clients",
                prefix + "client_modpack",
                prefix + "builds",
                prefix + "build_modversion"
            };
            // Try to connect and validate that the tables exists
            try
            {
                string sql = $"SHOW TABLES IN {database};";
                // Connect to the database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        // Start reading from the database
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Calculate the column to read from
                            string s = "Tables_in_" + database;
                            // Read from the database
                            while (reader.Read())
                            {
                                // Remove the table from the required list
                                tables.Remove(reader[s].ToString());
                            }
                            // If all tables were removed, then the database is valid
                            if (tables.Count == 0)
                            {
                                return true;
                            }
                            // If any tables are left, then the database is not valid
                            return false;
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                // Something went wrong, write it to the debug log, and return false
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        
        /// <summary>
        /// Updates the mods md5 value in the database
        /// </summary>
        /// <param name="mod">The mod to update</param>
        public void UpdateModversionMd5(Mcmod mod)
        {
            int id = GetModId(mod);
            string sql =
                $"UPDATE {database}.{prefix + "modversions"} SET md5=@md5 , updated_at=@update WHERE version LIKE @modversion AND mod_id LIKE @modid;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@md5", mod.JarMd5);
                    cmd.Parameters.AddWithValue("@modversion", $"{mod.Mcversion}-{mod.Version}");
                    cmd.Parameters.AddWithValue("@modid", id);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Adds a mod to solder.
        /// </summary>
        /// <param name="mod">The mod to add to solder</param>
        public void AddModToSolder(Mcmod mod)
        {
            string sql =
                $"INSERT INTO {database}.{prefix + "mods"}(name, description, author, link, pretty_name, created_at, updated_at) VALUES(@modslug, @descriptionValue, @authorValue, @linkValue, @name, @create, @update);";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modslug", mod.GetSafeModId());
                    cmd.Parameters.AddWithValue("@name", mod.Name);
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.Parameters.AddWithValue("@descriptionValue",
                        string.IsNullOrWhiteSpace(mod.Description) ? "" : mod.Description);
                    cmd.Parameters.AddWithValue("@authorValue", string.IsNullOrWhiteSpace(string.Join(", ", mod.AuthorList)) ? "" : string.Join(", ", mod.AuthorList));
                    cmd.Parameters.AddWithValue("@linkValue", string.IsNullOrWhiteSpace(mod.Url) ? "" : mod.Url);
                    if (GetModId(mod) == -1)
                        cmd.ExecuteNonQuery();
                }
            }

            AddNewModversionToSolder(mod);
        }

        /// <summary>
        /// Get the id of a mod on solder.
        /// </summary>
        /// <returns>Returns the modslug of the mod if found, otherwise returns -1.</returns>
        public int GetModId(Mcmod mod)
        {
            string sql = $"SELECT id FROM {database}.{prefix + "mods"} WHERE name LIKE @modname";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modname", mod.GetSafeModId());
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
        /// <returns>Returns true if the mod version is on solder, false if not. </returns>
        public bool IsModversionOnline(Mcmod mod)
        {
            string sql =
                $"SELECT id FROM {database}.{prefix + "modversions"} WHERE version LIKE @version AND mod_id LIKE @modslug;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@version", $"{mod.Mcversion}-{mod.Version}");
                    cmd.Parameters.AddWithValue("@modslug", mod.GetSafeModId());
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
        /// Adds a new mod version to Solder.
        /// </summary>
        /// <param name="mod">The mod to add to solder</param>
        public void AddNewModversionToSolder(Mcmod mod)
        {
            if (IsModversionOnline(mod))
                return;
            string sql =
                $"INSERT INTO {database}.{prefix + "modversions"}(mod_id, version, md5, created_at, updated_at) VALUES(@modslug, @version, @md5, @create, @update);";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modslug", mod.GetSafeModId());
                    cmd.Parameters.AddWithValue("@version", $"{mod.Mcversion}-{mod.Version}");
                    cmd.Parameters.AddWithValue("@md5", mod.JarMd5);
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
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
        public int GetModpackId(string modpackName)
        {
            string sql =
                $"SELECT id FROM {database}.{prefix + "modpacks"} WHERE slug LIKE @modpack OR name LIKE @modpack";
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
                            return Convert.ToInt32(reader["id"]);
                        }
                    }
                }
            }
            return -1;
        }

        public int CreateNewModpack(string modpackName)
        {
            int id = GetModpackId(modpackName);
            if (id != -1)
            {
                return id;
            }

            string sql =
                $"INSERT INTO {database}.{prefix + "modpacks"}(name, slug, created_at, updated_at, icon_md5, logo_md5, background_md5, recommended, latest, `order`, hidden, private, icon, logo, background) VALUES(@name, @slug, @create, @update, \"\",\"\",\"\",\"\",\"\", 0,1,0,0,0,0);";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", modpackName);
                    cmd.Parameters.AddWithValue("@slug", modpackName.ToLower().Replace(" ", "-"));
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            return GetModpackId(modpackName);
        }

        public void CreateModpackBuild(int modpackId, string version, string mcVersion, string javaVersion, int memory)
        {
            string sql =
                $"INSERT INTO {database}.{prefix + "builds"}(modpack_id, version, minecraft, is_published, private, created_at, updated_at, min_java, min_memory) VALUES(@modpack, @version, @mcVersion, 0, 0, @create, @update, @minJava, @minMemory);";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modpack", modpackId);
                    cmd.Parameters.AddWithValue("@version", version);
                    cmd.Parameters.AddWithValue("@mcVersion", mcVersion);
                    cmd.Parameters.AddWithValue("@create", DateTime.Now);
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.Parameters.AddWithValue("@minJava", string.IsNullOrWhiteSpace(javaVersion) ? "" : javaVersion);
                    cmd.Parameters.AddWithValue("@minMemory", memory);
                    cmd.ExecuteNonQuery();
                }
                sql = $"UPDATE {database}.{prefix + "modpacks"} SET updated_at=@update WHERE id LIKE @id;";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@update", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", modpackId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int GetBuildId(int modpackId, string version)
        {
            string sql =
                $"SELECT id FROM {database}.{prefix + "builds"} WHERE modpack_id LIKE @modpack AND version LIKE @version;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
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

        public int GetModversionId(int modId, Mcmod mod)
        {
            string version = $"{mod.Mcversion}-{mod.Version}";
            string sql =
                $"SELECT id FROM {database}.{prefix + "modversions"} WHERE mod_id LIKE @mod AND version LIKE @version";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
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

        private bool IsModversionInBuild(int build, int modversionId)
        {
            string sql =
                $"SELECT id FROM {database}.{prefix + "build_modversion"} WHERE modversion_id LIKE @version AND build_id LIKE @build;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
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
                string sql =
                    $"INSERT INTO {database}.{prefix + "build_modversion"}(modversion_id, build_id, created_at, updated_at) VALUES(@modslug, @buildid, @created, @updated);";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
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
                    sql = $"UPDATE {database}.{prefix + "builds"} SET updated_at=@update WHERE id LIKE @id;";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", build);
                        cmd.Parameters.AddWithValue("@update", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    int modpackid;
                    sql = $"SELECT modpack_id FROM {database}.{prefix + "builds"} WHERE id LIKE @buildid;";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@buildid", build);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();

                            modpackid = Convert.ToInt32(reader["modpack_id"].ToString());

                        }
                    }
                    sql = $"UPDATE {database}.{prefix + "modpacks"} SET updated_at=@update WHERE id LIKE @modpackid;";
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
