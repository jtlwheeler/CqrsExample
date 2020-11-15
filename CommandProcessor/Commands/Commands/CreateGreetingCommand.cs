using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Commands.Commands
{
    public class CreateGreetingCommand: ICommand
    {
        public Greeting Greeting { get; set; }
    }
}