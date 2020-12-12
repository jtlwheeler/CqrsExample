using Xunit;
using Moq;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands;
using FluentAssertions;

namespace CommandProcessor.Tests.Commands.Handlers
{
    public class CommandBusTests
    {
        [Fact]
        public void WhenAOpenAccountCommandIsSentOnTheCommandBus_ThenTheCorrectHandlerShouldBeCalled()
        {
            var mockOpenBankAccountHandler = new Mock<IOpenBankAccountHandler>();

            var command = new OpenBankAccountCommand
            {
                Name = "Jane Doe"
            };

            var commandBus = new CommandBus(mockOpenBankAccountHandler.Object);

            commandBus.Handle(command);

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