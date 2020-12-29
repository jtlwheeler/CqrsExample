using System;
using System.Threading.Tasks;
using Banking.CommandProcessor.Commands.Commands;
using Banking.CommandProcessor.Entities;
using Banking.Result;

namespace Banking.CommandProcessor.Commands.Handlers
{
    public class MakeDepositHandler: IMakeDepositHandler
    {
        private readonly IEntityStore entityStore;

        public MakeDepositHandler(IEntityStore entityStore)
        {
            this.entityStore = entityStore;
        }

        public async Task<Result<DepositId>> Handle(MakeDepositCommand command)
        {
            var entity = await entityStore.Load<BankAccount>(command.AccountId);

            var depositId = entity.MakeDeposit(command.Description, command.Amount);

            await entityStore.Save(entity);

            return Result<DepositId>.Ok(depositId);
        }
    }
}
