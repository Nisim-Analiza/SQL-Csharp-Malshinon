using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Repositories
{
    public interface IIntelReportRepository
    {
        int Insert(IntelReport report);
        List<IntelReport> GetAll();
        List<IntelReport> GetByReporterId(int reporterId);
        List<IntelReport> GetByTargetId(int targetId);
    }
}
