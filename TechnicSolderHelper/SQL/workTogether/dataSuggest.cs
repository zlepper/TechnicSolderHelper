using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TechnicSolderHelper.SQL.workTogether
{
    class DataSuggest
    {
        private readonly String _connectionString;
        private const String Database = "solderhelper";

        public DataSuggest()
        {
            _connectionString = String.Format("address=localhost;username=dataSuggester;password=suggest;database={0}", Database);
        }

        public void Suggest(String filename, String mcversion, String modversion, String md5)
        {
            String sql = "INSERT INTO solderhelper.new(filename, mcversion, modversion, md5) VALUES(@filename, @mcversion, @modversion, @md5);";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@filename", filename);
                    command.Parameters.AddWithValue("@mcversion", mcversion);
                    command.Parameters.AddWithValue("@modversion", modversion);
                    command.Parameters.AddWithValue("@md5", md5);
                    command.ExecuteNonQueryAsync();
                }
            }
        }

        public bool IsModSuggested(String md5)
        {
            String sql = "SELECT md5 FROM solderhelper.new WHERE md5 LIKE @md5;";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@md5", md5);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["md5"].Equals(md5))
                                return true;
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}
