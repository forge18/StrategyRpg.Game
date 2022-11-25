using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.QueryManagement
{
    public interface IQueryMediator
    {
        Task<IQueryResult> RunQuery(QueryTypeEnum queryType, IQuery queryData);
    }
}