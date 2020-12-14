using System.Threading.Tasks;

namespace CommandProcessor.Events.Persistence
{
    public interface IEventStore
    {
        public Task Save(IEvent @event);
    }
}
