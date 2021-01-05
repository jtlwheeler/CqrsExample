using System;

namespace Banking.Events
{
    public class DepositMadeEvent: Event
    {
        public const string EventTypeName = "DepositMadeEvent";
        public string Description { get; private set; }
        public decimal Amount { get; private set; }

        public DepositMadeEvent(Guid depositId, string description, decimal amount, Guid accountId, int version)
        {
            AggregateRootId = accountId;
            EntityId = depositId;
            Id = Guid.NewGuid();
            Description = description;
            Amount = amount;
            Timestamp = DateTime.UtcNow;
            Type = EventTypeName;
            Version = version;
        }
    }
}