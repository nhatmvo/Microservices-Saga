using OrderApi.Models;
using OrderApi.Models.ApiModels;

namespace OrderApi.Services
{
    public interface IOrderService
    {
        CreateOrderResponse CreateOrder(CreateOrderRequest request);

        void CompletedOrder(int orderId);

        void CancelledOrder(int orderId);
    }
}
