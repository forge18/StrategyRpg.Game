using System.Threading;
using System.Threading.Tasks;
namespace Infrastructure.HubMediator
{
    public interface IQueryHandler<TQuery> where TQuery : IQuery
    {
        Task<QueryResult> Handle(TQuery query, CancellationToken cancellationToken = default);
    }
}