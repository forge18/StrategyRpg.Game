using System;
using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public interface ICommandProcessor
    {
        object GetCommand(CommandTypeEnum commandType);
        Type GetCommandType(CommandTypeEnum commandHandlerEnum);
        Task<NoResult> ExecuteCommand(CommandTypeEnum commandType, ICommand commandData);
    }
}