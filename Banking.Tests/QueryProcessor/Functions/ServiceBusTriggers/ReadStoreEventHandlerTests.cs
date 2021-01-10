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
using Banking.QueryProcessor.Repository;

namespace Banking.Tests.QueryProcessor.Functions.ServiceBusTriggers
{
    public class ReadStoreEventHandlerTests
    {
        private readonly Mock<IRepositoryFacade> mockRepositoryFacade;
        private readonly Mock<IRepository<Transaction>> mockTransactionRepository;
        private readonly Mock<IRepository<Banking.QueryProcessor.Domain.BankAccount.BankAccount>> mockBankAccountRepository;
        private readonly Mock<ILogger> mockLogger;
        public ReadStoreEventHandlerTests()
        {
            mockRepositoryFacade = new Mock<IRepositoryFacade>();
            mockLogger = new Mock<ILogger>();

            mockTransactionRepository = new Mock<IRepository<Transaction>>();
            mockRepositoryFacade
                .Setup(mock => mock.TransactionsRepository)
                .Returns(mockTransactionRepository.Object);

            mockBankAccountRepository = new Mock<IRepository<Banking.QueryProcessor.Domain.BankAccount.BankAccount>>();
            mockRepositoryFacade
                .Setup(mock => mock.BankAccountRepository)
                .Returns(mockBankAccountRepository.Object);
        }

        [Fact]
        public async void WhenABankAccountCreatedEventIsReceived_ThenANewBankAccountIsSaved()
        {
            var mockBankAccountRepository = new Mock<IRepository<Banking.QueryProcessor.Domain.BankAccount.BankAccount>>();

            mockRepositoryFacade
                .Setup(mock => mock.BankAccountRepository)
                .Returns(mockBankAccountRepository.Object);

            var readerStoreEventHandler = new ReadStoreEventHandler(mockRepositoryFacade.Object);

            var bankAccountId = new AccountId();
            var bankAccountCreatedEvent = new BankAccountCreatedEvent("John Doe", bankAccountId.Value, 1);

            var jsonData = JsonConvert.SerializeObject(bankAccountCreatedEvent);
            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockRepositoryFacade.Verify(
                m => m.BankAccountRepository.Insert(It.Is<Banking.QueryProcessor.Domain.BankAccount.BankAccount>(p => p.Id == bankAccountId.Value.ToString() && p.AccountHolderName == "John Doe"))
            );
        }

        [Fact]
        public async void WhenAMakeDepositEventIsReceived_ThenADepositIsSaved()
        {
            var readerStoreEventHandler = new ReadStoreEventHandler(mockRepositoryFacade.Object);

            var accountId = Guid.NewGuid();
            var mockBankAccount = new Banking.QueryProcessor.Domain.BankAccount.BankAccount
            {
                Id = accountId.ToString()
            };

            mockBankAccountRepository
                .Setup(mock => mock.Get(mockBankAccount.Id))
                .Returns(Task.Run(() => mockBankAccount));

            var depositMadeEvent = new DepositMadeEvent(Guid.NewGuid(), "A Deposit", 123.45m, accountId, 1);

            var jsonData = JsonConvert.SerializeObject(depositMadeEvent);

            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockRepositoryFacade.Verify(
                m => m.TransactionsRepository.Insert(It.Is<Transaction>(
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
            var readerStoreEventHandler = new ReadStoreEventHandler(mockRepositoryFacade.Object);

            var accountId = Guid.NewGuid();
            var mockBankAccount = new Banking.QueryProcessor.Domain.BankAccount.BankAccount
            {
                Id = accountId.ToString()
            };

            mockBankAccountRepository
                .Setup(mock => mock.Get(mockBankAccount.Id))
                .Returns(Task.Run(() => mockBankAccount));

            var depositMadeEvent = new DepositMadeEvent(Guid.NewGuid(), "A Deposit", 123.45m, accountId, 1);

            var jsonData = JsonConvert.SerializeObject(depositMadeEvent);

            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockRepositoryFacade.Verify(
                m => m.BankAccountRepository.Update(It.Is<Banking.QueryProcessor.Domain.BankAccount.BankAccount>(
                    bankAccount =>
                        bankAccount.Balance == 123.45m
                    )
                )
            );
        }
    }
}
