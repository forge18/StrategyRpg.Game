using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Combat
{
    public class GetUnitsToLoadQuery : IQuery
    {

    }

    public class GetUnitsToLoadHandler : IQueryHandler<GetUnitsToLoadQuery>, IHasEnum
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