using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleDirectory.Data;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;
using SimpleDirectory.Extension.Services;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleDirectory.Consumer
{
    public class Program
    {
        private readonly IReportService _report;

        public Program(IReportService report)
        {
            _report = report;
        }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }

        public void Run()
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
                    var report = _report.CreateReportAsync(message).GetAwaiter().GetResult();

                    if (_report.SaveChangesAsync().GetAwaiter().GetResult() > 0)
                    {
                        Console.WriteLine($"[{report.Id}] Report received at {report.CreatedAt}: {report.Location}");
                    }
                };

                channel.BasicConsume(queue: "reportqueue", autoAck: true, consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => 
                {
                    services.AddScoped<Program>();
                    services.AddEntityFrameworkNpgsql().AddDbContext<DirectoryDbContext>(options =>
                    {
                        options.UseNpgsql(
                            "User ID = postgres; " +
                            "Password = 0000; " +
                            "Server = localhost; " +
                            "Port = 5432; " +
                            "Database = SimpleDirectory; " +
                            "Integrated Security = true; " +
                            "Pooling = true;");
                    });
                    services.AddScoped<IReportService, ReportService>();
                    services.AddLogging(options =>
                    {
                        options.AddFilter("Microsoft", LogLevel.Warning).AddConsole();
                    });
                });
        }
    }
}
