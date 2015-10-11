using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using ModpackHelper.Shared.Web.Api;
using ModpackHelper.webmods.Helpers;

namespace ModpackHelper.webmods.db
{
    public class MySqlInitializer : IDatabaseInitializer<ModpackHelperContext>
    {
        public void InitializeDatabase(ModpackHelperContext context)
        {
            if (!context.Database.Exists())
            {
                // if database did not exist before - create it
                context.Database.Create();
            }
            else
            {
                // query to check if MigrationHistory table is present in the database 
                var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                  "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                  "modpackhelper"));

                // if MigrationHistory table is not there (which is the case first time we run) - create it
                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                }
            }
            // Make sure to create a default user that can be used to login to the DB
            if (!context.Users.Any())
            {
                User u = new User()
                {
                    Username = "admin",
                    Password = PasswordHash.CreateHash("admin")
                };
                context.Users.Add(u);
                context.SaveChanges();
            }
        }
    }
}
