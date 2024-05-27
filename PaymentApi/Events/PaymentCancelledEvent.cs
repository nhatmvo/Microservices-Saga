namespace PaymentApi.Events
{
    public class PaymentCancelledEvent : BaseEvent
    {
        public int OrderId { get; set; }

        public string Reason { get; set; }

        public int PaymentState { get; set; }
    }
}
