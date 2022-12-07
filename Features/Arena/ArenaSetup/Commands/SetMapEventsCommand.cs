using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;

namespace Features.Arena.ArenaSetup
{
    public class SetMapEventsCommand : ICommand
    {
        public EntitySet MapEvents;

        public SetMapEventsCommand(IEcsWorldService ecsWorldService, EntitySet mapEvents)
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
            _arena.Set<MapEvents>(new MapEvents(){ Values = mapIds });

            return Task.CompletedTask;
        }
    }
}