using System.Threading.Tasks;
using Banking.Events;

namespace Banking.CommandProcessor.Events
{
    public interface IEventBus
    {
        public Task Publish<T>(T @event) where T: IEvent;
    }
}
