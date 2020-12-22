using System.Threading.Tasks;
using Banking.Events;

namespace Banking.CommandProcessor.Events.EventStore
{
    public interface IEventStore
    {
        public Task Save(IEvent @event);
    }
}