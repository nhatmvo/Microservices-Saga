using Microsoft.AspNetCore.Mvc;
using OrderApi.Models.ApiModels;
using OrderApi.Services;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Route("CreateOrder")]
        [HttpPost]
        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            return _orderService.CreateOrder(request);
        }
    }
}