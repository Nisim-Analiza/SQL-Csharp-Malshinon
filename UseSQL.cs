using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Data
{
    public class UseSQL
    {
        private string _connectionString;

        private static UseSQL _instance;
        public static UseSQL Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UseSQL();
                return _instance;
            }
        }

        private UseSQL() { }

        public void LoadConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["MySqlConnection"];
            if (cs == null)
                throw new Exception("Connection string 'MySqlConnection' not found in App.config");

            _connectionString = cs.ConnectionString;
        }

        public List<Dictionary<string, object>> RunQuery(string query)
        {
            var results = new List<Dictionary<string, object>>();

            if (string.IsNullOrEmpty(_connectionString))
            {
                Console.WriteLine("Connection string not loaded.");
                return results;
            }

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                row[reader.GetName(i)] = reader.GetValue(i);
                            results.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }

            return results;
        }
    }
}
