using System;
using System.Collections.Generic;
using Models;
using Repositories;
using Utils;

namespace Services
{
    public class IntelService
    {
        private readonly IIntelReportRepository _reportRepository;
        private readonly IPersonRepository _personRepository;
 

        public IntelService(
            IIntelReportRepository reportRepository,
            Repositories.MySql.MySqlPersonCommandRepository personCommandRepo,
            IPersonRepository personRepository
            )
        {
            _reportRepository = reportRepository;
            _personRepository = personRepository;
          
        }

        public void SubmitIntel(Person reporter, string text)
        {
            if (reporter == null || string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Invalid reporter or report text.");
                return;
            }

            var target = ExtractTarget(text);
            if (target == null)
            {
                Console.WriteLine("Could not identify a target from the text.");
                return;
            }

            var report = new IntelReport
            {
                ReporterId = reporter.Id,
                TargetId = target.Id,
                Text = text,
                Timestamp = DateTime.Now
            };

            _reportRepository.Insert(report);
            Console.WriteLine("Intel submitted successfully.");

            UpdateStats(reporter, target, text.Length);
            Console.WriteLine($"Report submitted by {reporter.FirstName} {reporter.LastName} targeting {target.FirstName} {target.LastName}.");
        }

        //public void PrintAllReports()
        //{
        //    var reports = _reportRepository.GetAll();

        //    if (reports.Count == 0)
        //    {
        //        Console.WriteLine("No reports found.");
        //        return;
        //    }

        //    foreach (var report in reports)
        //    {
        //        Console.WriteLine($"\nReport ID: {report.Id}");
        //        Console.WriteLine($"Reporter ID: {report.ReporterId}, Target ID: {report.TargetId}");
        //        Console.WriteLine($"Text: {report.Text}");
        //        Console.WriteLine($"Timestamp: {report.Timestamp}");
        //        Console.WriteLine(new string('-', 30));
        //    }
        //}
        public List<IntelReport> GetAllReports()
        {
            return _reportRepository.GetAll();
        }


        private void UpdateStats(Person reporter, Person target, int textLength)
        {
            reporter.NumReports++;
            _personRepository.UpdateReportCount(reporter.Id, reporter.NumReports);

            target.NumMentions++;
            _personRepository.UpdateMentionCount(target.Id, target.NumMentions);

            if (reporter.NumReports >= 10 && AverageTextLength(reporter.Id) >= 100)
            {
                _personRepository.UpdateType(reporter.Id, "potential_agent");
                Console.WriteLine($"Reporter {reporter.FirstName} flagged as potential agent.");
            }

            if (target.NumMentions >= 20)
            {
                Console.WriteLine($"Target {target.FirstName} flagged as potential threat.");
            }
        }

        private int AverageTextLength(int reporterId)
        {
            var reports = _reportRepository.GetByReporterId(reporterId);
            if (reports.Count == 0) return 0;

            int totalLength = 0;
            foreach (var report in reports)
                totalLength += report.Text.Length;

            return totalLength / reports.Count;
        }

        private Person ExtractTarget(string text)
        {
            var words = text.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length - 1; i++)
            {
                var firstName = Capitalize(words[i]);
                var lastName = Capitalize(words[i + 1]);

                // חיפוש במסד הנתונים
                var person = _personRepository.GetByFullName(firstName, lastName);
                if (person != null)
                {
                    Console.WriteLine($"Found existing target: {firstName} {lastName}");
                    return person;
                }

                // אם לא נמצא — צור יעד חדש
                Console.WriteLine($"Target not found. Creating new target: {firstName} {lastName}");
                var newTarget = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    SecretCode = Guid.NewGuid().ToString("N"),
                    Type = "target",
                    NumReports = 0,
                    NumMentions = 0
                };

                return _personRepository.Insert(newTarget);
            }

            Console.WriteLine("No valid target name found in the report text.");
            return null;
        }

        private string Capitalize(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return word;
            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        }


        public void PrintAllReports()
        {
            var reports = GetAllReports(); // פונקציה שמחזירה List<IntelReport>

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

            foreach (var report in reports)
            {
                var row = new Dictionary<string, object>
            {
                {"Report ID", report.Id},
                {"Reporter ID", report.ReporterId},
                {"Target ID", report.TargetId},
                {"Text", report.Text},
                {"Time", report.Timestamp}
            };
                data.Add(row);
            }

            RowPrinter.Print(data);
        }
    }
}
