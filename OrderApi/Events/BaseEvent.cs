namespace OrderApi.Events
{
    public class BaseEvent
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
    }
}
