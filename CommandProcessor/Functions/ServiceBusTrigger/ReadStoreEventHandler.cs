using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CommandProcessor.Functions.SeviceBusTrigger
{
    public class ReadStoreEventHandler
    {
        [FunctionName("ReadStoreEventHandler")]                    
        public static void Run(
            [ServiceBusTrigger("eventstream", Connection = "ServiceBusConnection")] 
            string myQueueItem,
            Int32 deliveryCount,
            DateTime enqueuedTimeUtc,
            string messageId,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            log.LogInformation($"EnqueuedTimeUtc={enqueuedTimeUtc}");
            log.LogInformation($"DeliveryCount={deliveryCount}");
            log.LogInformation($"MessageId={messageId}");
        }
    }
}
