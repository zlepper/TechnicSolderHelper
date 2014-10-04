using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;

namespace TechnicSolderHelper.SQL
{
    public class ModListSQLHelper : SQLHelper
    {
        private readonly String CreateTableString;
        public ModListSQLHelper()
            : base("ModList", "modlist")
        {
            CreateTableString = String.Format("CREATE TABLE IF NOT EXISTS '{0}'('ID' INTEGER, 'ModName' TEXT, 'ModID' TEXT, 'ModVersion' TEXT, 'MinecraftVersion' TEXT, 'FileName' TEXT, 'FileVersion' TEXT, 'MD5' TEXT, PRIMARY KEY(ID));", this.TableName);
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
        public void addMod(String ModName, String modID, String ModVersion, String MinecraftVersion, String FileName, String MD5value)
        {

            ModName = ModName.Replace("'", "`");
            ModVersion = ModVersion.Replace("'", "`");
            modID = modID.Replace("'", "`");
            MinecraftVersion = MinecraftVersion.Replace("'", "`");
            FileName = FileName.Replace("'", "`");
            String FileVersion = MinecraftVersion + "-" + ModVersion;


            String sql = String.Format("INSERT INTO {0} ('ModName', 'ModID', 'ModVersion', 'MinecraftVersion', 'FileName', 'FileVersion', 'MD5') values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');",
                this.TableName, ModName, modID, ModVersion, MinecraftVersion, FileName, FileVersion, MD5value);
            //Debug.WriteLine(sql);
            executeDatabaseQuery(sql);
        }

        public Boolean IsFileInDatabase(String MD5Value)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE MD5 LIKE '{1}';", this.TableName, MD5Value);
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
                                return true;
                            }
                            else
                            {
                                return false;
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
            return mod;
            
        }

        public override void resetTable()
        {
            base.resetTable();
            executeDatabaseQuery(CreateTableString);
        }
    }
}
