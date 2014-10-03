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

            String sql = String.Format("SELECT PermLink FROM {0} WHERE ModID = '{1}';", this.TableName, ModID);
            Debug.WriteLine(sql);

            try
            {
                db.Open();
                SQLiteCommand command = new SQLiteCommand(sql, db);
                SQLiteDataReader reader = command.ExecuteReader();
                ownPermissions p = new ownPermissions();

                while (reader.Read())
                {
                    Debug.WriteLine(reader["PermLink"].ToString());
                    if (String.IsNullOrWhiteSpace(reader["PermLink"].ToString()))
                    {
                        p.hasPermission = false;
                        p.Link = null;
                        reader.Close();
                        db.Close();
                        break;
                    }
                    else
                    {
                        p.hasPermission = true;
                        p.Link = reader["PermLink"].ToString();
                        reader.Close();
                        db.Close();
                        break;
                    }
                }
                Debug.WriteLine("Nothing found");
                reader.Close();
                db.Close();
                return p;
            }
            catch (Exception e)
            {
                db.Close();
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.Source);
            }
            finally
            {
                db.Close();
            }

            ownPermissions pe = new ownPermissions();
            pe.hasPermission = false;
            pe.Link = null;
            db.Close();
            return pe;
        }

        public void addOwnModPerm(String ModName, String ModID, String PermissionLink)
        {
            ModName = ModName.Replace("'", "`");
            ModID = ModID.Replace("'", "`");
            Debug.WriteLine(db.State.ToString());

            String sql = String.Format("INSERT INTO {0}(ModName, ModID, PermLink) VALUES ('{1}','{2}','{3}');", this.TableName, ModName, ModID, PermissionLink);
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
