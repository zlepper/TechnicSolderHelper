using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Mono.Data.Sqlite;
using System.Diagnostics;

namespace TechnicSolderHelper.SQL
{
    public class FTBPermissionsSQLHelper : SQLHelper
    {
        protected readonly String CreateTableString;

        public FTBPermissionsSQLHelper()
            : base("FTBPermssions", "ftbperms")
        {
            CreateTableString = String.Format("CREATE TABLE IF NOT EXISTS `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT, `ModAuthor` TEXT, `ShortName` TEXT, `ModID` TEXT UNIQUE, `PublicPerm` TEXT , `PrivatePerm` TEXT, `ModLink` TEXT, `PermLink` TEXT, `CustPrivate` TEXT, `CustFTB` TEXT, PRIMARY KEY(ID));", this.TableName);
            executeDatabaseQuery(CreateTableString);
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
        public PermissionLevel doFTBHavePermission(String toCheck, Boolean isPublic, Boolean checkShortName)
        {
            toCheck = toCheck.Replace("'", "`");

            String sql = "";
            if (checkShortName)
            {
                sql = String.Format("SELECT PublicPerm, PrivatePerm FROM {0} WHERE ShortName LIKE '{1}';", this.TableName, toCheck.ToLower());
            }
            else
            {
                sql = String.Format("SELECT PublicPerm, PrivatePerm FROM {0} WHERE ModID LIKE '{1}';", this.TableName, toCheck.ToLower());
            }
            Debug.WriteLine(sql);
            if (isUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            String level = "";
                            while (reader.Read())
                            {
                                if (isPublic)
                                {
                                    level = reader["PublicPerm"].ToString();
                                }
                                else
                                {
                                    level = reader["PrivatePerm"].ToString();
                                }
                            }

                            switch (level.ToLower())
                            {
                                case "open":
                                    return PermissionLevel.Open;
                                case "closed":
                                    return PermissionLevel.Closed;
                                case "ftb":
                                    return PermissionLevel.FTB;
                                case "notify":
                                    return PermissionLevel.Notify;
                                case "request":
                                    return PermissionLevel.Request;
                                default:
                                    return PermissionLevel.Unknown;
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
                            String level = "";
                            while (reader.Read())
                            {
                                if (isPublic)
                                {
                                    level = reader["PublicPerm"].ToString();
                                }
                                else
                                {
                                    level = reader["PrivatePerm"].ToString();
                                }
                            }

                            switch (level.ToLower())
                            {
                                case "open":
                                    return PermissionLevel.Open;
                                case "closed":
                                    return PermissionLevel.Closed;
                                case "ftb":
                                    return PermissionLevel.FTB;
                                case "notify":
                                    return PermissionLevel.Notify;
                                case "request":
                                    return PermissionLevel.Request;
                                default:
                                    return PermissionLevel.Unknown;
                            }
                        }
                    }
                }
            }
        }

        public enum InfoType
        {
            PermLink,
            CustPrivate,
            CustFTB,
            ModLink,
            ModAuthor,
            ShortName,
            ModID,
            ModName
        }

        public String getInfoFromModID(String ModID, InfoType infoType)
        {
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("SELECT {2} FROM {0} WHERE ModID LIKE '{1}';", this.TableName, ModID.ToLower(), infoType.ToString());
            Debug.WriteLine(sql);

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
                                return reader[infoType.ToString()].ToString();
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
                                return reader[infoType.ToString()].ToString();
                            }
                        }
                    }
                }
            }
            return "";
        }

        public String getInfoFromShortName(String ShortName, InfoType infoType)
        {
            ShortName = ShortName.Replace("'", "`");

            String sql = String.Format("SELECT {2} FROM {0} WHERE ShortName LIKE '{1}';", this.TableName, ShortName.ToLower(), infoType.ToString());
            Debug.WriteLine(sql);

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
                                return reader[infoType.ToString()].ToString();
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
                                return reader[infoType.ToString()].ToString();
                            }
                        }
                    }
                }
            }
            return "";
        }

        public String getShortName(String ModID)
        {
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("SELECT ShortName FROM {0} WHERE ModID LIKE '{1}';", this.TableName, ModID);
            Debug.WriteLine(sql);

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
                                return reader["ShortName"].ToString();
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
                                return reader["ShortName"].ToString();
                            }
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Adds a mod to the FTB permission database
        /// </summary>
        /// <param name="ModName"></param>
        /// <param name="ModAuthor"></param>
        /// <param name="ModID"></param>
        /// <param name="PublicPermissions"></param>
        /// <param name="PrivatePermissions"></param>
        public void addFTBModPerm(String ModName, String ModAuthor, String ModID, String PublicPermissions, String PrivatePermissions, String ModLink, String PermLink, String CustPrivate, String CustFTB, String shortname)
        {
            ModName = ModName.Replace("'", "`");
            ModAuthor = ModAuthor.Replace("'", "`");
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("INSERT OR REPLACE INTO {0}(ModName, ModAuthor, ModID, PublicPerm, PrivatePerm, ModLink, PermLink, CustPrivate, CustFTB, ShortName) VALUES ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}', '{10}');", this.TableName, ModName, ModAuthor, ModID, PublicPermissions, PrivatePermissions, ModLink, PermLink, CustPrivate, CustFTB, shortname);
            //Debug.WriteLine(sql);

            executeDatabaseQuery(sql);
        }

        public void addFTBModPerm(String ModID, String ShortName)
        {
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("INSERT OR ABORT INTO {0}(ShortName, ModID) VALUES ('{1}', '{2}');", this.TableName, ShortName, ModID);
            executeDatabaseQuery(sql);
        }

        public override void resetTable()
        {
            base.resetTable();
            executeDatabaseQuery(CreateTableString);
        }
    }
}
