using Models;
using Repositories;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace Repositories.MySql
{
    public class MySqlIntelReportRepository : IIntelReportRepository
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public MySqlIntelReportRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int Insert(IntelReport report)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "INSERT INTO intelreports (reporter_id, target_id, text, timestamp) VALUES (@reporter, @target, @text, @timestamp); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@reporter", report.ReporterId);
                    cmd.Parameters.AddWithValue("@target", report.TargetId);
                    cmd.Parameters.AddWithValue("@text", report.Text);
                    cmd.Parameters.AddWithValue("@timestamp", report.Timestamp);
                    cmd.ExecuteNonQuery();
                    return (int)cmd.LastInsertedId;
                }
            }
        }

        public List<IntelReport> GetAll()
        {
            var list = new List<IntelReport>();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM intelreports";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(ReadIntel(reader));
                    }
                }
            }
            return list;
        }

        public List<IntelReport> GetByReporterId(int reporterId)
        {
            return GetByPerson("reporter_id", reporterId);
        }

        public List<IntelReport> GetByTargetId(int targetId)
        {
            return GetByPerson("target_id", targetId);
        }

        public List<IntelReport> GetByKeyword(string keyword)
        {
            var list = new List<IntelReport>();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM intelreports WHERE text LIKE @keyword";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(ReadIntel(reader));
                        }
                    }
                }
            }
            return list;
        }

        private List<IntelReport> GetByPerson(string field, int id)
        {
            var list = new List<IntelReport>();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM intelreports WHERE {field} = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(ReadIntel(reader));
                        }
                    }
                }
            }
            return list;
        }

        private IntelReport ReadIntel(MySqlDataReader reader)
        {
            return new IntelReport
            {
                Id = reader.GetInt32("id"),
                ReporterId = reader.GetInt32("reporter_id"),
                TargetId = reader.GetInt32("target_id"),
                Text = reader.GetString("text"),
                Timestamp = reader.GetDateTime("timestamp")
            };
        }
    }
}

