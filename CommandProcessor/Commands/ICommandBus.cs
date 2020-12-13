using System;
using CommandProcessor.Result;
namespace CommandProcessor.Commands
{
    public interface ICommandBus
    {
        public Result<Guid> Handle(ICommand command);
    }
}