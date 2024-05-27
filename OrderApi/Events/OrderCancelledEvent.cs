namespace OrderApi.Events
{
    public class OrderCancelledEvent : BaseEvent
    {
        public int OrderId { get; set; }
    }
}
