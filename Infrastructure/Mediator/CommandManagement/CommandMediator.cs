using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.CommandManagement
{
    public class CommandMediator : ICommandMediator
    {
        private readonly ICommandFactory _commandFactory;

        public CommandMediator(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public ICommandHandler<ICommand> GetCommand(CommandTypeEnum commandType)
        {
            return _commandFactory.CreateInstance(commandType) as ICommandHandler<ICommand>;
        }

        public Task ExecuteCommand(CommandTypeEnum commandType, ICommand commandData)
        {
            var command = GetCommand(commandType);
            return command.Handle(commandData);
        }
    }
}