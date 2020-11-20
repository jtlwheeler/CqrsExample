using System.Collections.Generic;
using CommandProcessor.Commands.Entities;
using CommandProcessor.Events;
using CommandProcessor.Events.Events;

namespace CommandProcessor.Persistence
{
    public class InMemoryGreetingRepository : IGreetingRepository
    {
        private List<Greeting> greetings;
        private IEventBus eventBus;

        public InMemoryGreetingRepository(IEventBus eventBus)
        {
            greetings = new List<Greeting>();
            this.eventBus = eventBus;
        }

        public void Save(Greeting greeting)
        {
            greetings.Add(greeting);

            var greetingCreatedEvent = new GreetingCreatedEvent(greeting.Message);
            eventBus.Publish(greetingCreatedEvent);
        }
    }
}