using System;
using System.Data.SQLite;
using Mono.Data.Sqlite;
using TechnicSolderHelper.OLD.confighandler;

namespace TechnicSolderHelper.OLD.SQL
{
    public class OwnPermissionsSqlHelper : SqlHelper
    {
        private readonly String _createTableString;

        public OwnPermissionsSqlHelper()
            : base("ownperm")
        {
            ConfigHandler ch = new ConfigHandler();
            try
            {
                if (Convert.ToInt32(ch.GetConfig("ownpermsversion")) < 3)
                {
                    _createTableString =
                        String.Format(
                            "DROP TABLE IF EXISTS `{0}`; CREATE TABLE `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT, `ModID` TEXT NOT NULL UNIQUE, `ModAuthor` TEXT, `PermLink` TEXT, `ModLink` TEXT, `LicenseLink` TEXT, PRIMARY KEY(ID));",
                            TableName);
                    ch.SetConfig("ownpermsversion", "3");
                }
                else
                {
                    _createTableString =
                        String.Format(
                            "CREATE TABLE IF NOT EXISTS `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT, `ModID` TEXT NOT NULL UNIQUE, `ModAuthor` TEXT, `PermLink` TEXT, `ModLink` TEXT, `LicenseLink` TEXT, PRIMARY KEY(ID));",
                            TableName);
                }
            }
            catch (Exception)
            {
                _createTableString =
                        String.Format(
                            "DROP TABLE IF EXISTS `{0}`; CREATE TABLE `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT, `ModID` TEXT NOT NULL UNIQUE, `ModAuthor` TEXT, `PermLink` TEXT, `ModLink` TEXT, `LicenseLink` TEXT, PRIMARY KEY(ID));",
                            TableName);
                ch.SetConfig("ownpermsversion", "3");
            }
            finally
            {
                ExecuteDatabaseQuery(_createTableString);
            }
        }

        /// <summary>
        /// Checks if the user already has their own permissions linked
        /// </summary>
        /// <param name="modId">
        /// The Mod ID to search for</param>
        /// <returns>Returns a ownPermission object, with hasPermission set to true if the user have permission</returns>
        public OwnPermissions DoUserHavePermission(String modId)
        {

            String sql = String.Format("SELECT * FROM {0} WHERE ModID LIKE @modid;", TableName);

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
                                p.HasPermission = true;
                                p.PermissionLink = reader["PermLink"].ToString();
                                p.ModLink = reader["ModLink"].ToString();
                                p.LicenseLink = reader["LicenseLink"].ToString();
                                if (String.IsNullOrWhiteSpace(p.PermissionLink) || String.IsNullOrWhiteSpace(p.ModLink))
                                {
                                    p.HasPermission = false;
                                }
                                return p;
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
                                p.HasPermission = true;
                                p.PermissionLink = reader["PermLink"].ToString();
                                p.ModLink = reader["ModLink"].ToString();
                                p.LicenseLink = reader["LicenseLink"].ToString();
                                if (String.IsNullOrWhiteSpace(p.PermissionLink) || String.IsNullOrWhiteSpace(p.ModLink))
                                {
                                    p.HasPermission = false;
                                }
                                return p;
                            }
                        }
                    }
                }
            }
            p.HasPermission = false;
            return p;
        }

        public String GetAuthor(String modId)
        {
            String sql = String.Format("SELECT ModAuthor FROM {0} WHERE ModID LIKE \"{1}\";", TableName, modId);

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
                                return reader.GetValue(0).ToString();
                            }
                        }
                    }
                }
            }
            return "";
        }

        public void AddOwnModLicense(String modname, String modId, String licenseLink)
        {
            String sql;
            if (IsModInDatabase(modId))
            {
                sql = string.Format("UPDATE {0} SET LicenseLink = @license WHERE ModID LIKE @modid", TableName);
            }
            else
            {
                sql =
                    String.Format(
                        "INSERT OR REPLACE INTO {0}(ModName, ModID, LicenseLink) VALUES (@modname, @modid, @license);",
                        TableName);
            }
            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@modname", modname);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.Parameters.AddWithValue("@license", licenseLink);
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
                        cmd.Parameters.AddWithValue("@modname", modname);
                        cmd.Parameters.AddWithValue("@modid", modId);
                        cmd.Parameters.AddWithValue("@license", licenseLink);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void AddOwnModLink(String modname, String modid, String modlink)
        {
            String sql;
            if (IsModInDatabase(modid))
            {
                sql = string.Format("UPDATE {0} SET ModLink = @ModLink WHERE ModID LIKE @modid;", TableName);
            }
            else
            {
                sql =
                    String.Format(
                        "INSERT OR REPLACE INTO {0}(ModName, ModID, ModLink) VALUES (@modname, @modid, @ModLink);",
                        TableName);
            }
            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@modname", modname);
                        cmd.Parameters.AddWithValue("@modid", modid);
                        cmd.Parameters.AddWithValue("@ModLink", modlink);
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
                        cmd.Parameters.AddWithValue("@modname", modname);
                        cmd.Parameters.AddWithValue("@modid", modid);
                        cmd.Parameters.AddWithValue("@ModLink", modlink);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void AddOwnModPerm(String modName, String modId, String permissionLink)
        {
            String sql;
            if (IsModInDatabase(modId))
            {
                sql = string.Format("UPDATE {0} SET PermLink = @permlink WHERE ModID LIKE @modid;", TableName);
            }
            else
            {
                sql =
                    String.Format(
                        "INSERT OR REPLACE INTO {0}(ModName, ModID, PermLink) VALUES (@modname, @modid, @permlink);",
                        TableName);
            }
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
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void AddOwnModPerm(String modName, String modId, String permissionLink, String modLink)
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

            String sql = String.Format(IsModInDatabase(modId) ? "UPDATE {0} SET ModAuthor = @author WHERE ModID LIKE @modid;" : "INSERT INTO {0}(ModAuthor, ModID) VALUES(@author, @modid);", TableName);

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

        private Boolean IsModInDatabase(String modid)
        {
            String sql = string.Format("SELECT ModID FROM {0} WHERE ModID LIKE @modid;", TableName);

            if (Globalfunctions.IsUnix())
            {
                using (SqliteConnection db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (SqliteCommand cmd = new SqliteCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@modid", modid);
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["ModID"].ToString().Equals(modid))
                                {
                                    return true;
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
                        cmd.Parameters.AddWithValue("@modid", modid);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["ModID"].ToString().Equals(modid))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public override void ResetTable()
        {
            base.ResetTable();
            ExecuteDatabaseQuery(_createTableString);
        }
    }
}
