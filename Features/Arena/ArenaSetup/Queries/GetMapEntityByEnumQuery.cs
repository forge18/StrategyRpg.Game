using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Infrastructure.Map;

namespace Features.Arena.ArenaSetup
{
    public class GetMapEntityByEnumQuery : IQuery
    {
        public MapEnum MapEnum;

        public GetMapEntityByEnumQuery(MapEnum mapEnum)
        {
            MapEnum = mapEnum;
        }
    }

    public class GetMapEntityByEnumHandler : IQueryHandler<GetMapEntityByEnumQuery>, IHasEnum
    {
        public readonly World _world;

        public GetMapEntityByEnumHandler(World world)
        {
            _world = world;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetMapEntityByEnum;
        }
        
        public Task<QueryResult> Handle(GetMapEntityByEnumQuery query, CancellationToken cancellationToken = default)
        {
            var entities = _world.GetEntities().With<MapEnumValue>().AsSet().GetEntities();
            foreach (var entity in entities)
            {
                var mapEnumValue = entity.Get<MapEnumValue>();
                if (mapEnumValue.Value == query.MapEnum)
                {
                    var result = new QueryResult(
                        QueryTypeEnum.GetMapEntityByEnum,
                        true,
                        entity,
                        typeof(Entity)
                    );
                    return Task.FromResult(result);
                }
            }

            return Task.FromResult(
                new QueryResult(
                    QueryTypeEnum.GetMapEntityByEnum, 
                    false,
                    null,
                    null
                )
            );
        }
    }
}