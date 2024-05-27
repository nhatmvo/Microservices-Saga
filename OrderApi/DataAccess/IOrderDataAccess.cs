using OrderApi.Models.ApiModels;

namespace OrderApi.DataAccess
{
    public interface IOrderDataAccess
    {
        CreateOrderResponse CreateOrder(CreateOrderRequest request);

        void CompleteOrder(int orderId);

        void CancelOrder(int orderId);
    }
}
