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
        private readonly Mock<ILogger> mockLogger;
        public ReadStoreEventHandlerTests()
        {
            mockRepositoryFacade = new Mock<IRepositoryFacade>();
            mockLogger = new Mock<ILogger>();
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

            var mockBankAccountRepository = new Mock<IRepository<Banking.QueryProcessor.Domain.BankAccount.BankAccount>>();
            mockBankAccountRepository
                .Setup(mock => mock.Get(mockBankAccount.Id))
                .Returns(Task.Run(() => mockBankAccount));

            mockRepositoryFacade
                .Setup(mock => mock.BankAccountRepository)
                .Returns(mockBankAccountRepository.Object);

            var depositMadeEvent = new DepositMadeEvent(Guid.NewGuid(), "A Deposit", 123.45m, accountId, 1);

            var jsonData = JsonConvert.SerializeObject(depositMadeEvent);

            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockRepositoryFacade.Verify(
                m => m.BankAccountRepository.Update(It.Is<Banking.QueryProcessor.Domain.BankAccount.BankAccount>(
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
