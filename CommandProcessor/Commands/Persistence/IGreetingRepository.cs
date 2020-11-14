using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Persistence
{
    public interface IGreetingRepository
    {
        public void Save(Greeting greeting);
    }
}