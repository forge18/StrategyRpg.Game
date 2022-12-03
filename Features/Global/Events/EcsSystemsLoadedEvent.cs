using DefaultEcs.System;
using Infrastructure.Hub;
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

    public class EcsSystemsLoadedHandler : EventHandler<EcsSystemsLoadedEvent>, IHasEnum
    {
        public override int GetEnum()
        {
            return (int)EventTypeEnum.EcsSystemsLoaded;
        }
    }
}