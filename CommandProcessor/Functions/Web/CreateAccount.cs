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
using System;
using FluentValidation;
using System.Linq;

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

            var validator = new CreateAccountRequestValidator();
            var validationResult = validator.Validate(createAccountRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            var command = new OpenBankAccountCommand
            {
                Name = createAccountRequest.Name
            };

            var result = await commandBus.Handle(command);

            return new OkObjectResult(new CreateAccountResponse(result.Value));
        }
    }

    public class CreateAccountRequest
    {
        public string Name { get; set; }
    }

    public class CreateAccountResponse
    {
        public Guid AccountId { get; private set; }

        public CreateAccountResponse(Guid accountId)
        {
            AccountId = accountId;
        }
    }

    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
