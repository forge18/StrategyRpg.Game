using System;

namespace Infrastructure.EventManagement
{
    public interface IEventService
    {
        void Subscribe<IContract>(EventTypeEnum eventType, ISubscriber subscriber, Action<EventManagement.IContract> callback);
        void Unsubscribe<IContract>(EventTypeEnum eventType, ISubscriber subscriber);
        void Publish<IContract>(IEvent<IContract> gameEvent, IContract args);
    }
}