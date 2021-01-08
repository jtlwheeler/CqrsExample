using System;

namespace Banking.Events
{
    public class BankAccountCreatedEvent: Event
    {
        public const string EventTypeName = "BankAccountCreatedEvent";
        public string Name { get; private set; }

        public BankAccountCreatedEvent(string name, Guid accountId, int version = -1)
        {
            Id = Guid.NewGuid();
            AggregateRootId = accountId;
            EntityId = accountId;
            Timestamp = DateTime.UtcNow;
            Type = EventTypeName;
            Name = name;
            Version = version;
        }
    }
}