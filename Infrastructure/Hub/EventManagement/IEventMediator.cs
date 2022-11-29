namespace Infrastructure.HubMediator
{
    public interface IEventMediator
    {
        void Subscribe(EventTypeEnum eventType, IEventListener eventListener);
        void Unsubscribe(EventTypeEnum eventType, IEventListener eventListener);
        bool HasEventInstance(EventTypeEnum eventType);
        void Notify(EventTypeEnum eventType, IEvent eventData);
    }
}