using Xunit;
using FluentAssertions;
using System;
using Banking.CommandProcessor.Entities;
using Banking.Events;

namespace Banking.Tests.CommandProcessor.Entities
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

        [Fact]
        public void WhenADepositIsMade_ThenTheBalanceIncreases()
        {
            var account = new BankAccount();

            account.Open("Jane Doe");

            account.Balance.Should().Be(0.0m);

            account.MakeDeposit("ADescriptionOfTheDeposit", 123.45m);
            account.Balance.Should().Be(123.45m);
        }

        [Fact]
        public void WhenMultipleDepositsAreMade_ThenTheBalanceIsAddedCorrectly()
        {
            var account = new BankAccount();

            account.Open("Jane Doe");

            account.Balance.Should().Be(0.0m);

            account.MakeDeposit("First Deposit", 10.25m);
            account.MakeDeposit("Second Deposit", 1.25m);
            account.Balance.Should().Be(11.50m);
        }

        [Fact]
        public void WhenADepositIsMade_ThenADepositEventIsCreated()
        {
            var account = new BankAccount();

            account.Open("Jane Doe");

            account.MakeDeposit("ADescriptionOfTheDeposit", 123.45m);

            var depositMadeEvent = (DepositMadeEvent)account.Changes[1];

            depositMadeEvent.Amount.Should().Be(123.45m);
            depositMadeEvent.Description.Should().Be("ADescriptionOfTheDeposit");
        }

        [Fact]
        public void WhenEventsAreCreated_ThenTheVersionNumberShouldBeIncremented()
        {
            var account = new BankAccount();

            account.Open("Jane Doe");
            account.MakeDeposit("First Deposit", 1m);
            account.MakeDeposit("Second Deposit", 2m);
            account.MakeDeposit("Thrird Deposit", 3m);

            account.Changes.Count.Should().Be(4);

            var firstEvent = account.Changes[0];
            firstEvent.Version.Should().Be(1);

            var secondEvent = account.Changes[1];
            secondEvent.Version.Should().Be(2);

            var thirdEvent = account.Changes[2];
            thirdEvent.Version.Should().Be(3);

            var fourthEvent = account.Changes[3];
            fourthEvent.Version.Should().Be(4);
        }

        [Fact]
        public void WhenABankAccountIsAttemptedToBeOpenedForASecondTime_NothingHappens()
        {
            var account = new BankAccount();

            account.Open("Jane Doe");

            var accountId = account.Id;
            account.Changes.Count.Should().Be(1);

            account.Open("Jane Doe");

            account.Changes.Count.Should().Be(1);
            account.Id.Should().Be(accountId);
        }
    }
}
