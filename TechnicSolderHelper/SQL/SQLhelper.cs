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

            this.databaseName = databaseName;

            try
            {
                if (!File.Exists(databaseName))
                {
                    SQLiteConnection.CreateFile(databaseName);
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine(e.Message);
                //Debug.WriteLine(e.InnerException);
            }

            this.db = this.db = new SQLiteConnection("Data Source=" + this.databaseName + ";Version=3;");
            this.TableName = TableName;
            this.db.DefaultTimeout = 2000;
        }

        protected readonly String databaseName;
        protected readonly SQLiteConnection db;
        protected readonly String TableName;

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
            Debug.WriteLine("Database state is: " + db.State.ToString());
            Debug.WriteLine(sql);
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, db);
                db.Open();
                Debug.WriteLine("Database state is now: " + db.State.ToString());
                if (Async)
                {
                    command.ExecuteNonQueryAsync();
                }
                else
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("BUS");
                        throw;
                    }
                }
                db.Close();
                Debug.WriteLine("Database state is now: " + db.State.ToString());
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(db.DataSource.ToString());
                throw e;
            }
            finally
            {
                db.Close();
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