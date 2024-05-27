namespace PaymentApi.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public int OrderId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
