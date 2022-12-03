using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;
using Infrastructure.Hub;

namespace Features.Global
{
    public class GetEntitiesToRenderQuery : IQuery
    {
        public bool Test { get; set; }
    }

    public class GetEntitiesToRenderHandler : IQueryHandler<GetEntitiesToRenderQuery>, IHasEnum
    {
        private readonly World _world;
        private readonly ILogger<GetEntitiesToRenderHandler> _logger;

        public GetEntitiesToRenderHandler(IEcsWorldService ecsWorldService, ILoggerFactory loggerFactory)
        {
            _world = ecsWorldService.GetWorld();
            _logger = loggerFactory.CreateLogger<GetEntitiesToRenderHandler>();
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetEntitiesToRender;
        }

        public Task<QueryResult> Handle(GetEntitiesToRenderQuery query, CancellationToken cancellationToken = default)
        {
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