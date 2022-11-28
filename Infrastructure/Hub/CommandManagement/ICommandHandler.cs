using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Hub.CommandManagement
{
    public interface ICommandHandler
    {
        CommandTypeEnum GetEnum();
        Task Handle(ICommand command, CancellationToken cancellationToken = default);
    }
}