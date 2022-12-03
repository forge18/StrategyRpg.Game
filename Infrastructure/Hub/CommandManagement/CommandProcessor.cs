using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DependencyInjection;
using System.Linq;
using Infrastructure.Hub;

namespace Infrastructure.HubMediator
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandProcessor> _logger;

        private static Dictionary<CommandTypeEnum, Type> _commandTypes =
            new Dictionary<CommandTypeEnum, Type>();

        public CommandProcessor(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<CommandProcessor>();

            if (_commandTypes.Count == 0)
            {
                LoadCommandTypes();
            }
        }

        public void LoadCommandTypes()
        {
            var handlers = typeof(Startup).Assembly.GetTypes()
            	.Where(t =>
					t.GetInterfaces()
					.Any(i =>
						i.IsGenericType &&
						i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
					)
				);
  
            foreach (var handler in handlers)  
            {  
                var commandInstance = (IHasEnum)ActivatorUtilities.CreateInstance(_serviceProvider, handler);
                var commandTypeEnum = (CommandTypeEnum)commandInstance.GetEnum();
                if (!_commandTypes.ContainsKey(commandTypeEnum))
                {
                    _commandTypes.Add(commandTypeEnum, handler);
                }
            }
        }

        public object GetCommand(CommandTypeEnum commandTypeEnum)
        {
            var type = GetCommandType(commandTypeEnum);
            return ActivatorUtilities.CreateInstance(_serviceProvider, type);
        }

        public Type GetCommandType(CommandTypeEnum commandHandlerEnum)
        {
            if (!_commandTypes.ContainsKey(commandHandlerEnum))
            {
                throw new Exception($"Command type {commandHandlerEnum} not found");
            }

            return _commandTypes[commandHandlerEnum];
        }

        public Task<NoResult> ExecuteCommand(CommandTypeEnum commandType, ICommand commandData)
        {
            var command = GetCommand(commandType);
            var method = ((object)command).GetType().GetMethod("Handle");
            var result = method.Invoke(command, new object[]{ commandData, default });
            
            return Task.FromResult<NoResult>(new NoResult());
        }
    }
}