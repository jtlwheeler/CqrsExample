using CommandProcessor.Commands.Commands;

namespace CommandProcessor.Commands.Handlers
{
    public interface ICreateGreetingHandler
    {
        public void Handle(CreateGreetingCommand command);
    }
}
