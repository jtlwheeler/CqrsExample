using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Banking.CommandProcessor.Events;
using Banking.Events;

namespace Banking.CommandProcessor.Functions.DatabaseTriggers
{
    public class EventPublisher
    {
        private readonly IEventBus eventBus;
        private ILogger logger;

        public EventPublisher(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        [FunctionName("EventPublisher")]
        public void Run([CosmosDBTrigger(
            databaseName: "EventStore",
            collectionName: "Events",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents,
            ILogger log)
        {
            logger = log;
            logger.LogInformation($"Documents modified: {documents.Count}");

            foreach (var document in documents)
            {
                logger.LogInformation($"Processing document: {document}");

                var @event = EventConvert.Deserialize(document.ToString());

                eventBus.Publish(@event);
            }
        }
    }
}
