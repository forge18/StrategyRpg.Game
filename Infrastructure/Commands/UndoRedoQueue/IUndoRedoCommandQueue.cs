namespace Infrastructure.Commands
{
    public interface IUndoRedoCommandQueue
    {
        void Store(IUndoRedoCommand command);
        bool Process();
        void Undo();
    }

    public interface IUndoRedoCommandInvoker<TArgument>
    {
        void Store(IUndoRedoCommand<TArgument> command);
        bool Process();
        void Undo();
    }
}