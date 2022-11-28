namespace Infrastructure.Hub.EventManagement
{
    public interface IEventListener
    {
        void OnEvent(EventTypeEnum eventType, IEvent eventData);
    }
}