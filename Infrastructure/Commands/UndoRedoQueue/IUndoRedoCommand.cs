namespace Infrastructure.Commands
{
    public interface IUndoRedoCommand : ICommand
    {
        void Undo();
    }

    public interface IUndoRedoCommand<in TArgument1> : ICommand<TArgument1>
    {
        void Undo(TArgument1 argument1);
    }

    public interface IUndoRedoCommand<in TArgument1, in TArgument2> : ICommand<TArgument1, TArgument2>
    {
        void Undo(TArgument1 argument1, TArgument2 argument2);
    }

    public interface IUndoRedoCommand<in TArgument1, in TArgument2, in TArgument3> : ICommand<TArgument1, TArgument2, TArgument3>
    {
        void Undo(TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
    }

}