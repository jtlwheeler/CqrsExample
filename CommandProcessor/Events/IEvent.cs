using System;

namespace CommandProcessor.Events
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime Timestamp { get; }
        string Name { get; }
    }
}