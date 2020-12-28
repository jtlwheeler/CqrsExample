using System;
using Banking.Tests.TestDoubles;
using FluentAssertions;
using Xunit;

namespace Banking.Tests.CommandProcessor.Entities
{
    public class EntityTests
    {
        [Fact]
        public void WhenAnEntityIsCreated_ThenTheFirstEventVersionToAssignedShouldBeOne()
        {
            var entity = new EntityFake();

            entity.Increment();

            entity.Changes.Count.Should().Be(1);

            entity.Changes[0].Version.Should().Be(1);
        }

        [Fact]
        public void WhenAnEntityIsLoadedWithEvents_ThenTheNextEventVersionShouldBeInitializedProperly()
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

            entity.Load(event1);
            entity.Load(event2);

            entity.Increment();

            entity.Changes[0].Version.Should().Be(3);
        }
    }
}
