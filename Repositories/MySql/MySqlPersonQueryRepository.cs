using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Models;
using Repositories;

namespace Repositories.MySql
{
    public class MySqlPersonRepository : IPersonRepository
    {
        private readonly string _connectionString;

        public MySqlPersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Person GetByFullName(string firstName, string lastName)
        {
            const string query = "SELECT * FROM people WHERE first_name = @firstName AND last_name = @lastName";

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapReaderToPerson(reader);
                }
            }

            return null;
        }

        public Person GetById(int id)
        {
            const string query = "SELECT * FROM people WHERE id = @id";

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapReaderToPerson(reader);
                }
            }

            return null;
        }

        public List<Person> GetAll()
        {
            var result = new List<Person>();
            const string query = "SELECT * FROM people";

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(MapReaderToPerson(reader));
                    }
                }
            }

            return result;
        }

        public Person Insert(Person person)
        {
            const string query = @"INSERT INTO people 
                (first_name, last_name, secret_code, type, num_reports, num_mentions) 
                VALUES (@firstName, @lastName, @secretCode, @type, @numReports, @numMentions);
                SELECT LAST_INSERT_ID();";

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@firstName", person.FirstName);
                cmd.Parameters.AddWithValue("@lastName", person.LastName);
                cmd.Parameters.AddWithValue("@secretCode", person.SecretCode);
                cmd.Parameters.AddWithValue("@type", person.Type);
                cmd.Parameters.AddWithValue("@numReports", person.NumReports);
                cmd.Parameters.AddWithValue("@numMentions", person.NumMentions);

                conn.Open();
                person.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return person;
        }

        public void UpdateReportCount(int id, int numReports)
        {
            const string query = "UPDATE people SET num_reports = @numReports WHERE id = @id";

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@numReports", numReports);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateMentionCount(int id, int numMentions)
        {
            const string query = "UPDATE people SET num_mentions = @numMentions WHERE id = @id";

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@numMentions", numMentions);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateType(int id, string type)
        {
            const string query = "UPDATE people SET type = @type WHERE id = @id";

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private Person MapReaderToPerson(MySqlDataReader reader)
        {
            return new Person
            {
                Id = Convert.ToInt32(reader["id"]),
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                SecretCode = reader["secret_code"].ToString(),
                Type = reader["type"].ToString(),
                NumReports = Convert.ToInt32(reader["num_reports"]),
                NumMentions = Convert.ToInt32(reader["num_mentions"])
            };
        }
    }
}
