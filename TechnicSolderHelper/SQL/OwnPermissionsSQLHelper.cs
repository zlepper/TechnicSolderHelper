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
    public class OwnPermissionsSQLHelper : SQLHelper
    {
        protected readonly String CreateTableString;

        public OwnPermissionsSQLHelper()
            : base("OwnPermissions", "ownperm")
        {
            CreateTableString = String.Format("CREATE TABLE IF NOT EXISTS `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT NOT NULL, `ModID` TEXT NOT NULL UNIQUE, `ModAuthor` TEXT, `PermLink` TEXT NOT NULL, `ModLink` TEXT, PRIMARY KEY(ID));", this.TableName);
            executeDatabaseQuery(CreateTableString);
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

            String sql = String.Format("SELECT PermLink, ModLink FROM {0} WHERE ModID LIKE '{1}';", this.TableName, ModID);
            Debug.WriteLine(sql);

            bool didLoopRun = false;
            ownPermissions p = new ownPermissions();
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
                                didLoopRun = true;
                                p.hasPermission = true;
                                p.PermissionLink = reader["PermLink"].ToString();
                                p.ModLink = reader["ModLink"].ToString();
                                break;
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
                                didLoopRun = true;
                                p.hasPermission = true;
                                p.PermissionLink = reader["PermLink"].ToString();
                                p.ModLink = reader["ModLink"].ToString();
                                break;
                            }
                        }
                    }
                }
            }

            if (didLoopRun)
            {
                return p;
            }
            else
            {
                p.hasPermission = false;
                return p;
            }
        }

        public String getAuthor(String ModID)
        {
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("SELECT ModAuthor FROM {0} WHERE ModID LIKE '{1}';", this.TableName, ModID);
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
                                return reader["ModAuthor"].ToString();
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
                                return reader["ModAuthor"].ToString();
                            }
                        }
                    }
                }
            }
            return "";
        }

        public void addOwnModPerm(String ModName, String ModID, String PermissionLink)
        {
            addOwnModPerm(ModName, ModID, PermissionLink, "");
        }

        public void addOwnModPerm(String ModName, String ModID, String PermissionLink, String modLink)
        {
            ModName = ModName.Replace("'", "`");
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("INSERT OR REPLACE INTO {0}(ModName, ModID, PermLink, ModLink) VALUES ('{1}','{2}','{3}','{4}');", this.TableName, ModName, ModID, PermissionLink, modLink);
            Debug.WriteLine(sql);

            executeDatabaseQuery(sql);
        }

        public void addAuthor(String ModID, String AuthorName)
        {
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("INSERT OR REPLACE INTO {0}(ModAuthor, ModID)values('{2}','{1}');", this.TableName, ModID, AuthorName);
            Debug.WriteLine(sql);

            executeDatabaseQuery(sql);
        }

        public override void resetTable()
        {
            base.resetTable();
            executeDatabaseQuery(CreateTableString);
        }
    }
}
