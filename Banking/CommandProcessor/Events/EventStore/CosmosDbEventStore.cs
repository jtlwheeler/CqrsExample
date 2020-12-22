using System.Threading.Tasks;
using Banking.Events;
using Microsoft.Azure.Cosmos;

namespace Banking.CommandProcessor.Events.EventStore
{
    public class CosmosDbEventStore : IEventStore
    {
        private readonly Container container;
        public CosmosDbEventStore(Container container)
        {
            this.container = container;
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