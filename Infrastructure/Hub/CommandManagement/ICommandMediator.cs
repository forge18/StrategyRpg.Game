using System;
using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public interface ICommandMediator
    {
        ICommandHandler GetCommand(CommandTypeEnum commandType);
        Type GetCommandType(CommandTypeEnum commandHandlerEnum);
        Task<NoResult> ExecuteCommand(CommandTypeEnum commandType, ICommand commandData);
    }
}