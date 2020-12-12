using CommandProcessor.Commands.Entities;
using Xunit;
using FluentAssertions;
using System;
using CommandProcessor.Events.Events;

namespace CommandProcessor.Tests.Entities
{
    public class BankAccountTests
    {
        [Fact]
        public void WhenABankAccountIsOpened_ThenTheAccountHolderNameIsSet()
        {
            var account = new BankAccount();

            account.Open("John Doe");

            account.Name.Should().Be("John Doe");
            account.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void WhenABankAccountIsOpened_ThenAnEventIsCreated()
        {
            var account = new BankAccount();

            account.Open("John Doe");

            account.Changes.Count.Should().Be(1);

            var accountOpenedEvent = (BankAccountCreatedEvent)account.Changes[0];
            accountOpenedEvent.Id.Should().NotBeEmpty();
            accountOpenedEvent.Timestamp.Should().BeCloseTo(DateTime.UtcNow);
            accountOpenedEvent.EntityId.Should().Be(account.Id);
            accountOpenedEvent.Type.Should().Be("BankAccountCreatedEvent");
            accountOpenedEvent.Name.Should().Be("John Doe");
        }

        [Fact]
        public void WhenABankAccountIsOpened_ThenAnEventIsCreated_AndTheVersionIsSet()
        {
            var account = new BankAccount();

            account.Open("John Doe");

            var accountOpenedEvent = (BankAccountCreatedEvent)account.Changes[0];
            accountOpenedEvent.Version.Should().Be(1);
        }
    }
}