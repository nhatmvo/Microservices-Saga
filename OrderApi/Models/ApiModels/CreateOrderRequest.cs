using MediatR;

namespace OrderApi.Models.ApiModels
{
    public class CreateOrderRequest
    {
        public List<OrderLine> OrderLines { get; set; }

    }
}
