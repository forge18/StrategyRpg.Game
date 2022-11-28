using System;
using System.Threading.Tasks;

namespace Infrastructure.Hub.CommandManagement
{
    public interface ICommandMediator
    {
        ICommandHandler GetCommand(CommandTypeEnum commandType);
        Type GetCommandType(CommandTypeEnum commandHandlerEnum);
        Task ExecuteCommand(CommandTypeEnum commandType, ICommand commandData);
    }
}