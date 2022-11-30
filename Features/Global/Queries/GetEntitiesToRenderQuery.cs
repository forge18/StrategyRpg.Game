using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class GetEntitiesToRenderQuery : IQuery
    {
        public bool Test { get; set; }
    }

    public class GetEntitiesToRenderHandler : QueryHandler
    {
        public GetEntitiesToRenderHandler(IEcsWorldService ecsWorldService, ILoggerFactory loggerFactory)
            : base(ecsWorldService) {}

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.GetEntitiesToRender;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as GetEntitiesToRenderQuery;
            var entities = _world.GetEntities().With<NeedToRender>().AsSet();

            var result = new QueryResult(
                QueryTypeEnum.GetEntitiesToRender,
                true,
                entities,
                typeof(EntitySet)
            );

            return Task.FromResult(result);
        }
    }
}