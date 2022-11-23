namespace Infrastructure.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<in TArgument>
    {
        void Execute(TArgument argument);
    }

    public interface ICommand<in TArgument1, in TArgument2>
    {
        void Execute(TArgument1 argument1, TArgument2 argument2);
    }

    public interface ICommand<in TArgument1, in TArgument2, in TArgument3>
    {
        void Execute(TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
    }
}