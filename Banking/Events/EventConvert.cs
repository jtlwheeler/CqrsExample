using Newtonsoft.Json;

namespace Banking.Events
{
    public class EventConvert
    {
        public static T Deserialize<T>(string jsonEvent) where T : IEvent
        {
            return JsonConvert.DeserializeObject<T>(jsonEvent);
        }
    }
}
