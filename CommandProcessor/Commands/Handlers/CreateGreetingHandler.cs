using CommandProcessor.Commands.Commands;
using CommandProcessor.Persistence;

namespace CommandProcessor.Commands.Handlers
{
    public class CreateGreetingHandler : ICreateGreetingHandler
    {
        private readonly IGreetingRepository greetingRepository;

        public CreateGreetingHandler(IGreetingRepository greetingRepository)
        {
            this.greetingRepository = greetingRepository;
        }

        public void Handle(CreateGreetingCommand command)
        {
            greetingRepository.Save(command.Greeting);
        }
    }

}