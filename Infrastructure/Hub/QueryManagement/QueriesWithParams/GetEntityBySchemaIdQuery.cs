using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Worlds;
using Infrastructure.Hub.QueryManagement.Dto;

namespace Infrastructure.Hub.QueryManagement.QueriesWithParams
{
    public class GetEntityBySchemaIdQuery : IQuery
    {
        public string SchemaId { get; set; }
    }

    public class GetEntityBySchemaIdQueryHandler : QueryHandler
    {
        public GetEntityBySchemaIdQueryHandler(IEcsWorldService ecsWorldService) : 
            base(ecsWorldService) {}

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.GetEntityByEntityId;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as GetEntityBySchemaIdQuery;
            var schemaId = query.SchemaId;

            Entity resultEntity = default;
            var entities = _world.GetEntities().With<SchemaId>().AsSet().GetEntities();
            foreach (var entity in entities)
            {
                var schemaIdComponent = entity.Get<SchemaId>();
                if (schemaIdComponent.Value == schemaId)
                {
                    resultEntity = entity;
                    break;
                }
            }

            var result = new QueryResult(
                QueryTypeEnum.GetEntityBySchemaId,
                resultEntity != default,
                resultEntity,
                resultEntity != default ? typeof(DefaultEcs.Entity) : null
            );

            return Task.FromResult(result);
        }
    }
}