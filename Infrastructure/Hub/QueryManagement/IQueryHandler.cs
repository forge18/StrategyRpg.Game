using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;

namespace Infrastructure.HubMediator
{
    public interface IQueryHandler
    {
        abstract QueryTypeEnum GetEnum();
        abstract Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default);
        World GetWorld();
        QueryResult CreateResultObject(QueryTypeEnum queryTypeEnum, bool success, object result, System.Type resultType);
    }
}