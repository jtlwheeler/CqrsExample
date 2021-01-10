using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Banking.QueryProcessor.Queries.Handlers;
using MediatR;

namespace Banking.QueryProcessor.Functions.HttpTriggers.GetAccount
{
    public class GetAccount
    {
        private readonly IMediator mediator;

        public GetAccount(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("GetAccount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            
            var bankAccountQuery = new BankAccountQuery(id);
            var bankAccount = await mediator.Send(bankAccountQuery);

            var response = new AccountResponse(
                bankAccount.Id,
                bankAccount.AccountHolderName,
                bankAccount.Balance
            );

            return new OkObjectResult(response);
        }
    }
}
