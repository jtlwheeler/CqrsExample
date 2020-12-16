using CommandProcessor.Events;
using Newtonsoft.Json;

namespace CommandProcessor.Functions.DatabaseTrigger
{
    public class EventDeserializer
    {
        public static T Deserialize<T>(string jsonEvent) where T : IEvent
        {
            return JsonConvert.DeserializeObject<T>(jsonEvent);
        }
    }
}
