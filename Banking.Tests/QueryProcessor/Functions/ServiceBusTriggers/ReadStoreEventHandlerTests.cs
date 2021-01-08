using Xunit;
using Moq;
using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Functions.SeviceBusTriggers;
using Banking.Events;
using Banking.CommandProcessor.Entities;
using System.Threading.Tasks;

namespace Banking.Tests.QueryProcessor.Functions.ServiceBusTriggers
{
    public class ReadStoreEventHandlerTests
    {
        [Fact]
        public async void WhenABankAccountCreatedEventIsReceived_ThenANewBankAccountIsSaved()
        {
            var mockBankAccountRepository = new Mock<IBankAccountRepository>();
            var mockLogger = new Mock<ILogger>();
            var readerStoreEventHandler = new ReadStoreEventHandler(mockBankAccountRepository.Object);

            var bankAccountId = new AccountId();
            var bankAccountCreatedEvent = new BankAccountCreatedEvent("John Doe", bankAccountId.Value, 1);

            var jsonData = JsonConvert.SerializeObject(bankAccountCreatedEvent);
            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockBankAccountRepository.Verify(
                m => m.Save(It.Is<Banking.QueryProcessor.Domain.BankAccount.BankAccount>(p => p.Id == bankAccountId.Value.ToString() && p.AccountHolderName == "John Doe"))
            );
        }

        [Fact]
        public async void WhenAMakeDepositEventIsReceived_ThenADepositIsSaved()
        {
            var mockLogger = new Mock<ILogger>();
            var mockBankAccountRepository = new Mock<IBankAccountRepository>();
            var readerStoreEventHandler = new ReadStoreEventHandler(mockBankAccountRepository.Object);

            var accountId = Guid.NewGuid();
            var mockBankAccount = new Banking.QueryProcessor.Domain.BankAccount.BankAccount
            {
                Id = accountId.ToString()
            };
            mockBankAccountRepository
                .Setup(a => a.Get(mockBankAccount.Id))
                .Returns(Task.Run(() => mockBankAccount));

            var depositMadeEvent = new DepositMadeEvent(Guid.NewGuid(), "A Deposit", 123.45m, accountId, 1);

            var jsonData = JsonConvert.SerializeObject(depositMadeEvent);

            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockBankAccountRepository.Verify(
                m => m.Update(It.Is<Banking.QueryProcessor.Domain.BankAccount.BankAccount>(
                    bankAccount =>
                        bankAccount.Id == mockBankAccount.Id
                        && bankAccount.Transactions[0].Description == depositMadeEvent.Description
                        && bankAccount.Transactions[0].Id == depositMadeEvent.EntityId.ToString()
                        && bankAccount.Transactions[0].Amount == depositMadeEvent.Amount
                        && bankAccount.Transactions[0].Type == TransactionType.Deposit
                    )
                )
            );
        }
    }
}
