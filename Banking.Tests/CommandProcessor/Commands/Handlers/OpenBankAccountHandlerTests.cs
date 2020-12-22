using Xunit;
using Moq;
using FluentAssertions;
using System;
using Banking.CommandProcessor.Entities;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.CommandProcessor.Commands.Commands;

namespace Banking.Tests.CommandProcessor.Commands.Handlers
{
    public class OpenBankAccountHandlerTests
    {
        [Fact]
        public async void ShouldOpenBankAccountAndSaveEntity()
        {
            var entityStore = new Mock<IEntityStore>();
            var handler = new OpenBankAccountHandler(entityStore.Object);
            var command = new OpenBankAccountCommand
            {
                Name = "Bob Smith"
            };

            await handler.Handle(command);

            entityStore.Verify(m => m.Save(It.IsAny<BankAccount>()));
        }

        [Fact]
        public async void ShouldReturnAccountIdOnSuccess()
        {
            var entityStore = new Mock<IEntityStore>();
            var handler = new OpenBankAccountHandler(entityStore.Object);
            var command = new OpenBankAccountCommand
            {
                Name = "Jane Smith"
            };

            var result = await handler.Handle(command);

            result.Success.Should().Be(true);
            result.Value.GetType().Should().Be(typeof(Guid));
        }
    }
}