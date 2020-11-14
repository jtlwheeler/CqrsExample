using Xunit;
using Moq;
using CommandProcessor.Persistence;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Tests.Commands.Handlers
{
    public class CreateGreetingHandlerTest
    {
        [Fact]
        public void ShouldSaveGreeting()
        {
            var mock = new Mock<IGreetingRepository>();
            var createGreetingHandler = new CreateGreetingHandler(mock.Object);

            var command = new CreateGreetingCommand
            {
                Greeting = new Greeting { Message = "Hello, World!"}
            };
            
            createGreetingHandler.Handle(command);

            mock.Verify(mocker => mocker.Save(command.Greeting));
        }
    }
}