using System.Collections.Generic;
using CommandProcessor.Events;

namespace CommandProcessor.Entities
{
    public abstract class Entity
    {
        List<IEvent> changes;

        protected Entity()
        {
            changes = new List<IEvent>();
        }

        public void Apply(IEvent @event)
        {
            When(@event);
            changes.Add(@event);
        }

        protected abstract void When(IEvent @event);
    }
}
