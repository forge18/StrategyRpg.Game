using System.Collections;

namespace Infrastructure.Commands
{
    public class CommandQueue : ICommandQueue
    {
        private Queue _queue = new Queue();

        public void Store(ICommand command)
        {
            _queue.Enqueue(command);
        }

        public bool Process()
        {
            if (_queue.Count == 0)
                return false;

            ICommand command = (ICommand)_queue.Dequeue();
            command.Execute();
            return true;
        }
    }
}