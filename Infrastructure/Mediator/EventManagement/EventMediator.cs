using System.Collections.Generic;

namespace Infrastructure.MediatorNS.EventManagement
{
    public class EventMediator : IEventMediator
    {
        private readonly IEventFactory _eventFactory;

        private Dictionary<EventTypeEnum, IEventHandler> _eventInstances =
            new Dictionary<EventTypeEnum, IEventHandler>();

        public EventMediator(IEventFactory eventFactory)
        {
            _eventFactory = eventFactory;
        }

        public void Subscribe(EventTypeEnum eventType, IEventListener eventListener)
        {
            if (!HasEventInstance(eventType))
            {
                _eventInstances.Add(eventType, _eventFactory.CreateInstance(eventType));
            }

            _eventInstances[eventType].Subscribe(eventType, eventListener);
        }

        public void Unsubscribe(EventTypeEnum eventType, IEventListener eventListener)
        {
            _eventInstances[eventType].Unsubscribe(eventType, eventListener);

            if (!HasEventInstance(eventType))
            {
                _eventInstances.Remove(eventType);
            }
        }

        public bool HasEventInstance(EventTypeEnum eventType)
        {
            return _eventInstances.ContainsKey(eventType);
        }

        private void CreateInstance(EventTypeEnum eventType)
        {
            var instance = _eventFactory.CreateInstance(eventType);
            _eventInstances.Add(eventType, instance);
        }

        public IEventHandler GetEventHandlerInstance(EventTypeEnum eventType)
        {
            if (!HasEventInstance(eventType))
            {
                CreateInstance(eventType);
            }

            return _eventInstances[eventType];
        }

        public void Notify(EventTypeEnum eventType, IEvent eventData)
        {
            if (HasEventInstance(eventType))
            {
                _eventInstances[eventType].Notify(eventType, eventData);
            }
        }
    }
}