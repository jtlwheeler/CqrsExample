using System;
using System.Threading.Tasks;
using Banking.CommandProcessor.Commands.Commands;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.Result;

namespace Banking.CommandProcessor.Commands
{
    public class CommandBus : ICommandBus
    {
        private IOpenBankAccountHandler openBankAccountHandler;

        public CommandBus(IOpenBankAccountHandler openBankAccountHandler)
        {
            this.openBankAccountHandler = openBankAccountHandler;
        }

        public async Task<Result<Guid>> Handle(ICommand command)
        {

            if (command is OpenBankAccountCommand)
            {
                return await openBankAccountHandler.Handle((OpenBankAccountCommand)command);
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
