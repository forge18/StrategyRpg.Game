using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Combat
{
    public class GetUnitsToAddToGridQuery : IQuery
    {
        
    }

    public class GetUnitsToAddToGridHandler : IQueryHandler<GetUnitsToAddToGridQuery>, IHasEnum
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetUnitsToAddToGridHandler(IEcsWorldService ecsWorldService)
        {
            _ecsWorldService = ecsWorldService;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetUnitsToAddToGrid;
        }

        public Task<QueryResult> Handle(GetUnitsToAddToGridQuery query, CancellationToken cancellationToken = default)
        {
            var arena = _ecsWorldService.GetWorld(EcsWorldEnum.Arena);
            var unitIdsToAdd = arena.Get<UnitsToAddToGrid>().Values;
            List<int> unitIdsList = new List<int>();
            unitIdsList.AddRange(unitIdsToAdd);

            return Task.FromResult(new QueryResult(
                (QueryTypeEnum)GetEnum(),
                true,
                unitIdsList,
                typeof(List<int>)
            ));
        }
    }
}