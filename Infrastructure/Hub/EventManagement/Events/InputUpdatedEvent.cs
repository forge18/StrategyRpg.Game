
using System.Collections.Generic;

namespace Infrastructure.Hub.EventManagement.Events
{
    public class InputUpdatedEvent : IEvent
    {
        public Dictionary<InputButtonsEnum, bool> ButtonStatus;
    }

    public class InputUpdatedHandler : EventHandler
    {
        public override EventTypeEnum GetEnum()
        {
            return EventTypeEnum.InputUpdated;
        }
    }
}
