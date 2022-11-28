using System.Threading.Tasks;
using Infrastructure.Hub.QueryManagement.Dto;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Hub.QueryManagement
{
    public class QueryMediator : IQueryMediator
    {
        private readonly IQueryFactory _queryFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<QueryMediator> _logger;

        public QueryMediator(IQueryFactory queryFactory, ILoggerFactory loggerFactory)
        {
            _queryFactory = queryFactory;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<QueryMediator>();
        }

        public EmptyQuery EmptyQuery()
        {
            return new EmptyQuery();
        }

        public Task<QueryResult> RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData)
        {
            var type = _queryFactory.GetQueryType(queryHandlerEnum);
            var query = _queryFactory.CreateInstance(type);
            var result = query.Handle(queryData);

            return result;
        }
    }
}