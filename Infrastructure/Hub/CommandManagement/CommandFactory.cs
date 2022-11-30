using System;
using System.Collections.Generic;
using Features.Combat.ArenaSetup;
using Features.Exploration.Unit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.HubMediator
{
    public class CommandFactory : ICommandFactory
    {
        private Dictionary<CommandTypeEnum, Type> _commandTypes = new Dictionary<CommandTypeEnum, Type>()
        {
            { CommandTypeEnum.CreateArena, typeof(CreateArenaHandler) },
            { CommandTypeEnum.CreateGrid, typeof(CreateGridHandler) },
            { CommandTypeEnum.LoadNonPlayerUnits, typeof(LoadNonPlayerUnitsHandler) },
            { CommandTypeEnum.MovePlayer, typeof(MovePlayerHandler) },
            { CommandTypeEnum.SetObjectives, typeof(SetObjectivesHandler) },
            { CommandTypeEnum.SpawnUnit, typeof(SpawnUnitHandler) }
        };

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandFactory> _logger;

        public CommandFactory(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<CommandFactory>();
        }

        public Type GetCommandType(CommandTypeEnum commandTypeEnum)
        {
            return _commandTypes[commandTypeEnum];
        }

        public ICommandHandler CreateInstance(Type type)
        {
            return (ICommandHandler)ActivatorUtilities.CreateInstance(
                _serviceProvider,
                type
            );
        }
    }
}