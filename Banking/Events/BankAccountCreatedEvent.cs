using System;

namespace Banking.Events
{
    public class BankAccountCreatedEvent : IEvent
    {
        public const string EventTypeName = "BankAccountCreatedEvent";
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }
        public int Version { get; private set; }

        public BankAccountCreatedEvent(string name, Guid accountId, int version)
        {
            Id = Guid.NewGuid();
            EntityId = accountId;
            Timestamp = DateTime.UtcNow;
            Type = EventTypeName;
            Name = name;
            Version = version;
        }
    }
}