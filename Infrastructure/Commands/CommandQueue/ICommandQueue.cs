namespace Infrastructure.Commands
{
    public interface ICommandQueue
    {
        void Store(ICommand command); // Store or queues the command in the invoker
        bool Process(); // Processes the next command in the queue
    }

    public interface ICommandQueue<TArgument>
    {
        void Store(ICommand<TArgument> command);
        bool Process();
    }
}