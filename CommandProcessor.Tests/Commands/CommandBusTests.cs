using Xunit;
using Moq;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Entities;
using CommandProcessor.Commands;
using System;
using FluentAssertions;

namespace CommandProcessor.Tests.Commands.Handlers
{
    public class CommandBusTests
    {
        [Fact]
        public void WhenACreateGreetingCommandIsPassed_thenTheCorrectHandlerShouldBeCalled()
        {
            var mockCreateGreetingHandler = new Mock<ICreateGreetingHandler>();

            var command = new CreateGreetingCommand
            {
                Greeting = new Greeting
                {
                    Message = "Hello. This is a message"
                }
            };

            var commandBus = new CommandBus(mockCreateGreetingHandler.Object);

            commandBus.Handle(command);

            mockCreateGreetingHandler.Verify(mock => mock.Handle(command));
        }

        [Fact]
        public void ShouldRaiseError_WhenAnUnknownCommandIsPassed()
        {
            var mockCreateGreetingHandler = new Mock<ICreateGreetingHandler>();
            var commandBus = new CommandBus(mockCreateGreetingHandler.Object);

            var command = new UnknownCommand();

            commandBus.Invoking(bus => bus.Handle(command))
                .Should().Throw<UnknownCommandException>();
        }

        class UnknownCommand : ICommand
        {
        }
    }
}