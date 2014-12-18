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

namespace TechnicSolderHelper.SQL
{

    class SolderSQLHandler
    {
        /// <summary>
        /// The string used to connect to the server of choice.
        /// </summary>
        private String connectionString;
        private String database;

        public SolderSQLHandler(String server, String userid, String password, String database)
        {
            connectionString = String.Format("server={0};userid={1};password={2};database={3}", server, userid, password, database);
            Debug.WriteLine(connectionString);
            this.database = database;
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
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a certain mod already exists on solder
        /// using the modid
        /// </summary>
        /// <param name="modid">The modid of the mod to check for. 
        /// Should follow to regex "[a-z0-9]*"</param>
        /// <returns>True if the mod exists, otherwise false</returns>
        public Boolean doesModExist(String modid)
        {
            String sql = "SELECT * FROM solderapi.mods WHERE name LIKE @modid";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@modid", modid);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.WriteLine("Found sometihng");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        


    }
}
