using System;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public class BankAccount: Entity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        public void Open(string name)
        {
            var newAccountId = Guid.NewGuid();
            var @event = new BankAccountCreatedEvent(name, newAccountId, 1);
            Apply(@event);
        }

        protected override void When(IEvent @event)
        {
            switch (@event)
            {
                case BankAccountCreatedEvent accountCreatedEvent:
                    Name = accountCreatedEvent.Name;
                    Id = accountCreatedEvent.EntityId;
                    break;
            }
        }

        public void MakeDeposit(string description, decimal amount)
        {
            Balance += amount;
            var @event = new DepositMadeEvent(description, amount, Id, 0);
            Apply(@event);
        }
    }
}
