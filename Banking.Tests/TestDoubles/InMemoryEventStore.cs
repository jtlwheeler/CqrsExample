using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.CommandProcessor.Events.EventStore;
using Banking.Events;

namespace Banking.Tests.TestDoubles
{
    public class InMemoryEventStore : IEventStore
    {
        public List<Event> SavedEvents { get; private set; }

        public InMemoryEventStore()
        {
            SavedEvents = new List<Event>();
        }

        public async Task Save(Event @event)
        {
            await Task.Run(() => SavedEvents.Add(@event));
        }

        public async Task<List<Event>> GetEvents(Guid aggregateRootId)
        {
            return await Task.Run(() =>
                SavedEvents
                    .Where(@event => @event.AggregateRootId == aggregateRootId)
                    .OrderBy(@event => @event.Version)
                    .ToList()
            );
        }
    }
}
