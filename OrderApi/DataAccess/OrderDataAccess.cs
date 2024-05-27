using Microsoft.EntityFrameworkCore;
using OrderApi.Models;
using OrderApi.Models.ApiModels;

namespace OrderApi.DataAccess
{
    public class OrderDataAccess : IOrderDataAccess
    {
        private readonly OrderDbContext _dbContext;

        public OrderDataAccess(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CancelOrder(int orderId)
        {
            var orderToCancel = _dbContext.Set<Order>().FirstOrDefault(x => x.OrderId == orderId);
            if (orderToCancel != null)
            {
                orderToCancel.OrderStateId = (int)OrderStateEnum.Rejected;
                _dbContext.Set<Order>().Update(orderToCancel);
                _dbContext.SaveChanges();
                return;
            }
            throw new Exception($"Order not found with id: {orderId}");
        }

        public void CompleteOrder(int orderId)
        {
            var orderToComplete = _dbContext.Set<Order>().FirstOrDefault(x => x.OrderId == orderId);
            if (orderToComplete != null)
            {
                orderToComplete.OrderStateId = (int)OrderStateEnum.Approved;
                _dbContext.Set<Order>().Update(orderToComplete);
                _dbContext.SaveChanges();
                return;
            }
            throw new Exception($"Order not found with id: {orderId}");
        }

        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            var orderToAdd = request.OrderLines.Select(x => new Order
            {
                OrderStateId = (int)OrderStateEnum.Pending,
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).FirstOrDefault();

            if (orderToAdd != null)
            {
                var product = _dbContext.Set<Product>().AsNoTracking().FirstOrDefault(x => x.ProductId == orderToAdd.ProductId);

                _dbContext.Set<Order>().Add(orderToAdd);
                _dbContext.SaveChanges();
                return new CreateOrderResponse
                {
                    OrderId = orderToAdd.OrderId,
                    TotalPrice = product.Price * orderToAdd.Quantity,
                    Message = "Order created in pending state"
                };
            }
            throw new Exception("Cannot add order");
        }
    }
}
