using System.Collections.Generic;
using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Persistence
{
    public class InMemoryGreetingRepository : IGreetingRepository
    {
        private List<Greeting> list;

        public InMemoryGreetingRepository()
        {
            list = new List<Greeting>();
        }

        public void Save(Greeting greeting)
        {
            list.Add(greeting);
        }
    }
}