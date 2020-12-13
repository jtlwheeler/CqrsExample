using System;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Entities;
using CommandProcessor.Result;

namespace CommandProcessor.Commands.Handlers
{
    public class OpenBankAccountHandler : IOpenBankAccountHandler
    {
        private IEntityStore entityStore;

        public OpenBankAccountHandler(IEntityStore entityStore)
        {
            this.entityStore = entityStore;
        }

        public Result<Guid> Handle(OpenBankAccountCommand command)
        {
            var newBankAccount = new BankAccount();
            newBankAccount.Open(command.Name);

            entityStore.Save(newBankAccount);

            return Result<Guid>.Ok(newBankAccount.Id);
        }
    }
}
