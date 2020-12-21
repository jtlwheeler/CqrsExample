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
using System.Linq;
using FluentValidation.Results;

namespace CommandProcessor.Functions.HttpTriggers.CreateAccount
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
            var createAccountRequest = await ParseBody(req);

            var validationResult = ValidateRequest(createAccountRequest);

            if (!validationResult.IsValid)
            {
                return CreateBadRequestResponse(validationResult);
            }

            var command = new OpenBankAccountCommand
            {
                Name = createAccountRequest.Name
            };

            var result = await commandBus.Handle(command);

            return new OkObjectResult(new CreateAccountResponse(result.Value));
        }

        private async Task<CreateAccountRequest> ParseBody(HttpRequest request)
        {
            var content = await new StreamReader(request.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<CreateAccountRequest>(content);
        }

        private ValidationResult ValidateRequest(CreateAccountRequest requestBody)
        {
            var validator = new CreateAccountRequestValidator();
            return validator.Validate(requestBody);
        }

        private BadRequestObjectResult CreateBadRequestResponse(ValidationResult validationResult)
        {
            return new BadRequestObjectResult(validationResult.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Error = e.ErrorMessage
            }));
        }
    }
}
