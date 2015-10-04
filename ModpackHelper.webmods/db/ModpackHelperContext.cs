using System.Data.Entity;
using ModpackHelper.Shared.Web.Api;

namespace ModpackHelper.webmods.db
{
    /// <summary>
    /// The main database connection making storing all the data
    /// </summary>
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ModpackHelperContext : DbContext
    {
        /// <summary>
        /// All the mods currently stored int he system
        /// </summary>
        public DbSet<Mod> Mods { get; set; }
        /// <summary>
        /// All the helper users in the system
        /// </summary>
        public DbSet<HelperUser> HelperUsers { get; set; }
        /// <summary>
        /// All the authors in the system
        /// </summary>
        public DbSet<Author> Authors { get; set; }
        /// <summary>
        /// All the users of the webapplication
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// All the connections to the server
        /// </summary>
        public DbSet<Connection> Connections { get; set; }

        /// <summary>
        /// Create a new database connection
        /// </summary>
        public ModpackHelperContext() : base()
        {
            Database.SetInitializer(new MySqlInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Define the primary key of the mods in the database
            modelBuilder.Entity<Mod>().HasKey(m => m.Id);

            // Map a many to many relationship between mods and authors
            modelBuilder.Entity<Mod>()
                .HasMany<Author>(m => m.Authors)
                .WithMany(a => a.Mods)
                .Map(ma =>
                {
                    ma.MapLeftKey("AuthorRefId");
                    ma.MapRightKey("ModRefId");
                    ma.ToTable("ModsByAuthors");
                });

            // Map a one to many relationship between HelperUsers and mods
            modelBuilder.Entity<HelperUser>()
                .HasMany<Mod>(u => u.Mods)
                .WithRequired(m => m.HelperUser)
                .HasForeignKey(m => m.HelperUserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany<Mod>(u => u.AcceptedMods)
                .WithOptional(m => m.AcceptedBy)
                .HasForeignKey(m => m.AcceptedById)
                .WillCascadeOnDelete(false);

            // Define the primary key of the HelperUser in the database
            modelBuilder.Entity<HelperUser>().HasKey(h => h.Id);

            // Define the primary key of the Users in the database
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            // Define the primary key of the Authors in the database
            modelBuilder.Entity<Author>().HasKey(a => a.Id);

            // Map relation between "User.Connections" and "Connection.User"
            modelBuilder.Entity<User>()
                .HasMany<Connection>(u => u.Connections)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}