using System.Threading.Tasks;
using Infrastructure.MediatorNS.CommandManagement;
using Infrastructure.MediatorNS.EventManagement;
using Infrastructure.MediatorNS.QueryManagement;

namespace Infrastructure.MediatorNS
{
    public interface IMediator
    {
        Task ExecuteCommand(CommandTypeEnum commandType, ICommand commandData);
        void SubscribeToEvent(EventTypeEnum eventType, IEventListener eventListener);
        void UnsubscribeFromEvent(EventTypeEnum eventType, IEventListener eventListener);
        void NotifyOfEvent(EventTypeEnum eventType, IEvent eventData);
        IQueryResult RunQuery(QueryTypeEnum queryType, IQuery queryData);
    }
}