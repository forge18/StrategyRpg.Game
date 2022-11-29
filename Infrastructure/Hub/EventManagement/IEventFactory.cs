namespace Infrastructure.HubMediator
{
    public interface IEventFactory
    {
        dynamic CreateInstance(EventTypeEnum eventType);
    }
}