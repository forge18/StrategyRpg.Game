using System;

namespace Infrastructure.Hub.EventManagement
{
    public interface IEventFactory
    {
        dynamic CreateInstance(EventTypeEnum eventType);
    }
}