using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Commands.Commands
{
    public class CreateGreetingCommand
    {
        public Greeting Greeting { get; set; }
    }
}