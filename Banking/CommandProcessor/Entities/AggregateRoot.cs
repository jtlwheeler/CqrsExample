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
            GuardAgainstVersionMismatch(@event.Version);

            When(@event);
            NextEventVersionToAssign++;
        }

        protected void Apply(Event @event)
        {
            GuardAgainstVersionMismatch(@event.Version);

            When(@event);
            Changes.Add(@event);
            NextEventVersionToAssign++;
        }

        private void GuardAgainstVersionMismatch(int eventVersion)
        {
            if (eventVersion != NextEventVersionToAssign)
            {
                throw new EntityException($"Unexpected event version. Expected version '{eventVersion}' to be '{NextEventVersionToAssign}'");
            }
        }
    }
}
