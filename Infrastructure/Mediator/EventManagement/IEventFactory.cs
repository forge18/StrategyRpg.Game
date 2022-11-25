namespace Infrastructure.MediatorNS.EventManagement
{
    public interface IEventFactory
    {
        IEventHandler CreateInstance(EventTypeEnum eventType);
    }
}