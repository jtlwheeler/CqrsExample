using Xunit;
using Moq;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Tests.Commands.Handlers
{
    public class OpenBankAccountHandlerTests
    {
        [Fact]
        public void ShouldOpenBankAccountAndSaveEntity()
        {
            var entityStore = new Mock<IEntityStore>();
            var handler = new OpenBankAccountHandler(entityStore.Object);
            var command = new OpenBankAccountCommand
            {
                Name = "Bob Smith"
            };

            handler.Handle(command);

            entityStore.Verify(m => m.Save(It.IsAny<BankAccount>()));
        }
    }
}