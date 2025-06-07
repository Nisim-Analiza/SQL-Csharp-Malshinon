using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using Models;
using Utils;
namespace Services
{
    public class PersonService
    {
        private readonly IPersonRepository _queryRepo;
        private readonly IPersonCommandRepository _commandRepo;

        public PersonService(IPersonRepository queryRepo, IPersonCommandRepository commandRepo)
        {
            _queryRepo = queryRepo;
            _commandRepo = commandRepo;
        }

        public Person GetOrCreateReporter(string firstName, string lastName)
        {
            var person = _queryRepo.GetByFullName(firstName, lastName);
            if (person != null)
                return person;

            var newPerson = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                SecretCode = Guid.NewGuid().ToString().Substring(0, 8),
                Type = "reporter"
            };

            newPerson.Id = _commandRepo.Insert(newPerson);
            return newPerson;
        }

        public Person GetOrCreateTarget(string firstName, string lastName)
        {
            var person = _queryRepo.GetByFullName(firstName, lastName);
            if (person != null)
                return person;

            var newPerson = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                SecretCode = Guid.NewGuid().ToString().Substring(0, 8),
                Type = "target"
            };

            newPerson.Id = _commandRepo.Insert(newPerson);
            return newPerson;
        }

        public List<Person> GetAllPeople() => _queryRepo.GetAll();

        public void PrintAllPeople()
        {
            List<Models.Person> people = GetAllPeople();

            // ממירים את הרשימה למילונים שניתן להדפיס בטבלה
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

            foreach (var person in people)
            {
                var row = new Dictionary<string, object>
            {
                {"ID", person.Id},
                {"First Name", person.FirstName},
                {"Last Name", person.LastName},
                {"Secret Code", person.SecretCode},
                {"Type", person.Type},
                {"Reports", person.NumReports},
                {"Mentions", person.NumMentions}
            };
                data.Add(row);
            }

            RowPrinter.Print(data);
        }
    }

}

