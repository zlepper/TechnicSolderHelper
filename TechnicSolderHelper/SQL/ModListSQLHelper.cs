using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using Mono.Data.Sqlite;

namespace TechnicSolderHelper.SQL
{
    public class ModInfo : IEquatable<ModInfo>
    {
        public int ID { get; set; }
        public string ModName { get; set; }
        public string ModID { get; set; }
        public string ModVersion { get; set; }
        public string MinecraftVersion { get; set; }
        public string FileName { get; set; }
        public string FileVersion { get; set; }
        public string MD5 { get; set; }
        public int OnSolder { get; set; }


        public bool Equals(ModInfo other)
        {
            if (other == null) return false;
            return (this.MD5.Equals(other.MD5));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + MD5.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            ModInfo other = obj as ModInfo;
            if (other != null)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }
    }

    public class ModListSqlHelper : SqlHelper
    {
        private static List<ModInfo> _modInfo;
        public ModListSqlHelper()
            : base("modlist")
        {
            var createTableString = String.Format("CREATE TABLE IF NOT EXISTS '{0}'('ID' INTEGER, 'ModName' TEXT, 'ModID' TEXT, 'ModVersion' TEXT, 'MinecraftVersion' TEXT, 'FileName' TEXT, 'FileVersion' TEXT, 'MD5' TEXT UNIQUE, 'OnSolder' NUMERIC, PRIMARY KEY(ID));", TableName);
            ExecuteDatabaseQuery(createTableString);
            if (_modInfo == null)
            {
                ReloadEverything();
            }
        }

        private void ReloadEverything()
        {
            string sql = String.Format("SELECT * FROM {0};", TableName);
            var beforeSort = new List<ModInfo>();
            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection conn = new SqliteConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var modinfo = new ModInfo
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    ModName = reader["ModName"].ToString(),
                                    ModID = reader["ModID"].ToString(),
                                    ModVersion = reader["ModVersion"].ToString(),
                                    MinecraftVersion = reader["MinecraftVersion"].ToString(),
                                    FileName = reader["FileName"].ToString(),
                                    FileVersion = reader["FileVersion"].ToString(),
                                    MD5 = reader["MD5"].ToString(),
                                    OnSolder = Convert.ToInt32(reader["OnSolder"])
                                };
                                beforeSort.Add(modinfo);
                            }
                        }
                    }
                }
            }
            else
            {
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var modinfo = new ModInfo
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    ModName = reader["ModName"].ToString(),
                                    ModID = reader["ModID"].ToString(),
                                    ModVersion = reader["ModVersion"].ToString(),
                                    MinecraftVersion = reader["MinecraftVersion"].ToString(),
                                    FileName = reader["FileName"].ToString(),
                                    FileVersion = reader["FileVersion"].ToString(),
                                    MD5 = reader["MD5"].ToString(),
                                    OnSolder = Convert.ToInt32(reader["OnSolder"])
                                };
                                beforeSort.Add(modinfo);
                            }
                        }
                    }
                }
            }
            _modInfo = beforeSort.OrderBy(m => m.ID).Distinct().ToList();
        }

        public void SaveData()
        {
            string sqlDelete = string.Format("DELETE FROM {0};", TableName);
            string sqlInsert = String.Format(
                        "INSERT OR REPLACE INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5', 'OnSolder') VALUES(@modname, @modid, @modversion, @minecraftversion, @filename, @fileversion, @md5, @onsolder);",
                        TableName);
            if (Globalfunctions.IsUnix())
            {
                using (var conn = new SqliteConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand(sqlDelete, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SqliteCommand(sqlInsert, conn))
                    {
                        foreach (var modInfo in _modInfo)
                        {
                            cmd.Parameters.AddWithValue("@modname", modInfo.ModName);
                            cmd.Parameters.AddWithValue("@modid", modInfo.ModID);
                            cmd.Parameters.AddWithValue("@modversion", modInfo.ModVersion);
                            cmd.Parameters.AddWithValue("@minecraftversion", modInfo.MinecraftVersion);
                            cmd.Parameters.AddWithValue("@filename", modInfo.FileName);
                            cmd.Parameters.AddWithValue("@fileversion", modInfo.FileVersion);
                            cmd.Parameters.AddWithValue("@md5", modInfo.MD5);
                            cmd.Parameters.AddWithValue("@onsolder", modInfo.OnSolder);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            else
            {
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(sqlDelete, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SQLiteCommand(sqlInsert, conn))
                    {
                        foreach (var modInfo in _modInfo)
                        {
                            cmd.Parameters.AddWithValue("@modname", modInfo.ModName);
                            cmd.Parameters.AddWithValue("@modid", modInfo.ModID);
                            cmd.Parameters.AddWithValue("@modversion", modInfo.ModVersion);
                            cmd.Parameters.AddWithValue("@minecraftversion", modInfo.MinecraftVersion);
                            cmd.Parameters.AddWithValue("@filename", modInfo.FileName);
                            cmd.Parameters.AddWithValue("@fileversion", modInfo.FileVersion);
                            cmd.Parameters.AddWithValue("@md5", modInfo.MD5);
                            cmd.Parameters.AddWithValue("@onsolder", modInfo.OnSolder);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
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
            //modName = modName.Replace("'", "`");
            //modVersion = modVersion.Replace("'", "`");
            //modId = modId.Replace("'", "`");
            //minecraftVersion = minecraftVersion.Replace("'", "`");
            //fileName = fileName.Replace("'", "`");
            String fileVersion = minecraftVersion + "-" + modVersion;
            //var sql = String.Format(onSolder ? "INSERT OR REPLACE INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5', 'OnSolder') values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '1');" : "INSERT OR IGNORE INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5', 'OnSolder') values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '0');", TableName, modName, modId, modVersion, minecraftVersion, fileName, fileVersion, md5Value);

            //ExecuteDatabaseQuery(sql, true);
            if (_modInfo.Count(m => m.MD5.Equals(md5Value)) != 0)
            {
                var mod = _modInfo.SingleOrDefault(m => m.MD5.Equals(md5Value));
                if (mod.OnSolder != (onSolder ? 1 : 0))
                {
                    mod.OnSolder = onSolder ? 1 : 0;
                }
            }
            else
            {
                int newid = 0;
                if (_modInfo.Count > 0)
                {
                     newid = _modInfo.Last().ID + 1;
                }
                var modinfo = new ModInfo
                {
                    ID = newid,
                    ModName = modName,
                    ModID = modId,
                    ModVersion = modVersion,
                    MinecraftVersion = minecraftVersion,
                    FileName = fileName,
                    FileVersion = fileVersion,
                    MD5 = md5Value,
                    OnSolder = onSolder ? 1 : 0
                };
                _modInfo.Add(modinfo);
            }
        }

        public Boolean IsFileInSolder(String filePath)
        {
            String md5Value = CalculateMd5(filePath);
            var mod = _modInfo.SingleOrDefault(m => m.MD5.Equals(md5Value));
            if (mod != null)
            {
                return mod.OnSolder == 1;
            }
            else
            {
                return false;
            }
        }

        public Mcmod GetModInfo(String md5Value)
        {
            var mod = _modInfo.SingleOrDefault(m => m.MD5.Equals(md5Value));
            if (mod == null) return null;
            return new Mcmod
            {
                Name = mod.ModName,
                Mcversion = mod.MinecraftVersion,
                Modid = mod.ModID,
                Version = mod.ModVersion
            };
        }

        public override void ResetTable()
        {
            String sql = String.Format("UPDATE {0} SET OnSolder = '0'", TableName);
            ExecuteDatabaseQuery(sql);
            ReloadEverything();
        }

        public DataTable GetTableInfoForEditing()
        {
            String sql =
                String.Format(
                    "SELECT ID, ModName, ModID, ModVersion, MinecraftVersion, FileName FROM {0};", TableName);
            DataTable table = new DataTable();
            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                            table.Load(reader);
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
                            table.Load(reader);
                    }
                }
            }
            return table;
        }

        /// <summary>
        /// Sets the modlist info from the table
        /// </summary>
        /// <param name="table">The full table to be reinserted</param>
        public void SetTableInfoAfterEditing(DataTable table)
        {
            foreach (DataRow dataRow in table.Rows)
            {
                switch (dataRow.RowState)
                {
                    case DataRowState.Deleted:
                        DeleteRow(dataRow["ID", DataRowVersion.Original].ToString());
                        break;
                    case DataRowState.Modified:
                        UpdateRow(dataRow["ID"].ToString(), dataRow["ModName"].ToString(), dataRow["ModID"].ToString(), dataRow["ModVersion"].ToString(), dataRow["MinecraftVersion"].ToString(), dataRow["FileName"].ToString());
                        break;
                }
            }
            ReloadEverything();
        }

        private void DeleteRow(String id)
        {

            String sql = String.Format("DELETE FROM {0} WHERE ID = @id;", TableName);
            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQueryAsync();
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
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        private void UpdateRow(String id, String modName, String modId, String modVersion, String minecraftVersion, String fileName)
        {
            String fileVersion = minecraftVersion + "-" + modVersion;
            String sql =
                String.Format(
                    "UPDATE {0} SET ModName = @modname, ModID = @modid, ModVersion = @modversion, MinecraftVersion = @minecraftversion, FileName = @filename, FileVersion = @fileversion, OnSolder = 0 WHERE ID = @id;",
                    TableName);
            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@modname", modName);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.Parameters.AddWithValue("@modversion", modVersion);
                        cmd.Parameters.AddWithValue("@minecraftversion", minecraftVersion);
                        cmd.Parameters.AddWithValue("@filename", fileName);
                        cmd.Parameters.AddWithValue("@fileversion", fileVersion);
                        cmd.ExecuteNonQueryAsync();
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
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@modname", modName);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.Parameters.AddWithValue("@modversion", modVersion);
                        cmd.Parameters.AddWithValue("@minecraftversion", minecraftVersion);
                        cmd.Parameters.AddWithValue("@filename", fileName);
                        cmd.Parameters.AddWithValue("@fileversion", fileVersion);
                        cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
}
