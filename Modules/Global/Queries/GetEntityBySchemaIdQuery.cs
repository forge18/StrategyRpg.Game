using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class GetEntityBySchemaIdQuery : IQuery
    {
        public string SchemaId { get; set; }
    }

    public class GetEntityBySchemaIdHandler : IQueryHandler<GetEntityBySchemaIdQuery>, IHasEnum
    {
        private readonly World _world;

        public GetEntityBySchemaIdHandler(IEcsWorldService ecsWorldService) 
        {
            _world = ecsWorldService.GetWorld();
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetEntityBySchemaId;
        }

        public Task<QueryResult> Handle(GetEntityBySchemaIdQuery query, CancellationToken cancellationToken = default)
        {
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