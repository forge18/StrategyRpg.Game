using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class GetEntityByEntityIdQuery : IQuery
    {
        public string WorldId { get; set; }
        public int EntityId { get; set; }

        public GetEntityByEntityIdQuery(string worldId, int entityId)
        {
            WorldId = worldId;
            EntityId = entityId;
        }
    }

    public class GetEntityByEntityIdHandler : QueryHandler
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetEntityByEntityIdHandler(IEcsWorldService ecsWorldService) : 
            base(ecsWorldService) 
        {
            _ecsWorldService = ecsWorldService;
        }

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.GetEntityByEntityId;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as GetEntityByEntityIdQuery;
            var worldId = query.WorldId;
            var entityId = query.EntityId;

            Entity resultEntity = default;
            var world = worldId == "default" ? GetWorld() : _ecsWorldService.GetWorld(worldId);
            var entities = world.GetEntities().With<EntityId>().AsSet().GetEntities();
            foreach (var entity in entities)
            {
                var entityIdComponent = entity.Get<EntityId>();
                if (entityIdComponent.Value == entityId)
                {
                    resultEntity = entity;
                    break;
                }
            }

            var result = new QueryResult(
                QueryTypeEnum.GetEntityByEntityId,
                resultEntity != default,
                resultEntity,
                resultEntity != default ? typeof(DefaultEcs.Entity) : null
            );

            return Task.FromResult(result);
        }
    }
}