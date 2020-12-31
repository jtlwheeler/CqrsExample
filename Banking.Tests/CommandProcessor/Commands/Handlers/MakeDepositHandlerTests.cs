using Banking.CommandProcessor.Commands.Commands;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.CommandProcessor.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Banking.Tests.CommandProcessor.Commands.Handlers
{
    public class MakeDepositHandlerTests
    {
        [Fact]
        public async void ShoulMakeDeposit()
        {
            var entityStore = new Mock<IEntityStore>();

            var bankAccount = new BankAccount();
            bankAccount.Open("John Doe");

            var accountId = bankAccount.Id;
            var command = new MakeDepositCommand(accountId, 1.0m, "Bob's Auto Repair");

            entityStore
                .Setup(mock => mock.Load<BankAccount>(accountId))
                .ReturnsAsync(bankAccount);

            entityStore
                .Setup(mock => mock.Save(It.IsAny<BankAccount>()));

            var makeDepositHandler = new MakeDepositHandler(entityStore.Object);
            var commandResult = await makeDepositHandler.Handle(command);

            commandResult.Success.Should().Be(true);

            var depositId = commandResult.Value;
            depositId.Should().NotBe(default);
            depositId.Should().NotBe(accountId);

            entityStore.Verify(m => m.Save(It.IsAny<BankAccount>()));
            entityStore.Verify(m => m.Load<BankAccount>(accountId));
            entityStore.VerifyNoOtherCalls();
        }
    }
}
