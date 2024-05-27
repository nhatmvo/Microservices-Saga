namespace OrderApi.Models.ApiModels
{
    public class CreateOrderResponse
    {
        public int OrderId { get; set; }

        public string Message { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
