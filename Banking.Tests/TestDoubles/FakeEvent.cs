using System;
using Banking.Events;

namespace Banking.Tests.TestDoubles
{
    public class FakeEvent : Event
    {
        public FakeEvent(Guid id, Guid entityId, Guid aggregateRootId,
                            DateTime timestamp, int version)
        {
            Id = id;
            EntityId = entityId;
            AggregateRootId = aggregateRootId;
            Timestamp = timestamp;
            Type = "FakeEvent";
            Version = version;
        }
    }
}
