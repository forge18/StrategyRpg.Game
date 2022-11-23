using System;
using System.Collections.Generic;

namespace Infrastructure.EventManagement
{
    public abstract class Event<IContract> : IEvent<IContract>
    {
        Dictionary<ISubscriber, Action<IContract>> _subscribers = new Dictionary<ISubscriber, Action<IContract>>();

        public void Subscribe(ISubscriber subscriber, Action<IContract> callback)
        {
            _subscribers.Add(subscriber, callback);
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        public int SubscriberCount()
        {
            return _subscribers.Count;
        }

        public void Broadcast(IContract args)
        {
            foreach (var (subscriber, callback) in _subscribers)
            {
                callback(args);
            }
        }
    }
}