using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Repositories
{
    public interface IPersonRepository
    {
        Person GetByFullName(string firstName, string lastName);
        Person GetById(int id);
        List<Person> GetAll();
        Person Insert(Person person);
        void UpdateReportCount(int id, int numReports);
        void UpdateMentionCount(int id, int numMentions);
        void UpdateType(int id, string type);
    }
}
