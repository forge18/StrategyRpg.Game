using System.Threading;
using System.Threading.Tasks;
using Infrastructure.HubMediator;

namespace Features.Arena.ArenaSetup
{
    public class GetUnitsToLoadQuery : IQuery
    {
        
    }

    public class GetUnitsToLoadHandler : IQueryHandler<GetUnitsToLoadQuery>
    {
        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetUnitsToLoad;
        }

        public Task<QueryResult> Handle(GetUnitsToLoadQuery query, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}