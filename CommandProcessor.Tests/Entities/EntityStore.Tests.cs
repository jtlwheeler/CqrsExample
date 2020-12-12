using Xunit;
using FluentAssertions;
using Moq;
using CommandProcessor.Events.Persistence;
using CommandProcessor.Events;
using CommandProcessor.Commands.Entities;
using CommandProcessor.Entities;
using System;

namespace CommandProcessor.Tests.Entities
{
    public class EntityStoreTests
    {
        [Fact]
        public void ShouldSaveEventsForEntity()
        {
            var db = new Mock<IEventStore>();
            var entityStore = new EntityStore(db.Object);

            var entity = new EntityFake();
            var event1 = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = 1
            };

            var event2 = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = 2
            };

            var event3 = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = 3
            };

            entity.Apply(event1);
            entity.Apply(event2);
            entity.Apply(event3);

            entityStore.Save<EntityFake>(entity);

            db.Verify(m => m.Save(event1), Times.Once);
            db.Verify(m => m.Save(event2), Times.Once);
            db.Verify(m => m.Save(event3), Times.Once);

        }
    }

    public class EntityFake : Entity
    {
        protected override void When(IEvent @event)
        {
            // No-op
        }
    }

    public class FakeEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
    }
}