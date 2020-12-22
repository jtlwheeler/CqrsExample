using Xunit;
using Moq;
using FluentAssertions;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.CommandProcessor.Commands.Commands;
using Banking.CommandProcessor.Commands;

namespace Banking.Tests.CommandProcessor.Commands
{
    public class CommandBusTests
    {
        [Fact]
        public async void WhenAOpenAccountCommandIsSentOnTheCommandBus_ThenTheCorrectHandlerShouldBeCalled()
        {
            var mockOpenBankAccountHandler = new Mock<IOpenBankAccountHandler>();

            var command = new OpenBankAccountCommand
            {
                Name = "Jane Doe"
            };

            var commandBus = new CommandBus(mockOpenBankAccountHandler.Object);

            await commandBus.Handle(command);

            mockOpenBankAccountHandler.Verify(mock => mock.Handle(command));
        }

        [Fact]
        public void ShouldRaiseError_WhenAnUnknownCommandIsPassed()
        {
            var mockOpenBankAccountHandler = new Mock<IOpenBankAccountHandler>();
            var commandBus = new CommandBus(mockOpenBankAccountHandler.Object);

            var command = new UnknownCommand();

            commandBus.Invoking(bus => bus.Handle(command))
                .Should().Throw<UnknownCommandException>();
        }

        class UnknownCommand : ICommand
        {
        }
    }
}