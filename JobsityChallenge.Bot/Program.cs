using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Jobsity.Challenge.Bot
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            SetupClient();

            while (true)
            {
                Console.WriteLine("Waiting for messages");
                await Task.Delay(1000);
            }
        }

        private static void SetupClient()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "JobsityUser",
                Password = "JobPass#456"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "StockBot",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queue: "StockBot",
                     autoAck: true,
                     consumer: consumer);
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine(Environment.NewLine + "[Nova mensagem recebida] " + message);
        }
    }
}
