using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Infrastructure.DependencyInjection;
using System.Linq;
using Infrastructure.Hub;

namespace Infrastructure.HubMediator
{
    public class EventProcessor: IEventProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<EventProcessor> _logger;

        private static Dictionary<EventTypeEnum, Type> _eventTypes =
            new Dictionary<EventTypeEnum, Type>();

        private static Dictionary<EventTypeEnum, IEventHandler<IEvent>> _eventInstances =
            new Dictionary<EventTypeEnum, IEventHandler<IEvent>>();

        public EventProcessor(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<EventProcessor>();

            if (_eventTypes.Count == 0)
            {
                LoadEventTypes();
            }
        }

        public void LoadEventTypes()
        {
            var handlers = typeof(Startup).Assembly.GetTypes()
            	.Where(t =>
					t.GetInterfaces()
					.Any(i =>
						i.IsGenericType &&
						i.GetGenericTypeDefinition() == typeof(IEventHandler<>)
					)
				);
  
            foreach (var handler in handlers)  
            {  
                if (handler.Name == "EventHandler`1")  
				{  
					continue;  
				}
                var eventInstance = (IHasEnum)ActivatorUtilities.CreateInstance(_serviceProvider, handler);
                var eventTypeEnum = (EventTypeEnum)eventInstance.GetEnum();
                if (!_eventTypes.ContainsKey(eventTypeEnum))
                {
                    _eventTypes.Add(eventTypeEnum, handler);
                }
            }
        }

        public void Subscribe(EventTypeEnum eventTypeEnum, IEventListener eventListener)
        {
            if (!HasEventInstance(eventTypeEnum))
            {
                var eventType = GetEventType(eventTypeEnum);
                _eventInstances.Add(eventTypeEnum, (IEventHandler<IEvent>)ActivatorUtilities.CreateInstance(_serviceProvider, eventType));
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

        public Type GetEventType(EventTypeEnum eventType)
        {
            if (!_eventTypes.ContainsKey(eventType))
            {
                throw new Exception($"Event type {eventType} not found");
            }

            return _eventTypes[eventType];
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