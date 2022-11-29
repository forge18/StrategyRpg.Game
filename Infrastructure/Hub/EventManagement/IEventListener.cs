namespace Infrastructure.HubMediator
{
    public interface IEventListener
    {
        void OnEvent(EventTypeEnum eventType, IEvent eventData);
    }
}