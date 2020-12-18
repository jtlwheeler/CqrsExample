using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace CommandProcessor.Events
{
    public class ServiceBusEventBus : IEventBus
    {
        private string queueName = "eventstream";
        private readonly ServiceBusClient serviceBusClient;
        private readonly ServiceBusSender serviceBusSender;

        public ServiceBusEventBus(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
            serviceBusSender = serviceBusClient.CreateSender(queueName);
        }

        public async Task Publish<T>(T @event) where T : IEvent
        {
            ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(@event));
            await serviceBusSender.SendMessageAsync(message);
        }
    }
}
