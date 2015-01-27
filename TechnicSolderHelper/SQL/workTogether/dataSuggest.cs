using System;
using MySql.Data.MySqlClient;

namespace TechnicSolderHelper.SQL.workTogether
{
    class DataSuggest
    {
        private readonly String _connectionStringSuggest;
        private readonly String _connectionStringGet;
        private const String SuggestDatabase = "solderhelper";
        private const String GetDatabase = "helpersolder";

        public DataSuggest()
        {
            _connectionStringSuggest = String.Format("address=zlepper.dk;username=dataSuggester;password=suggest;database={0}", SuggestDatabase);
            _connectionStringGet = String.Format("address=zlepper.dk;username=dataSuggester;password=suggest;database={0}", GetDatabase); 
        }

        public void Suggest(String filename, String mcversion, String modversion, String md5, String modid, String modname)
        {
            if (!IsModSuggested(md5))
            {
                const string sql = "INSERT INTO solderhelper.new(filename, mcversion, modversion, md5, modid, modname) VALUES(@filename, @mcversion, @modversion, @md5, @modid, @modname);";
                using (MySqlConnection connection = new MySqlConnection(_connectionStringSuggest))
                {
                    connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@filename", filename);
                        command.Parameters.AddWithValue("@mcversion", mcversion);
                        command.Parameters.AddWithValue("@modversion", modversion);
                        command.Parameters.AddWithValue("@md5", md5);
                        command.Parameters.AddWithValue("@modid", modid);
                        command.Parameters.AddWithValue("@modname", modname);
                        command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public bool IsModSuggested(String md5)
        {
            String sql = "SELECT md5 FROM solderhelper.new WHERE md5 LIKE @md5;";
            using (MySqlConnection connection = new MySqlConnection(_connectionStringSuggest))
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

        public Mcmod GetMcmod(String md5)
        {
            String sql = "SELECT modname, modid, mcversion, modversion, md5 FROM helpersolder.mods WHERE md5 LIKE @md5;";
            using (MySqlConnection connection = new MySqlConnection(_connectionStringGet))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@md5", md5);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["md5"].Equals(md5))
                            {
                                Mcmod mod = new Mcmod
                                {
                                    Version = reader["modversion"].ToString(),
                                    Name = reader["modname"].ToString(),
                                    Modid = reader["modid"].ToString(),
                                    Mcversion = reader["mcversion"].ToString()
                                };
                                return mod;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
