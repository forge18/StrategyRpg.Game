using System;
using System.Collections.Generic;
using Features.Combat.ArenaSetup;
using Features.Combat.GridActions;
using Features.Global;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Features.Unit;

namespace Infrastructure.HubMediator
{
    public class QueryFactory : IQueryFactory
    {
        private Dictionary<QueryTypeEnum, Type> _queryHandlers
            = new Dictionary<QueryTypeEnum, Type>()
            {
                { QueryTypeEnum.GetArenaScenario, typeof(GetArenaScenarioHandler) },
                { QueryTypeEnum.GetCellIdByPosition, typeof(GetCellIdByPositionHandler) },
                { QueryTypeEnum.GetEntitiesToRender, typeof(GetEntitiesToRenderHandler) },
                { QueryTypeEnum.GetEntityByEntityId, typeof(GetEntityByEntityIdHandler) },
                { QueryTypeEnum.GetEntityBySchemaId, typeof(GetEntityBySchemaIdHandler) },
                { QueryTypeEnum.GetGridPath, typeof(GetGridPathHandler) },
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
