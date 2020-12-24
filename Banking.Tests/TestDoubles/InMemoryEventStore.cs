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
        public List<IEvent> SavedEvents { get; private set; }

        public InMemoryEventStore()
        {
            SavedEvents = new List<IEvent>();
        }

        public async Task Save(IEvent @event)
        {
            await Task.Run(() => SavedEvents.Add(@event));
        }

        public async Task<List<IEvent>> GetEvents(Guid entityId)
        {
            return await Task.Run(() =>
                SavedEvents
                    .Where(@event => @event.EntityId == entityId)
                    .OrderBy(@event => @event.Version)
                    .ToList()
            );
        }
    }
}
