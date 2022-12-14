using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Combat
{
    public class GetUnitsToRemoveFromGridQuery : IQuery
    {
        
    }

    public class GetUnitsToRemoveFromGridHandler : IQueryHandler<GetUnitsToRemoveFromGridQuery>, IHasEnum
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetUnitsToRemoveFromGridHandler(IEcsWorldService ecsWorldService)
        {
            _ecsWorldService = ecsWorldService;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetUnitsToAddToGrid;
        }

        public Task<QueryResult> Handle(GetUnitsToRemoveFromGridQuery query, CancellationToken cancellationToken = default)
        {
            var arena = _ecsWorldService.GetWorld(EcsWorldEnum.Arena);
            var unitIdsToRemove= arena.Get<UnitsToRemoveFromGrid>().Values;

            return Task.FromResult(new QueryResult(
                (QueryTypeEnum)GetEnum(),
                true,
                unitIdsToRemove,
                typeof(int[])
            ));
        }
    }
}