using System.Collections.Generic;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public abstract class AggregateRoot
    {
        protected int NextEventVersionToAssign { get; private set; }

        public List<Event> Changes { get; private set; }

        protected AggregateRoot()
        {
            Changes = new List<Event>();
            NextEventVersionToAssign = 1;
        }

        protected abstract void When(Event @event);

        public void Replay(Event @event)
        {
            When(@event);
            NextEventVersionToAssign = @event.Version + 1;
        }

        protected void Apply(Event @event)
        {
            When(@event);
            Changes.Add(@event);
            NextEventVersionToAssign++;
        }
    }
}
