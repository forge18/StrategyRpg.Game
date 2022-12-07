using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public interface IMediator
    {
        Task<NoResult> ExecuteCommand(CommandTypeEnum commandType, ICommand commandData);
        void SubscribeToEvent(EventTypeEnum eventType, IEventListener eventListener);
        void UnsubscribeFromEvent(EventTypeEnum eventType, IEventListener eventListener);
        void NotifyOfEvent(EventTypeEnum eventType, IEvent eventData);
        Task<QueryResult> RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData = null);
    }
}