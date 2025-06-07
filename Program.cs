using System;
using System.Collections.Generic;
using System.Configuration;
using Repositories.MySql;
using Services;

namespace Malshinon
{
    class Program
    {
        static void Main()
        {
            // Load connection string from App.config
            string connectionString = ConfigurationManager
                .ConnectionStrings["MySqlConnection"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("ERROR: Connection string 'MySqlConnection' not found in App.config");
                return;
            }

            // Initialize repositories
            var personQueryRepo = new MySqlPersonRepository(connectionString);
            var personCommandRepo = new MySqlPersonCommandRepository(connectionString);
            var intelRepo = new MySqlIntelReportRepository(connectionString);

            // Initialize services
            var personService = new PersonService(personQueryRepo, personCommandRepo);
            var intelService = new IntelService(intelRepo, personCommandRepo, personQueryRepo);

            // Run the menu
            var menu = new Menu(personService, intelService);
            menu.Start();
        }
    }
}
