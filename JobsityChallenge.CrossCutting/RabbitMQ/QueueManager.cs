using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Microsoft.Extensions.Options;

namespace JobsityChallenge.CrossCutting.RabbitMQ
{
    public class QueueManager : IMessageQueue
    {
        private readonly ConnectionFactory configuration;

        public QueueManager(ConnectionFactory _configuration)
        {
            configuration = _configuration;

            using (var connection = configuration.CreateConnection())
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

        public void QueueMessage(string msg)
        {
            using (var connection = configuration.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "StockBot",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(msg);

                channel.BasicPublish(exchange: "",
                                     routingKey: "StockBot",
                                     basicProperties: null,
                                     body: body);
            }
        }


        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine(Environment.NewLine +
                "[Nova mensagem recebida] " + message);
        }
    }
}
