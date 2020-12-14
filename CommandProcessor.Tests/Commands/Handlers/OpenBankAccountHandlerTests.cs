using Xunit;
using Moq;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Entities;
using FluentAssertions;
using System;

namespace CommandProcessor.Tests.Commands.Handlers
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