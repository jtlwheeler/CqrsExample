using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Handlers;

namespace CommandProcessor.Commands
{
    public class CommandBus : ICommandBus
    {
        private ICreateGreetingHandler createGreetingHandler;

        public CommandBus(ICreateGreetingHandler createGreetingHandler)
        {
            this.createGreetingHandler = createGreetingHandler;
        }

        public void Handle(ICommand command)
        {
            createGreetingHandler.Handle((CreateGreetingCommand)command);
        }
    }
}