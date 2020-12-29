using System;
using System.Threading.Tasks;
using Banking.CommandProcessor.Commands.Commands;
using Banking.Result;

namespace Banking.CommandProcessor.Commands.Handlers
{
    public interface IMakeDepositHandler
    {
        public Task<Result<Guid>> Handle(MakeDepositCommand command);
    }
}
