using System.Collections.Generic;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public abstract class Entity
    {
        protected int NextEventVersionToAssign => Changes.Count + 1;

        public List<IEvent> Changes { get; private set; }

        protected Entity()
        {
            Changes = new List<IEvent>();
        }

        public abstract void When(IEvent @event);

        protected void Apply(IEvent @event)
        {
            When(@event);
            Changes.Add(@event);
        }
    }
}
