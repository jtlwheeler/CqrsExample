using System.Collections.Generic;

namespace CommandProcessor.Events.Persistence
{
    public class InMemoryEventStore : IEventStore
    {
        private List<IEvent> list;

        public InMemoryEventStore()
        {
            list = new List<IEvent>();
        }

        public void Save(IEvent @event)
        {
            list.Add(@event);
        }
    }
}
