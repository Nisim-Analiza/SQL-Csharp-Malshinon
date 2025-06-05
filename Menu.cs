using System;
using Data;
// Add Utils namespace
using Utils;

namespace Malshinon
{
    internal class Menu
    {
        private readonly UseSQL _db = UseSQL.Instance;

        private const string PeopleTable = "peoples";
        private const string IntelTable = "intelreports";

        public void Start()
        {
            try
            {
                _db.LoadConnectionString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load DB settings: " + ex.Message);
                return;
            }
            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        DisplayTable(PeopleTable);
                        break;

                    case "2":
                        DisplayTable(IntelTable);
                        break;

                    case "5":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
        }

   

        private void ShowMenu()
        {
            Console.WriteLine(
                "\nEnter your choice:\n" +
                "1 - See all people\n" +
                "2 - See all intel reports\n" +
                "5 - Exit\n"
            );
        }

        private void DisplayTable(string tableName)
        {
            var rows = _db.RunQuery("SELECT * FROM " + tableName);
            RowPrinter.Print(rows);
        }
    }
}   
