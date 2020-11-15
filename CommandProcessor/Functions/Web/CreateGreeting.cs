using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommandProcessor.Functions.Web
{
    public class CreateGreeting
    {
        public CreateGreeting()
        {
        }

        [FunctionName("CreateGreeting")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var greeting = JsonConvert.DeserializeObject<GreetingRequest>(content);

            return new OkResult();
        }
    }

    public class GreetingRequest
    {
        public string Message { get; set; }
    }
}
