using System;
using System.Threading.Tasks;
using CommandProcessor.Result;
namespace CommandProcessor.Commands
{
    public interface ICommandBus
    {
        public Task<Result<Guid>> Handle(ICommand command);
    }
}