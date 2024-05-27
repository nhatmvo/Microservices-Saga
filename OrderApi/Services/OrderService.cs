using OrderApi.DataAccess;
using OrderApi.Events;
using OrderApi.Models.ApiModels;
using OrderApi.Services.QueueService;

namespace OrderApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDataAccess _orderDataAccess;
        private readonly IClientQueueService _clientQueueService;

        public OrderService(IOrderDataAccess orderDataAccess, IClientQueueService clientQueueService) 
        {
            _orderDataAccess = orderDataAccess;
            _clientQueueService = clientQueueService;
        }

        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            var response = _orderDataAccess.CreateOrder(request);
            _clientQueueService.Send(new OrderCreatedEvent
            {
                OrderId = response.OrderId,
                TotalPrice = response.TotalPrice
            });
            return response;
        }

        public void CompletedOrder(int orderId)
        {
            _orderDataAccess.CompleteOrder(orderId);
        }

        public void CancelledOrder(int orderId)
        {
            _orderDataAccess.CancelOrder(orderId);
        }
    }
}
