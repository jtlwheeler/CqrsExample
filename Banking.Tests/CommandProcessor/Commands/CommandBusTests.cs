using Xunit;
using Moq;
using FluentAssertions;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.CommandProcessor.Commands.Commands;
using Banking.CommandProcessor.Commands;
using System;
using Banking.CommandProcessor.Entities;
using Banking.Result;

namespace Banking.Tests.CommandProcessor.Commands
{
    public class CommandBusTests
    {
        private readonly CommandBus commandBus;
        private readonly Mock<IOpenBankAccountHandler> mockOpenBankAccountHandler;
        private readonly Mock<IMakeDepositHandler> mockDepositHandler;


        public CommandBusTests()
        {
            mockOpenBankAccountHandler = new Mock<IOpenBankAccountHandler>();
            mockDepositHandler = new Mock<IMakeDepositHandler>();

            commandBus = new CommandBus(
                mockOpenBankAccountHandler.Object,
                mockDepositHandler.Object
            );
        }

        [Fact]
        public async void WhenAOpenAccountCommandIsSentOnTheCommandBus_ThenTheCorrectHandlerShouldBeCalled()
        {
            var command = new OpenBankAccountCommand
            {
                Name = "Jane Doe"
            };

            await commandBus.Handle(command);

            mockOpenBankAccountHandler.Verify(mock => mock.Handle(command));
        }

        [Fact]
        public void ShouldRaiseError_WhenAnUnknownCommandIsPassed()
        {
            var command = new UnknownCommand();

            commandBus.Invoking(bus => bus.Handle(command))
                .Should().Throw<UnknownCommandException>();
        }

        [Fact]
        public async void MakeDepsoitCommandIsSentOnTheCommandBus_ThenTheCorrectHandlerShouldBeCalled()
        {
            var depositId = new DepositId();
            mockDepositHandler
                .Setup(mock => mock.Handle(It.IsAny<MakeDepositCommand>()))
                .ReturnsAsync(Result<EntityId>.Ok(depositId));

            var command = new MakeDepositCommand(new AccountId(), 1.23m, "A Purchase");

            var result = await commandBus.Handle(command);

            mockDepositHandler.Verify(mock => mock.Handle(command));

            result.Success.Should().Be(true);
            result.Value.Should().Be(depositId);
        }

        class UnknownCommand : ICommand
        {
        }
    }
}