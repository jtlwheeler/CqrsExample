using System.Collections.Generic;
using CommandProcessor.Events;

namespace CommandProcessor.Entities
{
    public abstract class Entity
    {
        public List<IEvent> Changes { get; private set; }

        protected Entity()
        {
            Changes = new List<IEvent>();
        }

        public void Apply(IEvent @event)
        {
            When(@event);
            Changes.Add(@event);
        }

        protected abstract void When(IEvent @event);
    }
}
