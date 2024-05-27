using PaymentApi.Models;
using PaymentApi.Models.ApiModels;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using PaymentApi.Events;

namespace PaymentApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentDbContext _dbContext;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(PaymentDbContext dbContext, ILogger<PaymentService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void FulfillOrderPayment(int budgetId, decimal price, int orderId)
        {
            var budget = _dbContext.Set<Budget>().FirstOrDefault(x => x.BudgetId == budgetId);
            if (budget == null) throw new Exception("Budget not found");
            
            if (budget.Balance >= price)
            {
                budget.Balance -= price;
                _dbContext.Set<Budget>().Update(budget);
                _dbContext.SaveChanges();

                Send(new PaymentCompletedEvent
                {
                    OrderId = orderId,
                    PaymentState = (int)PaymentStates.Accepted
                });
            } else
            {
                Send(new PaymentCancelledEvent
                {
                    OrderId = orderId,
                    Reason = "Payment was not fulfilled due to sufficient balance!",
                    PaymentState = (int)PaymentStates.Cancelled
                });
            }
            
        }

        public TopUpBalanceResponse TopUpBalance(TopUpBalanceRequest request)
        {
            var budget = _dbContext.Set<Budget>().FirstOrDefault(x => x.BudgetId == request.BudgetId);

            if (budget == null)
            {
                return new TopUpBalanceResponse { IsSuccess = false };
            }
            budget.Balance += request.Balance;
            _dbContext.Set<Budget>().Update(budget);
            _dbContext.SaveChanges();

            return new TopUpBalanceResponse { IsSuccess = true };
        }

        private void Send<T>(T message) where T: BaseEvent
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "payment-order",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = JsonSerializer.Serialize(message);

            _logger.LogInformation($"Sending message to queue. MessageId: {message.MessageId}");
            channel.BasicPublish(exchange: string.Empty, routingKey: "payment-order", null, body: Encoding.UTF8.GetBytes(body));
        }
    }
}
