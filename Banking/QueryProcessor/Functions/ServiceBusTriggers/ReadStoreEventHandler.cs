using System;
using System.Threading.Tasks;
using Banking.Events;
using Banking.QueryProcessor.Domain.BankAccount;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Banking.QueryProcessor.Functions.SeviceBusTriggers
{
    public class ReadStoreEventHandler
    {
        private readonly IBankAccountRepository bankAccountRepository;
        private ILogger logger;

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
            logger = log;
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            log.LogInformation($"EnqueuedTimeUtc={enqueuedTimeUtc}");
            log.LogInformation($"DeliveryCount={deliveryCount}");
            log.LogInformation($"MessageId={messageId}");

            var @event = EventConvert.Deserialize(myQueueItem);

            switch (@event)
            {
                case BankAccountCreatedEvent bankAccountCreatedEvent:
                    log.LogInformation("Received BankAccountCreatedEvent event on message bus.");
                    await handleBankAccountCreatedEvent(bankAccountCreatedEvent);
                    break;
                case DepositMadeEvent depositMadeEvent:
                    log.LogInformation("Received DepositMadeEvent event on message bus.");
                    await handleDepositMadeEvent(depositMadeEvent);
                    break;
                default:
                    log.LogInformation("Received unknown event type.");
                    break;
            }
        }

        private async Task handleBankAccountCreatedEvent(BankAccountCreatedEvent @event)
        {
            var newBankAccount = new BankAccount
            {
                Id = @event.EntityId.ToString(),
                AccountHolderName = @event.Name,
                Balance = 0.0m
            };

            await bankAccountRepository.Save(newBankAccount);
        }

        private async Task handleDepositMadeEvent(DepositMadeEvent @event)
        {
            var bankAccountId = @event.AggregateRootId;

            var bankAccount = await bankAccountRepository.Get(bankAccountId.ToString());

            var deposit = new Transaction
            {
                Id = @event.EntityId.ToString(),
                Description = @event.Description,
                Amount = @event.Amount,
                Type = TransactionType.Deposit
            };

            bankAccount.Transactions.Add(deposit);

            await bankAccountRepository.Update(bankAccount);
        }
    }
}
