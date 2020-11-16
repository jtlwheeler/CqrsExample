using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CommandProcessor.Commands;
using CommandProcessor.Commands.Commands;
using CommandProcessor.Commands.Entities;

namespace CommandProcessor.Functions.Web
{
    public class CreateGreeting
    {
        private readonly ICommandBus commandBus;

        public CreateGreeting(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        [FunctionName("CreateGreeting")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create greeting");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var greeting = JsonConvert.DeserializeObject<GreetingRequest>(content);

            var command = new CreateGreetingCommand
            {
                Greeting = new Greeting
                {
                    Message = greeting.Message
                }
            };

            commandBus.Handle(command);

            return new OkResult();
        }
    }

    public class GreetingRequest
    {
        public string Message { get; set; }
    }
}
