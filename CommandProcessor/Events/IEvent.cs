using System;
using Newtonsoft.Json;

namespace CommandProcessor.Events
{
    public interface IEvent
    {
        [JsonProperty(PropertyName = "id")]
        Guid Id { get; }
        Guid EntityId { get; }
        DateTime Timestamp { get; }
        string Type { get; }
        int Version { get; }
    }
}