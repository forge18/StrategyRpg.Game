using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.EventManagement
{
    public interface IEventHandler
    {
        Task Handle(
            EventActionEnum action,
            EventTypeEnum eventType,
            IEvent eventData,
            IEventListener listener = null,
            CancellationToken cancellationToken = default
        );
        void Notify(EventTypeEnum eventType, IEvent eventData);
        void Subscribe(EventTypeEnum eventType, IEventListener eventListener);
        void Unsubscribe(EventTypeEnum eventType, IEventListener eventListener);

    }
}