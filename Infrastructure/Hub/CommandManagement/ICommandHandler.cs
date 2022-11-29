using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public interface ICommandHandler
    {
        CommandTypeEnum GetEnum();
        Task Handle(ICommand command, CancellationToken cancellationToken = default);
    }
}