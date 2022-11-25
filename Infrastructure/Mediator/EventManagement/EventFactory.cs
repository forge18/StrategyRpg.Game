using System;

namespace Infrastructure.MediatorNS.EventManagement
{
    public class EventFactory : IEventFactory
    {
        public IEventHandler CreateInstance(EventTypeEnum eventType)
        {
            try
            {
                Type type = Type.GetType(eventType.ToString() + "Handler");
                var command = (IEventHandler)Activator.CreateInstance(type);

                return command;
            }
            catch (Exception ex)
            {
                throw new Exception("EventFactory.CreateInstance: " + ex.Message);
            }
        }
    }
}