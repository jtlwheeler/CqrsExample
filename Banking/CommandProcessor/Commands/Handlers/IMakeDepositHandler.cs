using System.Threading.Tasks;
using Banking.CommandProcessor.Commands.Commands;
using Banking.CommandProcessor.Entities;
using Banking.Result;

namespace Banking.CommandProcessor.Commands.Handlers
{
    public interface IMakeDepositHandler
    {
        public Task<Result<EntityId>> Handle(MakeDepositCommand command);
    }
}
