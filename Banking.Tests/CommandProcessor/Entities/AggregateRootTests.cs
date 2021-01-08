using System;
using Banking.CommandProcessor.Entities;
using Banking.Tests.TestDoubles;
using FluentAssertions;
using Xunit;

namespace Banking.Tests.CommandProcessor.Entities
{
    public class AggregateRootTests
    {
        [Fact]
        public void WhenAnEntityIsCreated_ThenTheFirstEventVersionToAssignedShouldBeOne()
        {
            var entity = new AggregateRootFake
            {
                Id = new FakeId()
            };

            entity.Increment();

            entity.Changes.Count.Should().Be(1);

            entity.Changes[0].Version.Should().Be(1);
        }

        [Fact]
        public void WhenAnEventsAreReplayed_ThenTheNextEventVersionShouldBeInitializedProperly()
        {
            var entity = new AggregateRootFake
            {
                Id = new FakeId()
            };

            var event1 = new FakeEvent(
                Guid.NewGuid(),
                entity.Id.Value,
                entity.Id.Value,
                DateTime.UtcNow,
                1
            );

            var event2 = new FakeEvent(
                Guid.NewGuid(),
                entity.Id.Value,
                entity.Id.Value,
                DateTime.UtcNow,
                2
            );

            entity.Replay(event1);
            entity.Replay(event2);

            entity.Increment();

            entity.Changes[0].Version.Should().Be(3);
        }

        [Fact]
        public void WhenAnEventsAreReplayed_AnErrorShouldBeThrownIfTheirIsAVersionMismatch()
        {
            var entity = new AggregateRootFake
            {
                Id = new FakeId()
            };

            var event1 = new FakeEvent(
                Guid.NewGuid(),
                entity.Id.Value,
                entity.Id.Value,
                DateTime.UtcNow,
                1
            );

            var event2 = new FakeEvent(
                Guid.NewGuid(),
                entity.Id.Value,
                entity.Id.Value,
                DateTime.UtcNow,
                100
            );

            entity.Replay(event1);
            entity.Invoking(e => e.Replay(event2))
                .Should().Throw<EntityException>()
                .WithMessage("Unexpected event version. Expected version '100' to be '2'");
        }
    }
}
