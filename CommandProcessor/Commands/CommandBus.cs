using System;
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
            if (command is CreateGreetingCommand)
            {
                createGreetingHandler.Handle((CreateGreetingCommand)command);
            }
            else
            {
                throw new UnknownCommandException("Unknown Command");
            }
        }
    }

    public class UnknownCommandException : Exception
    {
        public UnknownCommandException(string message): base(message)
        {
        }
    }
}