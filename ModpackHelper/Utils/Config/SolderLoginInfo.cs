using System;
using ModpackHelper.Shared.Cryptography;

namespace ModpackHelper.Shared.Utils.Config
{
    /// <summary>
    /// Contains info about how to login to the solder DB
    /// </summary>
    public class SolderLoginInfo
    {
        /// <summary>
        /// The username to connect to the DB
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to login with
        /// </summary>
        public string Password {
            get {
                Crypto crypto = new Crypto();
                return crypto.DecryptString(pw);
            }
            set
            {
                Crypto crypto = new Crypto();
                pw = crypto.EncryptToString(value);
            }
        }

        /// <summary>
        /// The encrypted password
        /// </summary>
        private string pw { get; set; }

        /// <summary>
        /// The address of database
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The name of the scema to connect to
        /// Also known as the database name
        /// </summary>
        public string DatabaseSchema { get; set; }

        /// <summary>
        /// The prefix of the tables in solder
        /// </summary>
        public string TablePrefix { get; set; }

        /// <summary>
        /// Get the connectionstring that connects to the Database
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            Crypto crypto = new Crypto();
            return
                $"address={Address};username={Username};password={Password};database={DatabaseSchema}";
        }

        /// <summary>
        /// Checks if the info in here has any chance of being valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Address) &&
                   !string.IsNullOrWhiteSpace(DatabaseSchema);
        }
    }
}