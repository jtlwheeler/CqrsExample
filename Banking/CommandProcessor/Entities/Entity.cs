using System.Collections.Generic;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public abstract class Entity
    {
        protected int NextEventVersionToAssign { get; private set; }

        public List<IEvent> Changes { get; private set; }

        protected Entity()
        {
            Changes = new List<IEvent>();
            NextEventVersionToAssign = 1;
        }

        protected abstract void When(IEvent @event);

        public void Replay(IEvent @event)
        {
            When(@event);
            NextEventVersionToAssign = @event.Version + 1;
        }

        protected void Apply(IEvent @event)
        {
            When(@event);
            Changes.Add(@event);
            NextEventVersionToAssign++;
        }
    }
}
