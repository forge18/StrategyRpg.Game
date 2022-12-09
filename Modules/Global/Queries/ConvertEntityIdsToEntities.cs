using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Features.Global.Queries
{
    public class ConvertEntityIdsToEntities
    {
        public class ConvertEntityIdsToEntitiesQuery : IQuery
        {
            public List<int> EntityIds { get; set; }
        }

        public class ConvertEntityIdsToEntitiesHandler : IQueryHandler<ConvertEntityIdsToEntitiesQuery>, IHasEnum
        {
            private readonly IMediator _mediator;
            private readonly World _world;

            public ConvertEntityIdsToEntitiesHandler(IMediator mediator, IEcsWorldService ecsWorldService)
            {
                _mediator = mediator;
                _world = ecsWorldService.GetWorld();
            }

            public int GetEnum()
            {
                return (int)QueryTypeEnum.ConvertEntityIdsToEntities;
            }

            public Task<QueryResult> Handle(ConvertEntityIdsToEntitiesQuery query, CancellationToken cancellationToken = default)
            {
                var entities = new List<Entity>();

                foreach (var entityId in query.EntityIds)
                {
                    var entity = GetEntityByEntityId(entityId);
                    entities.Add(entity);
                }

                var result = new QueryResult(
                    QueryTypeEnum.ConvertEntityIdsToEntities,
                    true,
                    entities,
                    typeof(List<Entity>)
                );

                return Task.FromResult<QueryResult>(result);
            }

            private Entity GetEntityByEntityId(int entityId)
            {
                var entities = _world.GetEntities().With<EntityId>().AsSet();

                foreach (var entity in entities.GetEntities())
                {
                    var id = entity.Get<EntityId>();
                    if (id.Value == entityId)
                    {
                        return entity;
                    }
                }

                return default;
            }
        }
    }
}