using System;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Result;

namespace CommandProcessor.Commands.Handlers
{
    public interface IOpenBankAccountHandler
    {
        public Result<Guid> Handle(OpenBankAccountCommand command);
    }
}
