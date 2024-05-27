namespace OrderApi.Events
{
    public class PaymentCompletedEvent : PaymentBaseMessage
    {
        public int OrderId { get; set; }
    }
}
