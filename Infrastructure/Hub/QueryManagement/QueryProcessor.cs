using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Infrastructure.DependencyInjection;
using System.Linq;
using Infrastructure.Hub;

namespace Infrastructure.HubMediator
{
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<QueryProcessor> _logger;

        private static Dictionary<QueryTypeEnum, Type> _queryTypes =
            new Dictionary<QueryTypeEnum, Type>();

        public QueryProcessor(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<QueryProcessor>();

            if (_queryTypes.Count == 0)
            {
                LoadQueryTypes();
            }
        }

        public void LoadQueryTypes()
        {
            var handlers = typeof(Startup).Assembly.GetTypes()
            	.Where(t =>
					t.GetInterfaces()
					.Any(i =>
						i.IsGenericType &&
						!i.ContainsGenericParameters &&
						i.GetGenericTypeDefinition() == typeof(IQueryHandler<>)
					)
				);
  
            foreach (var handler in handlers)  
            {  
                var queryInstance = (IHasEnum)ActivatorUtilities.CreateInstance(_serviceProvider, handler);
                var queryTypeEnum = (QueryTypeEnum)queryInstance.GetEnum();
                if (!_queryTypes.ContainsKey(queryTypeEnum))
                {
                    _queryTypes.Add(queryTypeEnum, handler);
                }   
            }
        }

        public EmptyQuery EmptyQuery()
        {
            return new EmptyQuery();
        }

        public Task<QueryResult> RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData)
        {
            var query = GetQuery(queryHandlerEnum);
            var method = ((object)query).GetType().GetMethod("Handle");
            var result = (Task<QueryResult>)method.Invoke(query, new object[]{ queryData, default });

            return result;
        }

        public dynamic GetQuery(QueryTypeEnum queryTypeEnum)
        {
            var type = GetQueryType(queryTypeEnum);
            return ActivatorUtilities.CreateInstance(_serviceProvider, type);
        }

        public Type GetQueryType(QueryTypeEnum queryHandlerEnum)
        {
            if (!_queryTypes.ContainsKey(queryHandlerEnum))
            {
                throw new Exception($"Query type {queryHandlerEnum} not found");
            }

            return _queryTypes[queryHandlerEnum];
        }
    }
}