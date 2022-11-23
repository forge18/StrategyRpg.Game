using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Commands
{
    public class UndoRedoCommandQueue : IUndoRedoCommandQueue
    {
        private Queue _queue = new Queue();
        private Stack<IUndoRedoCommand> _undoStack = new Stack<IUndoRedoCommand>();
        private Stack<IUndoRedoCommand> _redoStack = new Stack<IUndoRedoCommand>();

        public void Store(IUndoRedoCommand command)
        {
            _queue.Enqueue(command);
        }

        public bool Process()
        {
            if (_queue.Count == 0)
                return false;

            IUndoRedoCommand command = (IUndoRedoCommand)_queue.Dequeue();
            command.Execute();
            _undoStack.Push(command);
            return true;
        }

        public void Undo()
        {
            if (_undoStack.Count == 0)
                return;

            IUndoRedoCommand command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }

        public void Redo()
        {
            if (_redoStack.Count == 0)
                return;

            IUndoRedoCommand command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }
}