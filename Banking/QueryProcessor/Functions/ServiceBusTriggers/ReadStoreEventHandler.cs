using System;
using System.Threading.Tasks;
using Banking.Events;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Banking.QueryProcessor.Functions.SeviceBusTriggers
{
    public class ReadStoreEventHandler
    {
        private readonly IRepositoryFacade repositoryFacade;
        private ILogger logger;

        public ReadStoreEventHandler(IRepositoryFacade repositoryFacade)
        {
            this.repositoryFacade = repositoryFacade;
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
                    await HandleBankAccountCreatedEvent(bankAccountCreatedEvent);
                    break;
                case DepositMadeEvent depositMadeEvent:
                    log.LogInformation("Received DepositMadeEvent event on message bus.");
                    await HandleDepositMadeEvent(depositMadeEvent);
                    break;
                default:
                    log.LogInformation("Received unknown event type.");
                    break;
            }
        }

        private async Task HandleBankAccountCreatedEvent(BankAccountCreatedEvent @event)
        {
            var newBankAccount = new BankAccount
            {
                Id = @event.EntityId.ToString(),
                AccountHolderName = @event.Name,
                Balance = 0.0m
            };

            repositoryFacade.BankAccountRepository.Insert(newBankAccount);
            await repositoryFacade.Save();
        }

        private async Task HandleDepositMadeEvent(DepositMadeEvent @event)
        {
            var bankAccountId = @event.AggregateRootId;

            var bankAccount = await repositoryFacade.BankAccountRepository.Get(bankAccountId.ToString());

            var deposit = new Transaction
            {
                Id = @event.EntityId.ToString(),
                Description = @event.Description,
                Amount = @event.Amount,
                Type = TransactionType.Deposit,
            };

            bankAccount.Transactions.Add(deposit);

            repositoryFacade.BankAccountRepository.Update(bankAccount);
            await repositoryFacade.Save();
        }
    }
}
