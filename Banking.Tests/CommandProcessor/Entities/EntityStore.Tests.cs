using Xunit;
using Moq;
using System;
using Banking.CommandProcessor.Entities;
using Banking.CommandProcessor.Events.EventStore;
using FluentAssertions;
using Banking.Tests.TestDoubles;

namespace Banking.Tests.CommandProcessor.Entities
{
    public class EntityStoreTests
    {
        [Fact]
        public async void ShouldSaveEventsForEntity()
        {
            var db = new Mock<IEventStore>();
            var entityStore = new EntityStore(db.Object);

            var entity = new AggregateRootFake
            {
                Id = Guid.NewGuid()
            };

            entity.Increment();
            entity.Increment();
            entity.Increment();

            await entityStore.Save<AggregateRootFake>(entity);

            db.Verify(m => m.Save(It.IsAny<FakeEvent>()), Times.Exactly(3));
        }

        [Fact]
        public async void ShouldLoadEntityAndApplyStoredEvent()
        {
            var db = new InMemoryEventStore();
            var entityStore = new EntityStore(db);

            var entity = new AggregateRootFake
            {
                Id = Guid.NewGuid()
            };

            entity.Increment();
            entity.Increment();
            entity.Increment();

            await entityStore.Save(entity);

            var loadedEntity = await entityStore.Load<AggregateRootFake>(entity.Id);

            loadedEntity.Changes.Count.Should().Be(0);
            loadedEntity.Count.Should().Be(3);
        }
    }
}
