using System;

namespace CommandProcessor.Events.Events
{
    public class GreetingCreatedEvent : IEvent
    {
        public Guid Id { get; private set; }

        public DateTime Timestamp { get; private set; }

        public string Name { get; private set; }

        public string Message { get; private set; }

        public GreetingCreatedEvent(string message)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            Name = "GreetingCreatedEvent";
            Message = message;
        }
    }
}