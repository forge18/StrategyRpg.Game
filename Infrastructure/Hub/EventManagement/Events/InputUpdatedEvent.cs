
using System.Collections.Generic;

namespace Infrastructure.HubMediator
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
