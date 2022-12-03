using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public interface IQueryProcessor
    {
        EmptyQuery EmptyQuery();
        Task<QueryResult> RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData);
    }
}