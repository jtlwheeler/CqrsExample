using System;
using CommandProcessor.Entities;
using CommandProcessor.Events;
using CommandProcessor.Events.Events;

namespace CommandProcessor.Commands.Entities
{
    public class BankAccount: Entity
    {
        public Guid Id { get; private set; }
        public Guid EntityId { get; private set; }
        public string Name { get; private set; }

        public void Open(string name)
        {
            var newAccountId = Guid.NewGuid();
            var @event = new BankAccountCreatedEvent(name, newAccountId);
            Apply(@event);
        }

        protected override void When(IEvent @event)
        {
            switch (@event)
            {
                case BankAccountCreatedEvent accountCreatedEvent:
                    Name = accountCreatedEvent.Name;
                    Id = Guid.NewGuid();
                    break;
            }
        }
    }
}