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
    public class CreateAccount
    {
        private readonly ICommandBus commandBus;

        public CreateAccount(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        [FunctionName("CreateAccount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create account");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var createAccountRequest = JsonConvert.DeserializeObject<CreateAccountRequest>(content);

            var command = new OpenBankAccountCommand
            {
                Name = createAccountRequest.Name
            };

            commandBus.Handle(command);

            return new OkResult();
        }
    }

    public class CreateAccountRequest
    {
        public string Name { get; set; }
    }
}
