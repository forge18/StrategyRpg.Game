namespace Infrastructure.MediatorNS.EventManagement
{
    public interface IEventListener
    {
        void OnEvent(EventTypeEnum eventType, IEvent eventData);
    }
}