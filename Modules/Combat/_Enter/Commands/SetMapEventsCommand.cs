using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Combat
{
    public class SetMapEventsCommand : ICommand
    {
        public EntitySet MapEvents;

        public SetMapEventsCommand(EntitySet mapEvents)
        {
            MapEvents = mapEvents;
        }
    }

    public class SetMapEventsHandler : ICommandHandler<SetMapEventsCommand>, IHasEnum
    {
        public readonly World _arena;
        public readonly IEcsEntityService _ecsEntityService;

        public SetMapEventsHandler(
            IEcsWorldService ecsWorldService,
            IEcsEntityService ecsEntityService
        )
        {
            _arena = ecsWorldService.GetWorld(EcsWorldEnum.Arena);
            _ecsEntityService = ecsEntityService;
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.SetMapEvents;
        }

        public Task Handle(SetMapEventsCommand command, CancellationToken cancellationToken = default)
        {
            var index = 0;
            var mapEventEntities = command.MapEvents.GetEntities();
            var mapIds = new int[mapEventEntities.Length];
            foreach (var mapEventEntity in mapEventEntities)
            {
                var mapId = _ecsEntityService.ParseEntityId(mapEventEntity);
                mapIds[index] = mapId;
                index++;
            }
            _arena.Set<MapEventEntities>(new MapEventEntities() { Values = mapIds });

            return Task.CompletedTask;
        }
    }
}