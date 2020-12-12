using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Commands.Handlers
{
    public class OpenBankAccountHandler : IOpenBankAccountHandler
    {
        private IEntityStore entityStore;

        public OpenBankAccountHandler(IEntityStore entityStore)
        {
            this.entityStore = entityStore;
        }

        public void Handle(OpenBankAccountCommand command)
        {
            var newBankAccount = new BankAccount();
            newBankAccount.Open(command.Name);

            entityStore.Save(newBankAccount);
        }
    }
}
