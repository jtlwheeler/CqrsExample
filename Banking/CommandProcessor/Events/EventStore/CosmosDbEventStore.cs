using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Banking.Events;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Banking.CommandProcessor.Events.EventStore
{
    public class CosmosDbEventStore : IEventStore
    {
        private readonly Container container;
        public CosmosDbEventStore(Container container)
        {
            this.container = container;
        }

        public async Task<List<IEvent>> GetEvents(Guid entityId)
        {
            var events = new List<IEvent>();
            using (var setIterator = container.GetItemLinqQueryable<IEvent>()
                .Where(@event => @event.EntityId == entityId)
                .OrderBy(@event => @event.Version)
                .ToStreamIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    using (ResponseMessage response = await setIterator.ReadNextAsync())
                    {
                        response.EnsureSuccessStatusCode();
                        using (StreamReader sr = new StreamReader(response.Content))
                        using (JsonTextReader jtr = new JsonTextReader(sr))
                        {
                            var jsonSerializer = new JsonSerializer();
                            var json = jsonSerializer.Deserialize<JObject>(jtr);
                            var array = (JArray)json.GetValue("Documents");

                            foreach (var item in array)
                            {
                                var @event = EventConvert.Deserialize(item.ToString());
                                events.Add(@event);
                            }
                        }
                    }
                }
            }

            return events;
        }

        public async Task Save(IEvent @event)
        {
            if (@event is BankAccountCreatedEvent)
            {
                await container.CreateItemAsync<BankAccountCreatedEvent>((BankAccountCreatedEvent)@event, new PartitionKey(@event.EntityId.ToString()));
            }
        }
    }
}
