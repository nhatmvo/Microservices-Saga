using OrderApi.Events;

namespace OrderApi.Services.QueueService
{
    public interface IClientQueueService
    {
        void Send<T>(T message) where T : BaseEvent;
    }
}
