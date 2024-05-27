using OrderApi.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderApi.Services.Consumers
{
    public class PaymentConsumer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentConsumer(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "payment-order", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    var baseMessage = JsonSerializer.Deserialize<PaymentBaseMessage>(message);
                    if (baseMessage != null && baseMessage.PaymentState == 1)
                    {
                        var orderCompletedMsg = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
                        orderService.CompletedOrder(orderCompletedMsg.OrderId);
                    } else
                    {
                        var orderCanceleedMsg = JsonSerializer.Deserialize<OrderCancelledEvent>(message);
                        orderService.CancelledOrder(orderCanceleedMsg.OrderId);
                    }
                }
            };

            channel.BasicConsume(queue: "payment-order",
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
