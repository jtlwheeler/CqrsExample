using CommandProcessor.Commands.Entities;
using Xunit;
using FluentAssertions;

namespace CommandProcessor.Tests.Entity
{
    public class BankAccountTests
    {
        [Fact]
        public void WhenABankAccountIsOpeed_ThenTheAccountHolderNameIsSet()
        {
            var account = new BankAccount();

            account.Open("John Doe");

            account.Name.Should().Be("John Doe");
            account.Id.Should().NotBeEmpty();
        }
    }
}