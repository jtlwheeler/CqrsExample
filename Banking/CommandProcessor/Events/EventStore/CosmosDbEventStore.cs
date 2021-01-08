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

        public async Task<List<Event>> GetEvents(Guid entityId)
        {
            var events = new List<Event>();
            using (var setIterator = container.GetItemLinqQueryable<Event>()
                .Where(@event => @event.EntityId == entityId)
                .OrderBy(@event => @event.Version)
                .ToStreamIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    using ResponseMessage response = await setIterator.ReadNextAsync();

                    response.EnsureSuccessStatusCode();

                    using StreamReader sr = new StreamReader(response.Content);
                    using JsonTextReader jtr = new JsonTextReader(sr);

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

            return events;
        }

        public async Task Save(Event @event)
        {
            switch (@event)
            {
                case BankAccountCreatedEvent bankAccountCreatedEvent:
                    await container.CreateItemAsync(bankAccountCreatedEvent, new PartitionKey(bankAccountCreatedEvent.AggregateRootId.ToString()));
                    break;
                case DepositMadeEvent depositMadeEvent:
                    await container.CreateItemAsync(depositMadeEvent, new PartitionKey(depositMadeEvent.AggregateRootId.ToString()));
                    break;
                default:
                    throw new UnprocessableEventException($"Unable to save event. {@event}");
            }
        }
    }
}
