using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Infrastructure.HubMediator
{
    public class Mediator: IMediator
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IEventProcessor _eventProcessor;
        private readonly IQueryProcessor _queryProcessor;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<Mediator> _logger;

        public Mediator(
            ICommandProcessor commandProcessor,
            IEventProcessor eventProcessor,
            IQueryProcessor queryProcessor,
            ILoggerFactory loggerFactory
        )
        {
            _commandProcessor = commandProcessor;
            _eventProcessor = eventProcessor;
            _queryProcessor = queryProcessor;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Mediator>();
        }

        // Command Management
        public async Task<NoResult> ExecuteCommand(CommandTypeEnum commandType, ICommand commandData)
        {
            return await _commandProcessor.ExecuteCommand(commandType, commandData);
        }

        // Event Management
        public void SubscribeToEvent(EventTypeEnum eventType, IEventListener eventListener)
        {
            _eventProcessor.Subscribe(eventType, eventListener);
        }

        public void UnsubscribeFromEvent(EventTypeEnum eventType, IEventListener eventListener)
        {
            _eventProcessor.Unsubscribe(eventType, eventListener);
        }

        public void NotifyOfEvent(EventTypeEnum eventType, IEvent eventData)
        {
            _eventProcessor.Notify(eventType, eventData);
        }

        public QueryResult RunQuery(QueryTypeEnum queryHandlerEnum, IQuery queryData = null)
        {
            var data = queryData != null ? queryData : _queryProcessor.EmptyQuery();
            var result = _queryProcessor.RunQuery(queryHandlerEnum, queryData);
            return result.Result;
        }
    }
}