using System;
using Newtonsoft.Json;

namespace Banking.Events
{
    public abstract class Event
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; protected set; }
        [JsonProperty(PropertyName = "EntityId")]
        public Guid EntityId { get; protected set; }
        [JsonProperty(PropertyName = "AggregateRootId")]
        public Guid AggregateRootId { get; protected set; }
        [JsonProperty(PropertyName = "Timestamp")]
        public DateTime Timestamp { get; protected set; }
        public string Type { get; protected set; }
        public int Version { get; protected set; }
    }
}