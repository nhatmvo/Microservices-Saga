using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int OrderStateId { get; set; }

        public int Quantity { get; set; }
    }
}
