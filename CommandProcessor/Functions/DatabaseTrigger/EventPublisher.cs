using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

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
                log.LogInformation($"Document Id: {document.Id}");
            }
        }
    }
}
