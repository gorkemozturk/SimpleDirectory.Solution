using SimpleDirectory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Interfaces
{
    public interface IReportService : IBaseService<Report>
    {
        Task<Report> CreateReportAsync(string location);
        ReportQueueResultDTO CreateReportQueue(ReportCreateDTO report);
    }
}
