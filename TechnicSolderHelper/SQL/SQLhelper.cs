using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Collections;
using System.Configuration;

namespace TechnicSolderHelper.SQL
{
    public class SQLhelper
    {
        private String databaseName = "SolderHelper.sqlite";
        private SQLiteConnection db;
        private String TableName;

        public SQLhelper(String TableName)
        {
            this.TableName = TableName.ToLower();
            try
            {
                if (!File.Exists(databaseName)) {
                    SQLiteConnection.CreateFile(databaseName);
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine(e.Message);
                //Debug.WriteLine(e.InnerException);
            }

            this.db = new SQLiteConnection("Data Source=" + databaseName + ";Version=3;");
            String sql = "";
            if (this.TableName.Equals("mod", StringComparison.OrdinalIgnoreCase))
            {
                sql = String.Format("CREATE TABLE IF NOT EXISTS '{0}'('ID' INTEGER, 'ModName' TEXT, 'ModID' TEXT, 'ModVersion' TEXT, 'MinecraftVersion' TEXT, 'FileName' TEXT, 'FileVersion' TEXT, 'MD5' TEXT, PRIMARY KEY(ID));", this.TableName);
            }
            else
            {
                if (this.TableName.ToLower().Equals("ftbperms".ToLower()))
                {
                    sql = String.Format("CREATE TABLE IF NOT EXISTS `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT NOT NULL, `ModAuthor` TEXT NOT NULL, `ModID` TEXT NOT NULL, `PublicPerm` TEXT NOT NULL, `PrivatePerm` TEXT NOT NULL, PRIMARY KEY(ID));", this.TableName);
                }
                else
                {
                    if (this.TableName.ToLower().Equals("ownperms".ToLower()))
                    {
                        sql = String.Format("CREATE TABLE IF NOT EXISTS `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT NOT NULL, `ModAuthor` TEXT NOT NULL, `ModID` TEXT NOT NULL, `PermLink` TEXT NOT NULL, PRIMARY KEY(ID));", this.TableName);
                    }
                }
            }

            executeDatabaseQuery(sql, false);
            
        }

        private void executeDatabaseQuery(String sql)
        {
            executeDatabaseQuery(sql, false);
        }

        private void executeDatabaseQueryAsync(String sql)
        {
            executeDatabaseQuery(sql, true);
        }

        private void executeDatabaseQuery(String sql, Boolean Async)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, db);
                db.Open();
                if (Async)
                {
                    command.ExecuteNonQueryAsync();
                }
                else
                {
                    command.ExecuteNonQuery();
                }
                db.Close();
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                //Debug.WriteLine(e.Message);
                //Debug.WriteLine(e.InnerException);
                //Debug.WriteLine(e.ErrorCode);
                //Debug.WriteLine(e.StackTrace);
                throw new System.Data.SQLite.SQLiteException();
            }
            finally
            {
                db.Close();
            }
        }

        /// <summary>
        /// Checks if the ftb database contains any permissions for the mod
        /// </summary>
        /// <param name="ModID">
        /// The ID of the mod to check for</param>
        /// <param name="isPublic">
        /// If false, Checks for private distribution permissions. 
        /// If true, Checks for public distribution permissions.</param>
        /// <returns>Return the level of distribution.
        /// If no level found, return PermissionLevel.Unknown</returns>
        public PermissionLevel doFTBHavePermission(String ModID, Boolean isPublic)
        {
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("SELECT PublicPerm, PrivatePerm FROM {0} WHERE ModID = '{1}';", this.TableName, ModID);
            SQLiteDataReader reader;
            try
            {
                db.Open();
                SQLiteCommand command = new SQLiteCommand(sql, db);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (isPublic)
                    {
                        switch (reader["PublicPerm"].ToString())
                        {
                            case "Open":
                                reader.Close();
                                return PermissionLevel.Open;
                            case "Closed":
                                reader.Close();
                                return PermissionLevel.Closed;
                            case "FTB":
                                reader.Close();
                                return PermissionLevel.FTB;
                            case "Notify":
                                reader.Close();
                                return PermissionLevel.Notify;
                            case "Request":
                                reader.Close();
                                return PermissionLevel.Request;
                            default:
                                reader.Close();
                                return PermissionLevel.Unknown;
                        }
                    }
                    else
                    {
                        switch (reader["PrivatePerm"].ToString())
                        {
                            case "Open":
                                reader.Close();
                                return PermissionLevel.Open;
                            case "Closed":
                                reader.Close();
                                return PermissionLevel.Closed;
                            case "FTB":
                                reader.Close();
                                return PermissionLevel.FTB;
                            case "Notify":
                                reader.Close();
                                return PermissionLevel.Notify;
                            case "Request":
                                reader.Close();
                                return PermissionLevel.Request;
                            default:
                                reader.Close();
                                return PermissionLevel.Unknown;
                        }
                    }
                }

            }
            catch (Exception)
            {
                //Debug.WriteLine(e.Message);
            }
            finally
            {
                db.Close();
            }

            return PermissionLevel.Unknown;
        }

        /// <summary>
        /// Checks if the user already has their own permissions linked
        /// </summary>
        /// <param name="ModID">
        /// The Mod ID to search for</param>
        /// <returns>Returns a ownPermission object, with hasPermission set to true if the user have permission</returns>
        public ownPermissions doUserHavePermission(String ModID)
        {
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("SELECT PermLink FROM {0} WHERE ModID = '{1}';", this.TableName, ModID);

            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, db);
                db.Open();
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (String.IsNullOrWhiteSpace(reader["PermLink"].ToString()))
                    {
                        ownPermissions p = new ownPermissions();
                        p.hasPermission = false;
                        p.Link = null;
                        reader.Close();
                        return p;
                    }
                    else
                    {
                        ownPermissions p = new ownPermissions();
                        p.hasPermission = true;
                        p.Link = reader["PermLink"].ToString();
                        reader.Close();
                        return p;
                    }
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine(e.Message);
            }
            finally
            {
                db.Close();
            }

            ownPermissions pe = new ownPermissions();
            pe.hasPermission = false;
            pe.Link = null;
            return pe;
        }

        /// <summary>
        /// Adds a mod to the FTB permission database
        /// </summary>
        /// <param name="ModName"></param>
        /// <param name="ModAuthor"></param>
        /// <param name="ModID"></param>
        /// <param name="PublicPermissions"></param>
        /// <param name="PrivatePermissions"></param>
        public void addFTBModPerm(String ModName, String ModAuthor, String ModID, String PublicPermissions, String PrivatePermissions)
        {
            ModName = ModName.Replace("'", "`");
            ModAuthor = ModAuthor.Replace("'", "`");
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("INSERT INTO {0}(ModName, ModAuthor, ModID, PublicPerm, PrivatePerm) VALUES ('{1}','{2}','{3}','{4}','{5}');", this.TableName, ModName, ModAuthor, ModID, PublicPermissions, PrivatePermissions);
            //Debug.WriteLine(sql);

            executeDatabaseQuery(sql);
        }

        public void addOwnModPerm(String ModName, String ModAuthor, String ModID, String PermissionLink)
        {
            ModName = ModName.Replace("'", "`");
            ModAuthor = ModAuthor.Replace("'", "`");
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("INSERT INTO {0}(ModName, ModAuthor, ModID, PermLink) VALUES ({1},{2},{3},{4});", this.TableName, ModName, ModAuthor, ModID, PermissionLink);
            //Debug.WriteLine(sql);

            executeDatabaseQuery(sql);
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
        public void addDoneMod(String ModName, String modID, String ModVersion, String MinecraftVersion, String FileName, String MD5value)
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

        public void DropTable() {
            String sql = String.Format("DROP TABLE {0};", this.TableName);

            executeDatabaseQuery(sql);

        }

        public Boolean IsFileInDatabase(String MD5Value)
        {
            String sql = String.Format("SELECT * FROM {0} WHERE MD5 = '{1}';", this.TableName, MD5Value);
            //Debug.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, db);
            SQLiteDataReader reader;
            try
            {
                db.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //Debug.WriteLine(reader["MD5"].ToString());
                    if (reader["MD5"].ToString().Equals(MD5Value))
                    {
                        //Debug.WriteLine(reader["MD5"].ToString() + " == ");
                        //Debug.WriteLine(MD5Value);
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        //Debug.WriteLine(reader["MD5"].ToString() + " != ");
                        //Debug.WriteLine(MD5Value);
                        return false;
                    }
                }
                return false;
            }
            catch (System.NullReferenceException)
            {
                //Debug.WriteLine(e.Message);
                //Debug.WriteLine(e.InnerException);
                //Debug.WriteLine(e.StackTrace);
                return false;
            }
            catch (Exception)
            {
                //Debug.WriteLine(e.Message);
                //Debug.WriteLine(e.InnerException);
                //Debug.WriteLine(e.StackTrace);
                return false;
            }
            finally
            {
                db.Close();
            }

                
            
        }

        public static String calculateMD5(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

    }

    public class mod
    {
        public int ID { get; set; }
        public String ModName { get; set; }
        public String ModID { get; set; }
        public String ModVersion { get; set; }
        public String MinecraftVersion { get; set; }
        public String FileName { get; set; }
        public String FileVersion { get; set; }
        public String MD5 { get; set; }
    }
}
