using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.QueryManagement
{
    public class QueryMediator : IQueryMediator
    {
        private readonly IQueryFactory _queryFactory;

        public QueryMediator(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public EmptyQuery EmptyQuery()
        {
            return new EmptyQuery();
        }

        public IQueryHandler<IQuery> GetQuery(QueryTypeEnum queryType)
        {
            return _queryFactory.CreateInstance(queryType) as IQueryHandler<IQuery>;
        }

        public Task<IQueryResult> RunQuery(QueryTypeEnum queryType, IQuery queryData)
        {
            var query = GetQuery(queryType);
            var result = query.Handle(queryData);

            return Task.FromResult((IQueryResult)result);
        }
    }
}