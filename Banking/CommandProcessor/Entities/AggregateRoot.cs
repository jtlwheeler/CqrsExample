using System.Collections.Generic;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public abstract class AggregateRoot
    {
        private int nextEventVersionToAssign;

        public List<Event> Changes { get; private set; }

        protected AggregateRoot()
        {
            Changes = new List<Event>();
            nextEventVersionToAssign = 1;
        }

        protected abstract void When(Event @event);

        public void Replay(Event @event)
        {
            GuardAgainstVersionMismatch(@event.Version);

            When(@event);
            nextEventVersionToAssign++;
        }

        protected void Apply(Event @event)
        {
            @event.Version = nextEventVersionToAssign++;

            When(@event);
            Changes.Add(@event);
        }

        private void GuardAgainstVersionMismatch(int eventVersion)
        {
            if (eventVersion != nextEventVersionToAssign)
            {
                throw new EntityException($"Unexpected event version. Expected version '{eventVersion}' to be '{nextEventVersionToAssign}'");
            }
        }
    }
}
