using CommandProcessor.Commands.Commands;

namespace CommandProcessor.Commands.Handlers
{
    public interface IOpenBankAccountHandler
    {
        public void Handle(OpenBankAccountCommand command);
    }
}
