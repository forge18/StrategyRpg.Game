using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Worlds;
using Infrastructure.Hub.QueryManagement.Dto;

namespace Infrastructure.Hub.QueryManagement
{
    public abstract class QueryHandler : IQueryHandler
    {
        protected readonly World _world;

        public QueryHandler(IEcsWorldService ecsWorldService)
        {
            _world = ecsWorldService.GetWorld();
        }

        public abstract QueryTypeEnum GetEnum();
        public abstract Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default);

        public World GetWorld()
        {
            return _world;
        }

        public QueryResult CreateResultObject(
            QueryTypeEnum queryTypeEnum, 
            bool success, 
            object result, 
            System.Type resultType
        )
        {
            return new QueryResult(queryTypeEnum, success, result, resultType);
        }
    }
}