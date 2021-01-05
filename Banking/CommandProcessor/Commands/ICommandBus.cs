using System.Threading.Tasks;
using Banking.CommandProcessor.Entities;
using Banking.Result;

namespace Banking.CommandProcessor.Commands
{
    public interface ICommandBus
    {
        public Task<Result<EntityId>> Handle(ICommand command);
    }
}