using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Features.Arena.ArenaSetup
{
    public class GetMapEventsQuery : IQuery
    {
        
    }

    public class GetMapEventsHandler : IQueryHandler<GetMapEventsQuery>, IHasEnum
    {
        public readonly World _world;

        public GetMapEventsHandler(World world)
        {
            _world = world;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetMapEvents;
        }
        
        public Task<QueryResult> Handle(GetMapEventsQuery query, CancellationToken cancellationToken = default)
        {
            var mapEvents = _world.GetEntities().With<IsMapEvent>().AsSet();

            return Task.FromResult(
                new QueryResult(
                    QueryTypeEnum.GetMapEvents, 
                    mapEvents != null,
                    mapEvents,
                    typeof(EntitySet)
                )
            );
        }
    }
}