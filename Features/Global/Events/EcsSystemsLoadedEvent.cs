using DefaultEcs.System;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class EcsSystemsLoadedEvent : IEvent
    {
        public ISystem<float> Systems;

        public EcsSystemsLoadedEvent(ISystem<float> systems)
        {
            Systems = systems;
        }
    }

    public class EcsSystemsLoadedHandler : EventHandler
    {
        public override EventTypeEnum GetEnum()
        {
            return EventTypeEnum.EcsSystemsLoaded;
        }
    }
}