using System;

namespace Infrastructure.MediatorNS.CommandManagement
{
    public class CommandFactory : ICommandFactory
    {
        public ICommand CreateInstance(CommandTypeEnum commandType)
        {
            try
            {
                Type type = Type.GetType(commandType.ToString() + "Command");
                var command = (ICommand)Activator.CreateInstance(type);

                return command;
            }
            catch (Exception ex)
            {
                throw new Exception("CommandFactory.CreateInstance: " + ex.Message);
            }
        }
    }
}