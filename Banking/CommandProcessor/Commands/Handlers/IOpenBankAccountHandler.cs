using System;
using System.Threading.Tasks;
using Banking.CommandProcessor.Commands.Commands;
using CommandProcessor.Result;

namespace Banking.CommandProcessor.Commands.Handlers
{
    public interface IOpenBankAccountHandler
    {
        public Task<Result<Guid>> Handle(OpenBankAccountCommand command);
    }
}