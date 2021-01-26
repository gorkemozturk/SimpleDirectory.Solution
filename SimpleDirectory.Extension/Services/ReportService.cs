using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using SimpleDirectory.Data;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Services
{
    public class ReportService : BaseService<Report>, IReportService
    {
        public ReportService(DirectoryDbContext context) : base(context)
        {
        }

        public async Task<Report> CreateReportAsync(string location)
        {
            // First, we need to fetch persons with their contacts that
            // equal to location credentials including enum type and value.
            var query = await _context.Persons
                .Include(p => p.Contacts)
                .Where(c => c.Contacts.Any(c => c.Type == Domain.Enums.ContactEnum.ContactTypes.Location && c.Body.Equals(location)))
                .ToListAsync();

            // As a second step, we have to filter the query collection
            // to reach total phone numbers using where statement and after count them.
            // The "numbers" result returns total phone numbers in related location.
            var numbers = query.Sum(c => c.Contacts.Where(c => c.Type == Domain.Enums.ContactEnum.ContactTypes.PhoneNumber).Count());

            // As a last step, we just need to count the query collection
            // to reach total people in related location.
            var people = query.Count();

            var report = new Report()
            {
                Location = location,
                People = people,
                Phones = numbers,
                CreatedAt = DateTime.Now,
                IsCompleted = true
            };

            _context.Reports.Add(report);

            return report;
        }

        public ReportQueueResultDTO CreateReportQueue(ReportCreateDTO report)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqps://bcybidpx:NGlFdHp4l8Dmq4nyWrfAkK7qp1PqoMLe@orangutan.rmq.cloudamqp.com/bcybidpx")
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "reportqueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(report.Location);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "reportqueue",
                    basicProperties: null,
                    body: body);
            }

            return new ReportQueueResultDTO 
            {
                Location = report.Location,
                IsCompleted = false
            };
        }
    }
}
