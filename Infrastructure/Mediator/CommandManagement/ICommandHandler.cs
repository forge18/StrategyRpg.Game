using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.CommandManagement
{
    public interface ICommandHandler<ICommand>
    {
        Task Handle(ICommand command, CancellationToken cancellationToken = default);
    }
}