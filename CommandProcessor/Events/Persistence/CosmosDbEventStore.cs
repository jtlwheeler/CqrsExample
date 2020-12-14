using System;
using System.Threading.Tasks;
using CommandProcessor.Events.Events;
using Microsoft.Azure.Cosmos;

namespace CommandProcessor.Events.Persistence
{
    public class CosmosDbEventStore : IEventStore
    {
        private readonly CosmosClient client;
        private string databaseId = "EventStore";
        private string containerId = "Events";
        public CosmosDbEventStore(CosmosClient client)
        {
            this.client = client;
        }

        public async Task Save(IEvent @event)
        {
            Database database = await this.client.CreateDatabaseIfNotExistsAsync(databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/id");

            if (@event is BankAccountCreatedEvent)
            {
                await container.CreateItemAsync<BankAccountCreatedEvent>((BankAccountCreatedEvent)@event, new PartitionKey(@event.Id.ToString()));
            }
        }
    }
}
