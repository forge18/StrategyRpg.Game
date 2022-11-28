using System;
using System.Collections.Generic;
using Infrastructure.Hub.EventManagement.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Hub.EventManagement
{
    public class EventFactory : IEventFactory
    {
        private Dictionary<EventTypeEnum, Type> _eventTypes =
            new Dictionary<EventTypeEnum, Type>()
            {
                { EventTypeEnum.InputUpdated, typeof(InputUpdatedEvent) }
            };

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventFactory> _logger;

        public EventFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<EventFactory>();
        }

        public dynamic CreateInstance(EventTypeEnum eventTypeEnum)
        {
            switch (eventTypeEnum)
            {
                case EventTypeEnum.InputUpdated:
                    return ActivatorUtilities.CreateInstance(
                        _serviceProvider,
                        typeof(InputUpdatedHandler)
                    );
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventTypeEnum), eventTypeEnum, null);
            }
        }
    }
}