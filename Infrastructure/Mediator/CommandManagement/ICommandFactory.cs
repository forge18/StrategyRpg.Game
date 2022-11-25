namespace Infrastructure.MediatorNS.CommandManagement
{
    public interface ICommandFactory
    {
        ICommand CreateInstance(CommandTypeEnum commandType);
    }
}