

using System;
using System.Collections.Generic;

namespace Infrastructure.EventManagement.Events.InputUpdated
{
    public class InputUpdatedEvent : Event<InputUpdatedContract>
    {
        private static Dictionary<ISubscriber, Action<InputUpdatedContract>> _subscribers = new Dictionary<ISubscriber, Action<InputUpdatedContract>>();


    }
}