using System;

namespace CommandProcessor.Events.Events
{
    public class GreetingCreatedEvent : IEvent
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }

        public DateTime Timestamp { get; private set; }

        public string Type { get; private set; }
        public int Version { get; private set; }
        public string Message { get; private set; }

        public GreetingCreatedEvent(string message)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            Type = "GreetingCreatedEvent";
            Message = message;
        }
    }
}