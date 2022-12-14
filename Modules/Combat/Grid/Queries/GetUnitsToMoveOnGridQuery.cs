using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Combat
{
    public class GetUnitsToMoveOnGridQuery : IQuery
    {
        
    }

    public class GetUnitsToMoveOnGridHandler : IQueryHandler<GetUnitsToMoveOnGridQuery>, IHasEnum
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetUnitsToMoveOnGridHandler(IEcsWorldService ecsWorldService)
        {
            _ecsWorldService = ecsWorldService;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetUnitsToMoveOnGrid;
        }

        public Task<QueryResult> Handle(GetUnitsToMoveOnGridQuery query, CancellationToken cancellationToken = default)
        {
            var arena = _ecsWorldService.GetWorld(EcsWorldEnum.Arena);
            var unitIdsToMove = arena.Get<UnitsToMoveOnGrid>().Values;

            return Task.FromResult(new QueryResult(
                (QueryTypeEnum)GetEnum(),
                true,
                unitIdsToMove,
                typeof(int[])
            ));
        }
    }
}