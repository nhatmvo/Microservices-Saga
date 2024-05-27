namespace PaymentApi.Events
{
    public class BaseEvent
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
    }
}
