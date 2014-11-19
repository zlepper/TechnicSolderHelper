using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Mono.Data.Sqlite;
using System.Diagnostics;
using System.Security.Cryptography;

namespace TechnicSolderHelper.SQL
{
    public class ModListSQLHelper : SQLHelper
    {
        protected readonly String CreateTableString;

        public ModListSQLHelper()
            : base("ModList", "modlist")
        {
            CreateTableString = String.Format("CREATE TABLE IF NOT EXISTS '{0}'('ID' INTEGER, 'ModName' TEXT, 'ModID' TEXT, 'ModVersion' TEXT, 'MinecraftVersion' TEXT, 'FileName' TEXT, 'FileVersion' TEXT, 'MD5' TEXT UNIQUE, 'OnSolder' NUMERIC, PRIMARY KEY(ID));", this.TableName);
            executeDatabaseQuery(CreateTableString);
        }

        /// <summary>
        /// Inserts a mod into the database
        /// </summary>
        /// <param name="ModName">
        /// Specifies the Mod Name
        /// </param>
        /// <param name="modID">
        /// Specifies the Mod ID
        /// </param>
        /// <param name="ModVersion">
        /// Specifies the modversion
        /// </param>
        /// <param name="MinecraftVersion">
        /// Specifies the Minecraft version
        /// </param>
        /// <param name="FileName">
        /// Specifies the file name. Make sure to remember file extension
        /// </param>
        /// <param name="MD5value">
        /// The MD5 value of the file</param>
        public void addMod(String ModName, String modID, String ModVersion, String MinecraftVersion, String FileName, String MD5value, Boolean OnSolder)
        {
            ModName = ModName.Replace("'", "`");
            ModVersion = ModVersion.Replace("'", "`");
            modID = modID.Replace("'", "`");
            MinecraftVersion = MinecraftVersion.Replace("'", "`");
            FileName = FileName.Replace("'", "`");
            String FileVersion = MinecraftVersion + "-" + ModVersion;
            String sql = "";
            if (OnSolder)
            {
                sql = String.Format("INSERT OR REPLACE INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5', 'OnSolder') values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '1');",
                    this.TableName, ModName, modID, ModVersion, MinecraftVersion, FileName, FileVersion, MD5value);
            }
            else
            {
                sql = String.Format("INSERT OR IGNORE INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5', 'OnSolder') values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '0');",
                    this.TableName, ModName, modID, ModVersion, MinecraftVersion, FileName, FileVersion, MD5value);
            }

            executeDatabaseQuery(sql);
        }

        public Boolean IsFileInSolder(String FilePath)
        {
            String MD5Value = SQLHelper.calculateMD5(FilePath);
            String sql = String.Format("SELECT * FROM {0} WHERE MD5 LIKE '{1}';", this.TableName, MD5Value);
            if (isUnix())
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
                                if (reader["MD5"].ToString().Equals(MD5Value))
                                {
                                    if (reader["OnSolder"].ToString().Equals("1"))
                                    {
                                        return true;
                                    }
                                    return false;
                                }
                                else
                                {
                                    return false;
                                }
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
                                if (reader["MD5"].ToString().Equals(MD5Value))
                                {
                                    if (reader["OnSolder"].ToString().Equals("1"))
                                    {
                                        return true;
                                    }
                                    return false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return false;

        }

        public mcmod getModInfo(String MD5Value)
        {
            mcmod mod = new mcmod();
            String sql = String.Format("SELECT * FROM {0} WHERE MD5 LIKE '{1}';", this.TableName, MD5Value);
            //Debug.WriteLine(sql);
            if (isUnix())
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
                                mod.name = reader["ModName"].ToString();
                                mod.mcversion = reader["MinecraftVersion"].ToString();
                                mod.modid = reader["ModID"].ToString();
                                mod.version = reader["ModVersion"].ToString();
                            }
                            return mod;
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
                                mod.name = reader["ModName"].ToString();
                                mod.mcversion = reader["MinecraftVersion"].ToString();
                                mod.modid = reader["ModID"].ToString();
                                mod.version = reader["ModVersion"].ToString();
                            }
                            return mod;
                        }
                    }
                }
            }

        }

        public override void resetTable()
        {
            String sql = String.Format("UPDATE {0} SET OnSolder = '0'", this.TableName);
            executeDatabaseQuery(sql);
        }
    }
}
