namespace CommandProcessor.Events
{
    public interface IEventBus
    {
        public void Publish(IEvent @event);
    }
}