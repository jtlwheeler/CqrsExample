namespace CommandProcessor.Events.Persistence
{
    public interface IEventStore
    {
        public void Save(IEvent @event);
    }
}
