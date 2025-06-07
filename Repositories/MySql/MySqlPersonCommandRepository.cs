using System;
using Models;
using System.Configuration;
using global::MySql.Data.MySqlClient;
using global::Repositories;


namespace Repositories.MySql
    {
    public class MySqlPersonCommandRepository : IPersonCommandRepository
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public MySqlPersonCommandRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int Insert(Person person)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "INSERT INTO people (first_name, last_name, secret_code, type) VALUES (@first, @last, @code, @type); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@first", person.FirstName);
                    cmd.Parameters.AddWithValue("@last", person.LastName);
                    cmd.Parameters.AddWithValue("@code", person.SecretCode);
                    cmd.Parameters.AddWithValue("@type", person.Type);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void IncrementReports(int personId)
        {
            ExecuteUpdate("UPDATE people SET num_reports = num_reports + 1 WHERE id = @id", personId);
        }

        public void IncrementMentions(int personId)
        {
            ExecuteUpdate("UPDATE people SET num_mentions = num_mentions + 1 WHERE id = @id", personId);
        }

        public void UpdateType(int personId, string newType)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "UPDATE people SET type = @type WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@type", newType);
                    cmd.Parameters.AddWithValue("@id", personId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ExecuteUpdate(string query, int personId)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", personId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
