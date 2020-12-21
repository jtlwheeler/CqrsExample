using Xunit;
using Moq;
using CommandProcessor.Domain.BankAccount;
using CommandProcessor.Functions.SeviceBusTrigger;
using System;
using Microsoft.Extensions.Logging;
using CommandProcessor.Events.Events;
using Newtonsoft.Json;

namespace CommandProcessor.Tests.Functions.ServiceBusTrigger
{
    public class ReadStoreEventHandlerTests
    {
        [Fact]
        public async void WhenABankAccountCreatedEventIsReceived_ThenANewBankAccountIsSaved()
        {
            var mockBankAccountRepository = new Mock<IBankAccountRepository>();
            var mockLogger = new Mock<ILogger>();
            var readerStoreEventHandler = new ReadStoreEventHandler(mockBankAccountRepository.Object);

            var bankAccountId = Guid.NewGuid();
            var bankAccountCreatedEvent = new BankAccountCreatedEvent("John Doe", bankAccountId, 1);

            var jsonData = JsonConvert.SerializeObject(bankAccountCreatedEvent);
            await readerStoreEventHandler.Run(jsonData, 1, DateTime.UtcNow, "1", mockLogger.Object);

            mockBankAccountRepository.Verify(
                m => m.Save(It.Is<BankAccount>(p => p.Id == bankAccountId.ToString() && p.AccountHolderName == "John Doe"))
            );
        }
    }
}
