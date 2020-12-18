using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace CommandProcessor.Events
{
    public class ServiceBusEventBus : IEventBus
    {
        private ServiceBusClient serviceBusClient;
        private string queueName = "eventstream";
        public ServiceBusEventBus(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
        }

        public async Task Publish<T>(T @event) where T : IEvent
        {
            ServiceBusSender sender = serviceBusClient.CreateSender(queueName);
            ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(@event));

            await sender.SendMessageAsync(message);
        }
    }
}
