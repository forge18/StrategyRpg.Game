using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.QueryManagement
{
    public interface IQueryMediator
    {
        EmptyQuery EmptyQuery();
        Task<IQueryResult> RunQuery(QueryTypeEnum queryType, IQuery queryData);
    }
}