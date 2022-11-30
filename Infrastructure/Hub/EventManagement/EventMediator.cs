using System.Collections.Generic;
using Features.Global;
using Microsoft.Extensions.Logging;

namespace Infrastructure.HubMediator
{
    public class EventMediator : IEventMediator
    {
        private readonly IEventFactory _eventFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<EventMediator> _logger;

        private Dictionary<EventTypeEnum, IEventHandler<IEvent>> _eventInstances =
            new Dictionary<EventTypeEnum, IEventHandler<IEvent>>()
            {
                {EventTypeEnum.EcsSystemsLoaded, new EcsSystemsLoadedHandler()},
                {EventTypeEnum.InputUpdated, new InputUpdatedHandler()}
            };

        public EventMediator(IEventFactory eventFactory, ILoggerFactory loggerFactory)
        {
            _eventFactory = eventFactory;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<EventMediator>();
        }

        public void Subscribe(EventTypeEnum eventTypeEnum, IEventListener eventListener)
        {
            if (!HasEventInstance(eventTypeEnum))
            {
                _eventInstances.Add(eventTypeEnum, _eventFactory.CreateInstance(eventTypeEnum));
            }

            _eventInstances[eventTypeEnum].Subscribe(eventTypeEnum, eventListener);
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

        public void Notify(EventTypeEnum eventType, IEvent eventData)
        {
            if (HasEventInstance(eventType))
            {
                _eventInstances[eventType].Notify(eventType, eventData);
            }
        }
    }
}