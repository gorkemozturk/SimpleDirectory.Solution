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

namespace SimpleDirectory.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Report> CreateReport(Report report)
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

                string message = JsonSerializer.Serialize(report);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "reportqueue",
                    basicProperties: null,
                    body: body);
            }

            return report;
        }
    }
}