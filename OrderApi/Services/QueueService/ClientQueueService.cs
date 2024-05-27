using OrderApi.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderApi.Services.QueueService
{
    public class ClientQueueService : IClientQueueService
    {
        private readonly ILogger<ClientQueueService> _logger;

        public ClientQueueService(ILogger<ClientQueueService> logger) => _logger = logger;

        public void Send<T>(T message) where T : BaseEvent
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "order-payment",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = JsonSerializer.Serialize(message);

            _logger.LogInformation($"Sending message to queue. MessageId: {message.MessageId}");
            channel.BasicPublish(exchange: string.Empty, routingKey: "order-payment", null, body: Encoding.UTF8.GetBytes(body));
        }
    }
}
