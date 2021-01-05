using Xunit;
using Moq;
using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Functions.SeviceBusTriggers;
using Banking.Events;
using Banking.CommandProcessor.Entities;

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
    }
}
