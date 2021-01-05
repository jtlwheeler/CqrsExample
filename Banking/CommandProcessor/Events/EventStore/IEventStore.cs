using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Events;

namespace Banking.CommandProcessor.Events.EventStore
{
    public interface IEventStore
    {
        public Task Save(Event @event);
        public Task<List<Event>> GetEvents(Guid aggregateRootId);
    }
}
