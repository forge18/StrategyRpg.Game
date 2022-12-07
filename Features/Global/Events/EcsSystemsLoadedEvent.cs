using DefaultEcs.System;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class EcsSystemsLoadedEvent : IEvent
    {

    }

    public class EcsSystemsLoadedHandler : EventHandler<EcsSystemsLoadedEvent>, IHasEnum
    {
        public ISystem<float> Systems;

        public EcsSystemsLoadedHandler(ISystem<float> systems)
        {
            Systems = systems;
        }

        public override int GetEnum()
        {
            return (int)EventTypeEnum.EcsSystemsLoaded;
        }
    }
}