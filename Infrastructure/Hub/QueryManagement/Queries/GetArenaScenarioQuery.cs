using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;

namespace Infrastructure.HubMediator
{
    public class GetArenaScenarioQuery : IQuery
    {

    }

    public class GetArenaScenarioHandler : QueryHandler
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetArenaScenarioHandler(IEcsWorldService ecsWorldService) : 
            base(ecsWorldService) 
        {
            _ecsWorldService = ecsWorldService;
        }

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.GetArenaScenario;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as GetArenaScenarioQuery;
            var arena = _ecsWorldService.GetWorld("arena");
            var scenarioResult = arena.GetEntities().With<CurrentScenario>().AsSet().GetEntities();

            string scenario = default;
            if (scenarioResult.Length > 0)
                scenario = scenarioResult[0].Get<CurrentScenario>().Value;

            var result = new QueryResult(
                QueryTypeEnum.GetArenaScenario,
                scenario != default,
                scenario,
                scenario != default ? typeof(CurrentScenario) : null
            );

            return Task.FromResult(result);
        }
    }
}