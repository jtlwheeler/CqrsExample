using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using FluentValidation.Results;
using Banking.CommandProcessor.Commands;
using Banking.CommandProcessor.Commands.Commands;

namespace Banking.CommandProcessor.Functions.HttpTriggers.CreateAccount
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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "account")] HttpRequest req,
            ILogger log)
        {
            var validationResult = await req.ValidateRequest<CreateAccountRequest, CreateAccountRequestValidator>();

            if (!validationResult.IsValid)
            {
                log.LogInformation("Invalid request.");
                return validationResult.ToBadRequest();
            }

            var validatedRequest = validationResult.Value;

            var command = new OpenBankAccountCommand
            {
                Name = validatedRequest.Name
            };

            var result = await commandBus.Handle(command);

            return new OkObjectResult(new CreateAccountResponse(result.Value));
        }
    }
}
