using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleDirectory.Domain.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleDirectory.Consumer
{
    class Program
    {
        static void Main(string[] args)
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

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var report = JsonSerializer.Deserialize<Report>(message);

                    Console.WriteLine($"[+] Report Received: {report.Location}");
                };

                channel.BasicConsume(queue: "reportqueue", autoAck: true, consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
