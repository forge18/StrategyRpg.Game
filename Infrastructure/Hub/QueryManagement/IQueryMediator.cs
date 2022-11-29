using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public interface IQueryMediator
    {
        EmptyQuery EmptyQuery();
        Task<QueryResult> RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData);
    }
}