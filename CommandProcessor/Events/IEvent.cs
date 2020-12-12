using System;

namespace CommandProcessor.Events
{
    public interface IEvent
    {
        Guid Id { get; }
        Guid EntityId { get; }
        DateTime Timestamp { get; }
        string Type { get; }
        int Version { get; }
    }
}