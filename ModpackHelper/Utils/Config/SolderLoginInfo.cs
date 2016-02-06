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
                return crypto.DecryptString(Pw);
            }
            set
            {
                Crypto crypto = new Crypto();
                Pw = crypto.EncryptToString(value);
            }
        }

        /// <summary>
        /// The encrypted password
        /// </summary>
        private string Pw { get; set; }

        /// <summary>
        /// The address of database
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Checks if the info in here has any chance of being valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Address) &&
                   !string.IsNullOrWhiteSpace(Password);
        }
    }
}