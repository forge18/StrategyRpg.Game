using System;

namespace Infrastructure.Hub.CommandManagement
{
    public interface ICommandFactory
    {
        ICommandHandler CreateInstance(Type type);
    }
}