using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using CommandProcessor.Events.Events;

namespace CommandProcessor.Functions.DatabaseTrigger
{
    public class EventPublisher
    {
        public EventPublisher()
        {
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
            log.LogInformation($"Documents modified: {documents.Count}");
            foreach (var document in documents)
            {
                log.LogInformation($"Document Id: {document.ToString()}");

                if (document.GetPropertyValue<string>("Type") == "BankAccountCreatedEvent")
                {
                    log.LogInformation($"Event {document.Id} is a BankAccountCreatedEvent");
                    var bankAccountCreatedEvent = EventDeserializer.Deserialize<BankAccountCreatedEvent>(document.ToString());
                    log.LogInformation($"Successfully serialized BankAccountCreatedEvent. Hello, {bankAccountCreatedEvent.Name}!");
                }
            }
        }
    }
}
