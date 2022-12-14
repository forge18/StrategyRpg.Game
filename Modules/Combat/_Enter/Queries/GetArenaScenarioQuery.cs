using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Combat
{
    public class GetArenaScenarioQuery : IQuery
    {

    }

    public class GetArenaScenarioHandler : IQueryHandler<GetArenaScenarioQuery>, IHasEnum
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetArenaScenarioHandler(IEcsWorldService ecsWorldService) 
        {
            _ecsWorldService = ecsWorldService;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetArenaScenario;
        }

        public Task<QueryResult> Handle(GetArenaScenarioQuery query, CancellationToken cancellationToken = default)
        {
            var arena = _ecsWorldService.GetWorld(EcsWorldEnum.Arena);
            string scenario = default;
            if (arena.Has<CurrentScenario>())
            {
                scenario = arena.Get<CurrentScenario>().Value;
            }

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