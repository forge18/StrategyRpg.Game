using System;
using Godot;
using Godot.Collections;
using Infrastructure.EventManagement.Events.InputUpdated;

namespace Infrastructure.EventManagement
{
    public class EventService : IEventService
    {
        private Dictionary<EventTypeEnum, IEvent<EventManagement.IContract>> _events =
            new Dictionary<EventTypeEnum, IEvent<EventManagement.IContract>>();

        private readonly Dictionary<EventTypeEnum, ValueTuple<Type, Type>> _eventTypes = new Dictionary<EventTypeEnum, ValueTuple<Type, Type>>
        {
            { EventTypeEnum.InputUpdated, (typeof(InputUpdatedEvent), typeof(InputUpdatedContract)) }
        };

        public void Subscribe<IContract>(
            EventTypeEnum eventType,
            ISubscriber subscriber, Action<EventManagement.IContract> callback
        )
        {
            if (!_events.ContainsKey(eventType))
            {
                CreateEventInstance(eventType);
            }

            _events[eventType].Subscribe(subscriber, callback);
        }

        public void Unsubscribe<IContract>(EventTypeEnum eventType, ISubscriber subscriber)
        {
            var gameEvent = _events[eventType];
            gameEvent.Unsubscribe(subscriber);

            if (gameEvent.SubscriberCount() == 0)
            {
                _events.Remove(eventType);
            }
        }

        public void Publish<IContract>(IEvent<IContract> gameEvent, IContract args)
        {
            gameEvent.Broadcast(args);
        }

        public IEvent<IContract> CreateEventInstance(EventTypeEnum eventType)
        {
            if (EventIsInstantiated(eventType))
            {
                GD.PrintErr("Event already instantiated");
                return default;
            }
            var eventClass = _eventTypes[eventType].Item1;
            var contractClass = _eventTypes[eventType].Item2;

            var genericType = eventClass.MakeGenericType(contractClass);
            var eventInstance = (IEvent<EventManagement.IContract>)Activator.CreateInstance(genericType);
            _events.Add(eventType, (IEvent<EventManagement.IContract>)eventInstance);

            return eventInstance;
        }

        public bool EventIsInstantiated(EventTypeEnum eventType)
        {
            return _events.ContainsKey(eventType);
        }
    }
}