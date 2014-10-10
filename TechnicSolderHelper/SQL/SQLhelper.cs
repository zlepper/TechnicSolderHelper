using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace TechnicSolderHelper.SQL
{
    public abstract class SQLHelper
    {
        public SQLHelper(String databaseName, String TableName) 
        {
            databaseName += ".sqlite";
			//databaseName = "C:\\" + databaseName;
            this.databaseName = databaseName;
			Debug.WriteLine (this.databaseName);
            try
            {
				throw new Exception();
                SQLiteConnection.CreateFile(this.databaseName);
            }
            catch (Exception)
            {
                //Debug.WriteLine(e.Message);
                //Debug.WriteLine(e.InnerException);
				try {
					File.Create(this.databaseName);
				} catch (Exception ex) {
					Debug.WriteLine ("Database already existing");
				}
            }
			SQLiteConnectionStringBuilder c = new SQLiteConnectionStringBuilder ();
			c.DataSource = this.databaseName;
			c.Version = 3;
            //this.ConnectionString = "Data Source=" + this.databaseName + ";Version=3;";
			this.ConnectionString = c.ConnectionString;
            this.TableName = TableName;
        }

        protected readonly String databaseName;
        protected readonly String TableName;
        protected readonly String ConnectionString;

        protected void executeDatabaseQuery(String sql)
        {
            executeDatabaseQuery(sql, false);
        }

        protected void executeDatabaseQueryAsync(String sql)
        {
            executeDatabaseQuery(sql, true);
        }

        protected void executeDatabaseQuery(String sql, Boolean Async)
        {
			Debug.WriteLine (this.ConnectionString);
			try{
            using (SQLiteConnection db = new SQLiteConnection(this.ConnectionString)) 
            {
                db.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, db))
                {
                    if (Async)
                    {
                        cmd.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
			}catch(Exception e) {
				Debug.WriteLine (e.Message);
				Debug.WriteLine (e.StackTrace);
			}
        }

        public virtual void resetTable()
        {
            String sql = String.Format("DROP TABLE {0};", this.TableName);
            executeDatabaseQuery(sql);
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
}