namespace CommandProcessor.Commands
{
    public interface ICommandBus
    {
        public void Handle(ICommand command);
    }
}