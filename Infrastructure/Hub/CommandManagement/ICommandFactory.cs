using System;

namespace Infrastructure.HubMediator
{
    public interface ICommandFactory
    {
        ICommandHandler CreateInstance(Type type);
    }
}