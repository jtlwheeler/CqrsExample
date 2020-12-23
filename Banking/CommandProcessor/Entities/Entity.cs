using System.Collections.Generic;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public abstract class Entity
    {
        private int _nextVersionToAssign = 1;

        public List<IEvent> Changes { get; private set; }

        protected Entity()
        {
            Changes = new List<IEvent>();
        }

        protected abstract void When(IEvent @event);

        public void Apply(IEvent @event)
        {
            When(@event);
            Changes.Add(@event);
        }

        protected int GetVersionAndIncrement() => _nextVersionToAssign++;
    }
}
