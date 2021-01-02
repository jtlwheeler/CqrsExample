using System;
using Newtonsoft.Json;

namespace Banking.Events
{
    public interface IEvent
    {
        [JsonProperty(PropertyName = "id")]
        Guid Id { get; }
        [JsonProperty(PropertyName = "EntityId")]
        Guid EntityId { get; }
        [JsonProperty(PropertyName = "AggregateRootId")]
        Guid AggregateRootId { get; }
        [JsonProperty(PropertyName = "Timestamp")]
        DateTime Timestamp { get; }
        string Type { get; }
        int Version { get; }
    }
}