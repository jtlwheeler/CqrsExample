using System;
using Banking.CommandProcessor.Entities;
using Banking.Events;

namespace Banking.Tests.TestDoubles
{
    public class FakeId: EntityId
    {
        public FakeId()
        {
            Value = Guid.NewGuid();
        }

        public FakeId(Guid value)
        {
            Value = value;
        }
    }

    public class AggregateRootFake : AggregateRoot
    {
        public FakeId Id { get; set; }
        public int Count { get; private set; }

        protected override void When(Event @event)
        {
            if (@event is FakeEvent)
            {
                Count++;
            }
        }

        public void Increment()
        {
            var @event = new FakeEvent(
                Guid.NewGuid(),
                Id.Value,
                Id.Value,
                DateTime.UtcNow,
                NextEventVersionToAssign
            );

            Apply(@event);
        }

        public void FakeApplyEvent(Event @event)
        {
            Apply(@event);
        }
    }
}
