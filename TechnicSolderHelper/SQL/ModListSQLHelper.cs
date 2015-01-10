using System;
using System.Data.SQLite;
using Mono.Data.Sqlite;
using System.Diagnostics;

namespace TechnicSolderHelper.SQL
{
    public class ModListSqlHelper : SqlHelper
    {
        public ModListSqlHelper()
            : base("modlist")
        {
            var createTableString = String.Format("CREATE TABLE IF NOT EXISTS '{0}'('ID' INTEGER, 'ModName' TEXT, 'ModID' TEXT, 'ModVersion' TEXT, 'MinecraftVersion' TEXT, 'FileName' TEXT, 'FileVersion' TEXT, 'MD5' TEXT UNIQUE, 'OnSolder' NUMERIC, PRIMARY KEY(ID));", TableName);
            ExecuteDatabaseQuery(createTableString);
        }

        /// <summary>
        /// Inserts a mod into the database
        /// </summary>
        /// <param name="modName">
        /// Specifies the Mod Name
        /// </param>
        /// <param name="modId">
        /// Specifies the Mod ID
        /// </param>
        /// <param name="modVersion">
        /// Specifies the modversion
        /// </param>
        /// <param name="minecraftVersion">
        /// Specifies the Minecraft version
        /// </param>
        /// <param name="fileName">
        /// Specifies the file name. Make sure to remember file extension
        /// </param>
        /// <param name="md5Value">
        /// The MD5 value of the file</param>
        /// <param name="onSolder">
        /// If the mod is added to a solder instance or just a zip folder. 
        /// </param>
        public void AddMod(String modName, String modId, String modVersion, String minecraftVersion, String fileName, String md5Value, Boolean onSolder)
        {
            modName = modName.Replace("'", "`");
            modVersion = modVersion.Replace("'", "`");
            modId = modId.Replace("'", "`");
            minecraftVersion = minecraftVersion.Replace("'", "`");
            fileName = fileName.Replace("'", "`");
            String fileVersion = minecraftVersion + "-" + modVersion;
            var sql = String.Format(onSolder ? "INSERT OR REPLACE INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5', 'OnSolder') values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '1');" : "INSERT OR IGNORE INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5', 'OnSolder') values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '0');", TableName, modName, modId, modVersion, minecraftVersion, fileName, fileVersion, md5Value);

            ExecuteDatabaseQuery(sql);
        }

        public Boolean IsFileInSolder(String filePath)
        {
            String md5Value = CalculateMd5(filePath);
            String sql = String.Format("SELECT * FROM {0} WHERE MD5 LIKE '{1}';", TableName, md5Value);
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
                                if (reader["MD5"].ToString().Equals(md5Value))
                                {
                                    if (reader["OnSolder"].ToString().Equals("1"))
                                    {
                                        return true;
                                    }
                                    return false;
                                }
                                return false;
                            }
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
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["MD5"].ToString().Equals(md5Value))
                                {
                                    if (reader["OnSolder"].ToString().Equals("1"))
                                    {
                                        return true;
                                    }
                                    return false;
                                }
                                return false;
                            }
                        }
                    }
                }
            }

            return false;

        }

        public Mcmod GetModInfo(String md5Value)
        {
            Mcmod mod = new Mcmod();
            String sql = String.Format("SELECT * FROM {0} WHERE MD5 LIKE @md5;", TableName);
            Debug.WriteLine(sql);
            if (IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@md5", md5Value);
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mod.Name = reader["ModName"].ToString();
                                mod.Mcversion = reader["MinecraftVersion"].ToString();
                                mod.Modid = reader["ModID"].ToString();
                                mod.Version = reader["ModVersion"].ToString();
                            }
                            return mod;
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
                            mod.Name = reader["ModName"].ToString();
                            mod.Mcversion = reader["MinecraftVersion"].ToString();
                            mod.Modid = reader["ModID"].ToString();
                            mod.Version = reader["ModVersion"].ToString();
                        }
                        return mod;
                    }
                }
            }
        }

        public override void ResetTable()
        {
            String sql = String.Format("UPDATE {0} SET OnSolder = '0'", TableName);
            ExecuteDatabaseQuery(sql);
        }
    }
}
