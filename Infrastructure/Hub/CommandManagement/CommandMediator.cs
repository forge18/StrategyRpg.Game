using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Features.Exploration.Unit;

namespace Infrastructure.HubMediator
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

        public async Task<NoResult> ExecuteCommand(CommandTypeEnum commandType, ICommand commandData)
        {
            var command = GetCommand(commandType);
            await command.Handle(commandData);
            return new NoResult();
        }
    }
}