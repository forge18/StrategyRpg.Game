
using System.Collections.Generic;

namespace Infrastructure.MediatorNS.EventManagement.Events
{
    public class InputUpdatedEvent : IEvent
    {
        public Dictionary<InputButtonsEnum, bool> ButtonStatus;
    }

    public class InputUpdatedHandler : EventHandler
    {

    }
}
