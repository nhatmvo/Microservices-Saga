namespace PaymentApi.Events
{
    public class PaymentCompletedEvent : BaseEvent
    {
        public int OrderId { get; set; }

        public int PaymentState { get; set; }
    }
}
