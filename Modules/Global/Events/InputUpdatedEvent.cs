
using System.Collections.Generic;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class InputUpdatedEvent : IEvent
    {
        public Dictionary<InputButtonsEnum, bool> ButtonStatus;
    }

    public class InputUpdatedHandler : EventHandler<InputUpdatedEvent>, IHasEnum
    {
        public override int GetEnum()
        {
            return (int)EventTypeEnum.InputUpdated;
        }
    }
}
