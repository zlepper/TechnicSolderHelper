using System;
using System.Data.SQLite;
using Mono.Data.Sqlite;

namespace TechnicSolderHelper.SQL
{
    public class OwnPermissionsSqlHelper : SqlHelper
    {
        private readonly String _createTableString;

        public OwnPermissionsSqlHelper()
            : base("ownperm")
        {
            _createTableString = String.Format("CREATE TABLE IF NOT EXISTS `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT NOT NULL, `ModID` TEXT NOT NULL UNIQUE, `ModAuthor` TEXT, `PermLink` TEXT NOT NULL, `ModLink` TEXT, PRIMARY KEY(ID));", TableName);
            ExecuteDatabaseQuery(_createTableString);
        }

        /// <summary>
        /// Checks if the user already has their own permissions linked
        /// </summary>
        /// <param name="modId">
        /// The Mod ID to search for</param>
        /// <returns>Returns a ownPermission object, with hasPermission set to true if the user have permission</returns>
        public OwnPermissions DoUserHavePermission(String modId)
        {
            modId = modId.Replace("'", "`");

            String sql = String.Format("SELECT PermLink, ModLink FROM {0} WHERE ModID LIKE '{1}';", TableName, modId);

            bool didLoopRun = false;
            OwnPermissions p = new OwnPermissions();
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
                                didLoopRun = true;
                                p.HasPermission = true;
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
                                p.HasPermission = true;
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
            p.HasPermission = false;
            return p;
        }

        public String GetAuthor(String modId)
        {
            modId = modId.Replace("'", "`");

            String sql = String.Format("SELECT ModAuthor FROM {0} WHERE ModID LIKE '{1}';", TableName, modId);

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

        public void AddOwnModPerm(String modName, String modId, String permissionLink, String modLink = "")
        {
            modName = modName.Replace("'", "`");
            modId = modId.Replace("'", "`");

            String sql = String.Format("INSERT OR REPLACE INTO {0}(ModName, ModID, PermLink, ModLink) VALUES ('{1}','{2}','{3}','{4}');", TableName, modName, modId, permissionLink, modLink);

            ExecuteDatabaseQuery(sql);
        }

        public void AddAuthor(String modId, String authorName)
        {
            modId = modId.Replace("'", "`");

            String sql = String.Format("UPDATE {0} SET ModAuthor = '{2}' WHERE ModID LIKE '{1}';", TableName, modId, authorName);

            ExecuteDatabaseQuery(sql);
        }

        public override void ResetTable()
        {
            base.ResetTable();
            ExecuteDatabaseQuery(_createTableString);
        }
    }
}
