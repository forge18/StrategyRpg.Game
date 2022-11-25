using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.QueryManagement
{
    public interface IQueryHandler<IQuery>
    {
        Task<QueryResult> Handle(IQuery query, CancellationToken cancellationToken = default);
    }
}