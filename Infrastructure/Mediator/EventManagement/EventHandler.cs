using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorNS.EventManagement
{
    public class EventHandler : IEventHandler
    {
        private Dictionary<EventTypeEnum, IEnumerable<IEventListener>> _eventListeners =
            new Dictionary<EventTypeEnum, IEnumerable<IEventListener>>();

        public Task Handle(
            EventActionEnum action,
            EventTypeEnum eventType,
            IEvent eventData,
            IEventListener listener = null,
            CancellationToken cancellationToken = default
        )
        {
            switch (action)
            {
                case EventActionEnum.Notify:
                    Notify(eventType, eventData);
                    break;
                case EventActionEnum.Subscribe:
                    Subscribe(eventType, listener);
                    break;
                case EventActionEnum.Unsubscribe:
                    Unsubscribe(eventType, listener);
                    break;
            }

            return Task.CompletedTask;
        }

        public void Notify(EventTypeEnum eventType, IEvent eventData)
        {
            if (_eventListeners.ContainsKey(eventType))
            {
                var listeners = _eventListeners[eventType] as List<IEventListener>;
                foreach (var listener in listeners)
                {
                    listener.OnEvent(eventType, eventData);
                }
            }
        }

        public void Subscribe(EventTypeEnum eventType, IEventListener eventListener)
        {
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners.Add(eventType, new List<IEventListener>());
            }

            var listeners = _eventListeners[eventType] as List<IEventListener>;
            listeners.Add(eventListener);
        }

        public void Unsubscribe(EventTypeEnum eventType, IEventListener eventListener)
        {
            if (_eventListeners.ContainsKey(eventType))
            {
                var listeners = _eventListeners[eventType] as List<IEventListener>;
                listeners.Remove(eventListener);
            }
        }
    }
}