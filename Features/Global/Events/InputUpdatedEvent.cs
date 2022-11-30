
using System.Collections.Generic;
using Infrastructure.HubMediator;

namespace Features.Global
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
