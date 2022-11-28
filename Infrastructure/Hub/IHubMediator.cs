using System.Threading.Tasks;
using Infrastructure.Hub.CommandManagement;
using Infrastructure.Hub.EventManagement;
using Infrastructure.Hub.QueryManagement;
using Infrastructure.Hub.QueryManagement.Dto;

namespace Infrastructure.Hub
{
    public interface IHubMediator
    {
        Task ExecuteCommand(CommandTypeEnum commandType, ICommand commandData);
        void SubscribeToEvent(EventTypeEnum eventType, IEventListener eventListener);
        void UnsubscribeFromEvent(EventTypeEnum eventType, IEventListener eventListener);
        void NotifyOfEvent(EventTypeEnum eventType, IEvent eventData);
        QueryResult RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData = null);
    }
}