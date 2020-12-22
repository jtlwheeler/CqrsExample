using Xunit;
using FluentAssertions;
using System;
using Banking.CommandProcessor.Functions.DatabaseTriggers;
using Banking.Events;

namespace Banking.Tests.CommandProcessor.Functions.DatabaseTrigger
{
    public class EventDeserializerTests
    {
        [Fact]
        public void WhenEventIsABankAccountCreatedEvent_ThenJsonIsDeserializedSuccessfully()
        {
            var jsonEvent = "{"
                + "\"id\": \"00000000-0000-0000-0000-000000000001\","
                + "\"EntityId\": \"11111111-1111-1111-1111-111111111111\","
                + "\"Timestamp\": \"2020-01-01T01:02:59.457182Z\","
                + "\"Type\": \"BankAccountCreatedEvent\","
                + "\"Name\": \"John Doe\","
                + "\"Version\": 1"
                + "}";

            var bankAccountCreatedEvent = EventDeserializer.Deserialize<BankAccountCreatedEvent>(jsonEvent);

            bankAccountCreatedEvent.GetType().Should().Be(typeof(BankAccountCreatedEvent));

            bankAccountCreatedEvent.Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000001"));
            bankAccountCreatedEvent.EntityId.Should().Be(Guid.Parse("11111111-1111-1111-1111-111111111111"));
            bankAccountCreatedEvent.Timestamp.Should().Be(DateTime.Parse("2020-01-01T01:02:59.457182Z").ToUniversalTime());
            bankAccountCreatedEvent.Type.Should().Be("BankAccountCreatedEvent");
            bankAccountCreatedEvent.Name.Should().Be("John Doe");
            bankAccountCreatedEvent.Version.Should().Be(1);
        }
    }
}
