using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Hub.QueryManagement.Queries;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Hub.QueryManagement
{
    public class QueryFactory : IQueryFactory
    {
        private Dictionary<QueryTypeEnum, Type> _queryHandlers 
            = new Dictionary<QueryTypeEnum, Type>()
            {
                { QueryTypeEnum.GetEntitiesToRender, typeof(GetEntitiesToRenderHandler) },
                { QueryTypeEnum.GetPlayerEntity, typeof(GetPlayerEntityHandler) },
                { QueryTypeEnum.PlayerIsMoving, typeof(PlayerIsMovingHandler) }
            };

        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<QueryFactory> _logger;

        public QueryFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<QueryFactory>();
        }

        public Type GetQueryType(QueryTypeEnum queryHandlerEnum)
        {
            return _queryHandlers[queryHandlerEnum];
        }

        public IQueryHandler CreateInstance(Type type)
        {
            return (IQueryHandler)ActivatorUtilities.CreateInstance(
                _serviceProvider,
                type
            );
        }
    }
}
