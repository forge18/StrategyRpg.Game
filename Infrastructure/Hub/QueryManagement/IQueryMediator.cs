using System.Threading.Tasks;
using Infrastructure.Hub.QueryManagement.Dto;

namespace Infrastructure.Hub.QueryManagement
{
    public interface IQueryMediator
    {
        EmptyQuery EmptyQuery();
        Task<QueryResult> RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData);
    }
}