using System;
using Banking.Events;

namespace Banking.Tests.TestDoubles
{
    public class FakeEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
    }
}
