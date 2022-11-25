using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.CommandManagement
{
    public interface ICommandMediator
    {
        ICommandHandler<ICommand> GetCommand(CommandTypeEnum commandType);
        Task ExecuteCommand(CommandTypeEnum commandType, ICommand commandData);
    }
}