using System;
using System.Threading.Tasks;
using Banking.Events;
using Banking.QueryProcessor.Domain.BankAccount;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Banking.QueryProcessor.Functions.SeviceBusTriggers
{
    public class ReadStoreEventHandler
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public ReadStoreEventHandler(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        [FunctionName("ReadStoreEventHandler")]
        public async Task Run(
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

            var jsonObject = JsonConvert.DeserializeObject<JObject>(myQueueItem);
            var eventType = jsonObject.GetValue("Type").ToString();
            if (eventType == BankAccountCreatedEvent.EventTypeName)
            {
                log.LogInformation("Received BankAccountCreatedEvent event on message bus.");
                var bankAccountCreatedEvent = EventConvert.Deserialize<BankAccountCreatedEvent>(myQueueItem);

                var newBankAccount = new BankAccount
                {
                    Id = bankAccountCreatedEvent.EntityId.ToString(),
                    AccountHolderName = bankAccountCreatedEvent.Name
                };

                await bankAccountRepository.Save(newBankAccount);
            }
        }
    }
}
