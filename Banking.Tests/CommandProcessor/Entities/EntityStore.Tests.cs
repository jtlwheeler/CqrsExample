using Xunit;
using Moq;
using System;
using Banking.CommandProcessor.Entities;
using Banking.Events;
using Banking.CommandProcessor.Events.EventStore;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Banking.Tests.CommandProcessor.Entities
{
    public class EntityStoreTests
    {
        [Fact]
        public async void ShouldSaveEventsForEntity()
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

            await entityStore.Save<EntityFake>(entity);

            db.Verify(m => m.Save(event1), Times.Once);
            db.Verify(m => m.Save(event2), Times.Once);
            db.Verify(m => m.Save(event3), Times.Once);
        }

        [Fact]
        public async void ShouldLoadEntityAndApplyStoredEvent()
        {
            var entity = new EntityFake
            {
                Id = Guid.NewGuid()
            };

            var event1 = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = entity.Id,
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = 1
            };

            var event2 = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = entity.Id,
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = 2
            };

            var event3 = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = entity.Id,
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = 3
            };

            var db = new InMemoryEventStore();
            var entityStore = new EntityStore(db);

            entity.Apply(event1);
            entity.Apply(event2);
            entity.Apply(event3);

            await entityStore.Save(entity);

            var loadedEntity = await entityStore.Load<EntityFake>(entity.Id);

            loadedEntity.Changes.Count.Should().Be(0);
            loadedEntity.Count.Should().Be(3);
        }
    }

    public class EntityFake : Entity
    {
        public Guid Id { get; set; }
        public int Count { get; private set; }

        public override void When(IEvent @event)
        {
            if (@event is FakeEvent)
            {
                Count++;
            }
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
