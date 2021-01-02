using System;

namespace Banking.Events
{
    public class DepositMadeEvent : IEvent
    {
        public const string EventTypeName = "DepositMadeEvent";
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public Guid AggregateRootId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Type { get; private set; }
        public int Version { get; private set; }

        public Guid DepositId { get; private set; }
        public string Description { get; private set; }
        public decimal Amount { get; private set; }


        public DepositMadeEvent(Guid depositId, string description, decimal amount, Guid accountId, int version)
        {
            AggregateRootId = accountId;
            EntityId = accountId;
            DepositId = depositId;
            Id = Guid.NewGuid();
            Description = description;
            Amount = amount;
            Timestamp = DateTime.UtcNow;
            Type = EventTypeName;
            Version = version;
        }
    }
}