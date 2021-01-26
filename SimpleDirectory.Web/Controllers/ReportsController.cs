using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;

namespace SimpleDirectory.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _report;

        public ReportsController(IReportService report)
        {
            _report = report;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IEnumerable<Report>> GetReports()
        {
            return await _report.GetResourcesAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Report>> GetReport([FromRoute] Guid id)
        {
            var report = await _report.GetResourceAsync(id);

            if (report == null)
            {
                var error = new CustomError
                {
                    Detail = new KeyNotFoundException().Message
                };

                return NotFound(error);
            }

            return report;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ReportQueueResultDTO> CreateReportQueue(ReportCreateDTO report)
        {
            return _report.CreateReportQueue(report);
        }
    }
}