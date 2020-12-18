using System.Threading.Tasks;

namespace CommandProcessor.Events
{
    public interface IEventBus
    {
        public Task Publish<T>(T @event) where T: IEvent;
    }
}
