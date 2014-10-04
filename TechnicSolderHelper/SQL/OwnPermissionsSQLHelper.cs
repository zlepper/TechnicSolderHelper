using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;

namespace TechnicSolderHelper.SQL
{
    public class OwnPermissionsSQLHelper : SQLHelper
    {
        private readonly String CreateTableString;
        public OwnPermissionsSQLHelper() : base("OwnPermissions", "ownperm") {
            CreateTableString = String.Format("CREATE TABLE IF NOT EXISTS `{0}` ( `ID` INTEGER NOT NULL, `ModName` TEXT NOT NULL, `ModID` TEXT NOT NULL, `PermLink` TEXT NOT NULL, PRIMARY KEY(ID));", this.TableName);
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

            String sql = String.Format("SELECT PermLink FROM {0} WHERE ModID LIKE '{1}';", this.TableName, ModID);
            Debug.WriteLine(sql);

            bool didLoopRun = false;
            ownPermissions p = new ownPermissions();
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
                            p.Link = reader["PermLink"].ToString();
                            break;
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

        public void addOwnModPerm(String ModName, String ModID, String PermissionLink)
        {
            ModName = ModName.Replace("'", "`");
            ModID = ModID.Replace("'", "`");

            String sql = String.Format("INSERT INTO {0}(ModName, ModID, PermLink) VALUES ('{1}','{2}','{3}');", this.TableName, ModName, ModID, PermissionLink);
            Debug.WriteLine(sql);

            executeDatabaseQuery(sql);
            Debug.WriteLine("EXECUTEd");
        }

        public override void resetTable()
        {
            base.resetTable();
            executeDatabaseQuery(CreateTableString);
        }
    }
}
