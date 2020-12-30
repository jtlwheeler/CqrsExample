using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Banking.CommandProcessor.Commands;
using Banking.CommandProcessor.Functions.HttpTriggers.CreateAccount;
using Banking.CommandProcessor.Commands.Commands;

namespace Banking.CommandProcessor.Functions.HttpTriggers.MakeDeposit
{
    public class MakeDeposit
    {
        private readonly ICommandBus commandBus;

        public MakeDeposit(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        [FunctionName("MakeDeposit")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{accountId}/deposit")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var validatedRequest = await req.ValidateRequest<MakeDepositRequest, MakeDepositRequestValidator>();

                if (!validatedRequest.IsValid)
                {
                    return validatedRequest.ToBadRequest();
                }

                var body = validatedRequest.Value;

                var command = new MakeDepositCommand(body.AccountId, body.Amount, body.Description);

                var result = await commandBus.Handle(command);

                return new OkObjectResult(new MakeDepositResponse(result.Value));
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}
