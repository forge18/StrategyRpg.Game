using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}