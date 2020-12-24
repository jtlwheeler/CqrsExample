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

            var entity = new EntityFake
            {
                Id = Guid.NewGuid()
            };

            entity.Increment();
            entity.Increment();
            entity.Increment();

            await entityStore.Save<EntityFake>(entity);

            db.Verify(m => m.Save(It.IsAny<FakeEvent>()), Times.Exactly(3));
        }

        [Fact]
        public async void ShouldLoadEntityAndApplyStoredEvent()
        {
            var db = new InMemoryEventStore();
            var entityStore = new EntityStore(db);

            var entity = new EntityFake
            {
                Id = Guid.NewGuid()
            };

            entity.Increment();
            entity.Increment();
            entity.Increment();

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

        public void Increment()
        {
            var @event = new FakeEvent
            {
                Id = Guid.NewGuid(),
                EntityId = Id,
                Timestamp = DateTime.UtcNow,
                Type = "FakeEvent",
                Version = NextEventVersionToAssign
            };

            Apply(@event);
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
