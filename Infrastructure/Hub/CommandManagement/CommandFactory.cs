using System;
using System.Collections.Generic;
using Features.Exploration.Unit.Commands.MovePlayer;
using Microsoft.Extensions.DependencyInjection;
using Features.Exploration.Unit.Commands.RenderUnit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Hub.CommandManagement
{
    public class CommandFactory : ICommandFactory
    {
        private Dictionary<CommandTypeEnum, Type> _commandTypes = new Dictionary<CommandTypeEnum, Type>()
        {
            { CommandTypeEnum.MovePlayer, typeof(MovePlayerHandler) },
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