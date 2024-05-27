using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PaymentApi.Services;
using Newtonsoft.Json;
using PaymentApi.Events;

namespace PaymentApi.Consumers
{
    public class OrderQueueConsumer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderQueueConsumer(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "order-payment", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var orderToHandle = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                    paymentService.FulfillOrderPayment(1, orderToHandle.TotalPrice, orderToHandle.OrderId);
                }
            };

            channel.BasicConsume(queue: "order-payment",
                     autoAck: true,
                     consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
