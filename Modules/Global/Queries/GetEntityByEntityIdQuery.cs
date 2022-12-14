using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Global
{
    public class GetEntityByEntityIdQuery : IQuery
    {
        public string WorldName { get; set; }
        public int EntityId { get; set; }

        public GetEntityByEntityIdQuery(EcsWorldEnum worldName, int entityId)
        {
            WorldName = worldName.ToString();
            EntityId = entityId;
        }
    }

    public class GetEntityByEntityIdHandler : IQueryHandler<GetEntityByEntityIdQuery>, IHasEnum
    {
        private readonly World _world;

        public GetEntityByEntityIdHandler(IEcsWorldService ecsWorldService)
        {
            _world = ecsWorldService.GetWorld();
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetEntityByEntityId;
        }

        public Task<QueryResult> Handle(GetEntityByEntityIdQuery query, CancellationToken cancellationToken = default)
        {
            var entities = _world.GetEntities().With<EntityId>().AsSet();
            Entity entity = default;

            foreach (var e in entities.GetEntities())
            {
                var entityId = e.Get<EntityId>();
                if (entityId.Value == query.EntityId)
                {
                    entity = e;
                    break;
                }
            }

            var result = new QueryResult(
                QueryTypeEnum.GetEntityByEntityId,
                true,
                entity,
                typeof(Entity)
            );

            return Task.FromResult(result);
        }
    }
}