using System.Threading.Tasks;
using Infrastructure.MediatorNS.CommandManagement;
using Infrastructure.MediatorNS.EventManagement;
using Infrastructure.MediatorNS.QueryManagement;

namespace Infrastructure.MediatorNS
{
    public class Mediator : IMediator
    {
        private readonly ICommandMediator _commandMediator;
        private readonly IEventMediator _eventMediator;
        private readonly IQueryMediator _queryMediator;

        public Mediator(ICommandMediator commandMediator, IEventMediator eventMediator, IQueryMediator queryMediator)
        {
            _commandMediator = commandMediator;
            _eventMediator = eventMediator;
            _queryMediator = queryMediator;
        }

        // Command Management
        public Task ExecuteCommand(CommandTypeEnum commandType, ICommand commandData)
        {
            return _commandMediator.ExecuteCommand(commandType, commandData);
        }

        // Event Management
        public void SubscribeToEvent(EventTypeEnum eventType, IEventListener eventListener)
        {
            _eventMediator.Subscribe(eventType, eventListener);
        }

        public void UnsubscribeFromEvent(EventTypeEnum eventType, IEventListener eventListener)
        {
            _eventMediator.Unsubscribe(eventType, eventListener);
        }

        public void NotifyOfEvent(EventTypeEnum eventType, IEvent eventData)
        {
            _eventMediator.Notify(eventType, eventData);
        }

        // Query Management
        public IQueryResult RunQuery(QueryTypeEnum queryType, IQuery queryData = null)
        {
            var data = queryData != null ? queryData : _queryMediator.EmptyQuery();
            return (IQueryResult)_queryMediator.RunQuery(queryType, queryData);
        }
    }
}