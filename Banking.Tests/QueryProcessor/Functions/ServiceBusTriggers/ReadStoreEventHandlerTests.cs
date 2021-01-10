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
using Banking.Tests.TestDoubles;

namespace Banking.Tests.QueryProcessor.Functions.ServiceBusTriggers
{
    public class ReadStoreEventHandlerTests
    {
        private readonly MockBankingRepository mockRepositoryFacade;
        private readonly Mock<ILogger> mockLogger;
        private readonly ReadStoreEventHandler readerStoreEventHandler;

        public ReadStoreEventHandlerTests()
        {
            mockRepositoryFacade = new MockBankingRepository();
            mockLogger = new Mock<ILogger>();

            readerStoreEventHandler = new ReadStoreEventHandler(mockRepositoryFacade.MockRepositoryFacade.Object);
        }

        [Fact]
        public async void WhenABankAccountCreatedEventIsReceived_ThenANewBankAccountIsSaved()
        {
            var bankAccountId = new AccountId();
            var bankAccountCreatedEvent = new BankAccountCreatedEvent("John Doe", bankAccountId.Value, 1);

            var jsonData = JsonConvert.SerializeObject(bankAccountCreatedEvent);
            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockRepositoryFacade.MockRepositoryFacade.Verify(
                mock => mock.BankAccountRepository.Insert(
                        It.Is<Banking.QueryProcessor.Domain.BankAccount.BankAccount>(
                            bankAccount => 
                                bankAccount.Id == bankAccountId.Value.ToString() 
                                && bankAccount.AccountHolderName == "John Doe"
                    )
                )
            );
        }

        [Fact]
        public async void WhenAMakeDepositEventIsReceived_ThenADepositIsSaved()
        {
            var accountId = Guid.NewGuid();
            var mockBankAccount = new Banking.QueryProcessor.Domain.BankAccount.BankAccount
            {
                Id = accountId.ToString()
            };

            mockRepositoryFacade.MockBankAccountRepository
                .Setup(mock => mock.Get(mockBankAccount.Id))
                .Returns(Task.Run(() => mockBankAccount));

            var depositMadeEvent = new DepositMadeEvent(Guid.NewGuid(), "A Deposit", 123.45m, accountId, 1);

            var jsonData = JsonConvert.SerializeObject(depositMadeEvent);

            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockRepositoryFacade.MockRepositoryFacade.Verify(
                mock => mock.TransactionsRepository.Insert(It.Is<Transaction>(
                    transaction =>
                        transaction.BankAccount.Id == mockBankAccount.Id
                        && transaction.Description == depositMadeEvent.Description
                        && transaction.Id == depositMadeEvent.EntityId.ToString()
                        && transaction.Amount == depositMadeEvent.Amount
                        && transaction.Type == TransactionType.Deposit
                    )
                )
            );
        }

        [Fact]
        public async void WhenAMakeDepositEventIsReceived_ThenATheBankAccountBalanceIsUpdated()
        {
            var accountId = Guid.NewGuid();
            var mockBankAccount = new Banking.QueryProcessor.Domain.BankAccount.BankAccount
            {
                Id = accountId.ToString()
            };

            mockRepositoryFacade.MockBankAccountRepository
                .Setup(mock => mock.Get(mockBankAccount.Id))
                .Returns(Task.Run(() => mockBankAccount));

            var depositMadeEvent = new DepositMadeEvent(Guid.NewGuid(), "A Deposit", 123.45m, accountId, 1);

            var jsonData = JsonConvert.SerializeObject(depositMadeEvent);

            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockRepositoryFacade.MockRepositoryFacade.Verify(
                mock => mock.BankAccountRepository.Update(It.Is<Banking.QueryProcessor.Domain.BankAccount.BankAccount>(
                    bankAccount =>
                        bankAccount.Balance == 123.45m
                    )
                )
            );
        }
    }
}
