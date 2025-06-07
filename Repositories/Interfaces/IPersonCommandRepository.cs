using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Repositories
{
    public interface IPersonCommandRepository
    {
        int Insert(Person person);
        void IncrementReports(int personId);
        void IncrementMentions(int personId);
        void UpdateType(int personId, string newType);
    }
}
