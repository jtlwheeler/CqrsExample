using Banking.Events;
using Newtonsoft.Json;

namespace Banking.CommandProcessor.Functions.DatabaseTriggers
{
    public class EventDeserializer
    {
        public static T Deserialize<T>(string jsonEvent) where T : IEvent
        {
            return JsonConvert.DeserializeObject<T>(jsonEvent);
        }
    }
}
