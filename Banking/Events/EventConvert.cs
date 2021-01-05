using Banking.CommandProcessor.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Banking.Events
{
    public class EventConvert
    {
        public static T Deserialize<T>(string jsonEvent) where T : Event
        {
            return JsonConvert.DeserializeObject<T>(jsonEvent);
        }

        public static Event Deserialize(string jsonEvent)
        {
            var json = JsonConvert.DeserializeObject<JObject>(jsonEvent);
            var eventType = json.GetValue("Type").ToString();

            if (eventType == BankAccountCreatedEvent.EventTypeName)
            {
                return JsonConvert.DeserializeObject<BankAccountCreatedEvent>(jsonEvent);
            }
            else if (eventType == DepositMadeEvent.EventTypeName)
            {
                return JsonConvert.DeserializeObject<DepositMadeEvent>(jsonEvent);
            }

            throw new UnprocessableEventException("Invalid Event Type.");
        }
    }
}
