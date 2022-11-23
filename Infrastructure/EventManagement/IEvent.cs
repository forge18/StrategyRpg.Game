
using System;

namespace Infrastructure.EventManagement
{
    public interface IEvent<IContract>
    {
        void Subscribe(ISubscriber subscriber, Action<IContract> callback);
        void Unsubscribe(ISubscriber subscriber);
        int SubscriberCount();
        void Broadcast(IContract args);
    }
}