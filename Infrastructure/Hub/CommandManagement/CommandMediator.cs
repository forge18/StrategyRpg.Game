using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Features.Exploration.Unit.Commands.MovePlayer;
using Features.Exploration.Unit.Commands.RenderUnit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Hub.CommandManagement
{
    public class CommandMediator : ICommandMediator
    {
        private Dictionary<CommandTypeEnum, Type> _commandTypes =
            new Dictionary<CommandTypeEnum, Type>()
            {
                { CommandTypeEnum.MovePlayer, typeof(MovePlayerHandler) },
                { CommandTypeEnum.SpawnUnit, typeof(SpawnUnitHandler) }
            };

        private readonly ICommandFactory _commandFactory;
        private readonly ILogger<CommandMediator> _logger;

        public CommandMediator(ICommandFactory commandFactory, ILoggerFactory loggerFactory)
        {
            _commandFactory = commandFactory;
            _logger = loggerFactory.CreateLogger<CommandMediator>();
        }

        public ICommandHandler GetCommand(CommandTypeEnum commandTypeEnum)
        {
            var type = GetCommandType(commandTypeEnum);
            return _commandFactory.CreateInstance(type) as ICommandHandler;
        }

        public Type GetCommandType(CommandTypeEnum commandHandlerEnum)
        {
            if (!_commandTypes.ContainsKey(commandHandlerEnum))
            {
                throw new Exception($"Command type {commandHandlerEnum} not found");
            }

            return _commandTypes[commandHandlerEnum];
        }

        public Task ExecuteCommand(CommandTypeEnum commandType, ICommand commandData)
        {
            var command = GetCommand(commandType);
            return command.Handle(commandData);
        }
    }
}