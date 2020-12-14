using System;
using System.Threading.Tasks;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Result;

namespace CommandProcessor.Commands.Handlers
{
    public interface IOpenBankAccountHandler
    {
        public Task<Result<Guid>> Handle(OpenBankAccountCommand command);
    }
}
