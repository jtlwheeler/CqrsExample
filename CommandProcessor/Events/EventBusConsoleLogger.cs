using System;

namespace CommandProcessor.Events
{
    public class EventBusConsoleLogger : IEventBus
    {
        public void Publish(IEvent @event)
        {
            Console.WriteLine($"Publishing event {@event}");
        }
    }
}