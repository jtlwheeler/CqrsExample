using System;
using Banking.CommandProcessor.Entities;
using Banking.Events;

namespace Banking.Tests.TestDoubles
{
    public class EntityFake : Entity
    {
        public Guid Id { get; set; }
        public int Count { get; private set; }

        public override void When(IEvent @event)
        {
            if (@event is FakeEvent)
            {
                Count++;
            }
        }

        public void Increment()
        {
            var @event = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = Id,
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = NextEventVersionToAssign
            };

            Apply(@event);
        }
    }
}
