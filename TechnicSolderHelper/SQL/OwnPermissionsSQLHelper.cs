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

            String sql = String.Format("SELECT PermLink, ModLink FROM {0} WHERE ModID LIKE @modid;", TableName);

            bool didLoopRun = false;
            OwnPermissions p = new OwnPermissions();
            if (IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@modid", modId);
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
                        cmd.Parameters.AddWithValue("@modid", modId);
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
            String sql = String.Format("SELECT ModAuthor FROM {0} WHERE ModID LIKE @modid;", TableName);

            if (IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@modid", modId);
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
                        cmd.Parameters.AddWithValue("@modid", modId);
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

            String sql = String.Format("INSERT OR REPLACE INTO {0}(ModName, ModID, PermLink, ModLink) VALUES (@modname, @modid, @permlink, @modlink);", TableName);
            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@modname", modName);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.Parameters.AddWithValue("@permlink", permissionLink);
                        cmd.Parameters.AddWithValue("@modlink", modLink);
                        cmd.ExecuteNonQuery();
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
                        cmd.Parameters.AddWithValue("@modname", modName);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.Parameters.AddWithValue("@permlink", permissionLink);
                        cmd.Parameters.AddWithValue("@modlink", modLink);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void AddAuthor(String modId, String authorName)
        {
            if (String.IsNullOrWhiteSpace(modId))
            {
                return;
            }
            modId = modId.Replace("'", "`");

            String sql = String.Format("UPDATE {0} SET ModAuthor = @author WHERE ModID LIKE @modid;", TableName);

            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@author", authorName);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.ExecuteNonQuery();
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
                        cmd.Parameters.AddWithValue("@author", authorName);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public override void ResetTable()
        {
            base.ResetTable();
            ExecuteDatabaseQuery(_createTableString);
        }
    }
}
