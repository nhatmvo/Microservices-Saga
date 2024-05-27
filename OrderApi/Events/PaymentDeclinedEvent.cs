namespace OrderApi.Events
{
    public class PaymentDeclinedEvent : PaymentBaseMessage
    {
        public int OrderId { get; set; }

        public string Reason { get; set; }
    }
}
