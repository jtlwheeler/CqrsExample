using System;
using System.Threading.Tasks;
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

        public async Task<Result<Guid>> Handle(OpenBankAccountCommand command)
        {
            var newBankAccount = new BankAccount();
            newBankAccount.Open(command.Name);

            await entityStore.Save(newBankAccount);

            return Result<Guid>.Ok(newBankAccount.Id);
        }
    }
}
