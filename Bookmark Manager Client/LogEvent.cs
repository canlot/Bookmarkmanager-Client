using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client
{
    public enum EventType
    {
        None,
        Informational,
        Warning,
        Error,
        Critical
    }
    public class EventMessage
    {
        public string Message { get; set; }
        public EventType EventType { get; set; }
        public Exception Exceptions { get; set; }
    }
    public class LogEvent
    {
        public string Message { get; set; }
        public EventType EventType { get; set; }
        public void Log(EventType eventType, string message)
        {
            var eventMessage = new EventMessage
            {
                Message = message,
                EventType = eventType
            };
            ObjectRepository.EventDispatcher.Send<EventMessage>(eventMessage);
        }
        public void Clear()
        {
            var eventMessage = new EventMessage
            {
                Message = string.Empty,
                EventType = EventType.None
            };
            ObjectRepository.EventDispatcher.Send(eventMessage);
        }
    }
}
