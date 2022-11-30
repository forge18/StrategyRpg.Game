using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Infrastructure.HubMediator
{
    public class Mediator : IMediator
    {
        private readonly ICommandMediator _commandMediator;
        private readonly IEventMediator _eventMediator;
        private readonly IQueryMediator _queryMediator;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<Mediator> _logger;

        public Mediator(
            ICommandMediator commandMediator,
            IEventMediator eventMediator,
            IQueryMediator queryMediator,
            ILoggerFactory loggerFactory
        )
        {
            _commandMediator = commandMediator;
            _eventMediator = eventMediator;
            _queryMediator = queryMediator;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Mediator>();
        }

        // Command Management
        public async Task<NoResult> ExecuteCommand(CommandTypeEnum commandType, ICommand commandData)
        {
            return await _commandMediator.ExecuteCommand(commandType, commandData);
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

        public QueryResult RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData = null)
        {
            var data = queryData != null ? queryData : _queryMediator.EmptyQuery();
            var result = _queryMediator.RunQuery(queryHandlerEnum, queryData);
            return result.Result;
        }
    }
}