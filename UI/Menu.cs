using System;
using Services;
using Models;
using System.Collections.Generic;
using Utils;

namespace Malshinon
{
    public class Menu
    {
        private readonly PersonService _personService;
        private readonly IntelService _intelService;
        private Person _currentUser;

        public Menu(PersonService personService, IntelService intelService)
        {
            _personService = personService;
            _intelService = intelService;
        }

        public void Start()
        {
            Console.WriteLine("===== Welcome to Malshinon =====");

            string choice = "";

            while (choice != "5")
            {
                ShowMenu();

                Console.Write("\nChoose an option (1-5): ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        IdentifyPerson();
                        break;
                    case "2":
                        SubmitIntel();
                        break;
                    case "3":
                        _intelService.PrintAllReports();
                        break;
                    case "4":
                        _personService.PrintAllPeople();
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1. Add New person or Login");
            Console.WriteLine("2. Submit new intel report");
            Console.WriteLine("3. Show all reports");
            Console.WriteLine("4. Show all people");
            Console.WriteLine("5. Exit");
        }


        private void IdentifyPerson()
        {
            string firstName = GetNonEmptyInput("Enter your first name: ");
            string lastName = GetNonEmptyInput("Enter your last name: ");

            var person = _personService.GetOrCreateReporter(firstName, lastName);

            bool codeMatched = false;

            while (!codeMatched)
            {
                Console.Write("Enter your secret code (or press Enter if you don't have one): ");
                string inputCode = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputCode))
                {
                    Console.WriteLine();
                    Console.WriteLine("Your new secret code is: " + person.SecretCode);
                    Console.WriteLine("This code has been saved to the system.");
                    codeMatched = true;
                }
                else if (inputCode == person.SecretCode)
                {
                    Console.WriteLine();
                    Console.WriteLine("Welcome back!");
                    codeMatched = true;
                }
                else
                {
                    Console.WriteLine("Incorrect code. Try again or press Enter to generate a new one.");
                }
            }

            _currentUser = person;

            Console.WriteLine();
            Console.WriteLine("Identification Complete:");
            Console.WriteLine("ID: " + person.Id);
            Console.WriteLine("Name: " + person.FirstName + " " + person.LastName);
            Console.WriteLine("Type: " + person.Type);
            Console.WriteLine("Reports: " + person.NumReports);
            Console.WriteLine("Mentions: " + person.NumMentions);
        }
        private string GetNonEmptyInput(string prompt)
        {
            string input = "";

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                }
            }

            return input;
        }



        private void SubmitIntel()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Please identify yourself first.");
                return;
            }

            Console.Write("\nEnter your intel report text: ");
            var reportText = Console.ReadLine();

            _intelService.SubmitIntel(_currentUser, reportText);
        }
    }
}
