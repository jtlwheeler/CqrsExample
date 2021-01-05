using System;
using System.Threading.Tasks;
using Banking.CommandProcessor.Commands.Commands;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.CommandProcessor.Entities;
using Banking.Result;

namespace Banking.CommandProcessor.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IOpenBankAccountHandler openBankAccountHandler;
        private readonly IMakeDepositHandler makeDepositHandler;

        public CommandBus(IOpenBankAccountHandler openBankAccountHandler, IMakeDepositHandler makeDepositHandler)
        {
            this.openBankAccountHandler = openBankAccountHandler;
            this.makeDepositHandler = makeDepositHandler;
        }

        public async Task<Result<EntityId>> Handle(ICommand command)
        {
            if (command is OpenBankAccountCommand)
            {
                return await openBankAccountHandler.Handle((OpenBankAccountCommand)command);
            }
            else if (command is MakeDepositCommand)
            {
                return await makeDepositHandler.Handle((MakeDepositCommand)command);
            }
            else
            {
                throw new UnknownCommandException("Unknown Command");
            }
        }
    }

    public class UnknownCommandException : Exception
    {
        public UnknownCommandException(string message) : base(message)
        {
        }
    }
}
