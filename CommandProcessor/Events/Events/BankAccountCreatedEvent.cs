using System;

namespace CommandProcessor.Events.Events
{
    public class BankAccountCreatedEvent : IEvent
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }

        public BankAccountCreatedEvent(string name, Guid accountId)
        {
            Id = Guid.NewGuid();
            EntityId = accountId;
            Timestamp = DateTime.UtcNow;
            Type = "BankAccountCreatedEvent";
            Name = name;
        }
    }
}