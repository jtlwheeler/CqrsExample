using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Banking.QueryProcessor.Queries.Queries;
using Banking.QueryProcessor.Queries.Handlers;

namespace Banking.QueryProcessor.Functions.HttpTriggers.GetAccount
{
    public class GetAccount
    {
        private readonly IBankAccountQueryHandler bankAccountQueryHandler;

        public GetAccount(IBankAccountQueryHandler bankAccountQueryHandler)
        {
            this.bankAccountQueryHandler = bankAccountQueryHandler;
        }

        [FunctionName("GetAccount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            
            var bankAccountQuery = new BankAccountQuery(id);
            var bankAccount = await bankAccountQueryHandler.Handle(bankAccountQuery);

            var response = new AccountResponse(
                bankAccount.Id,
                bankAccount.AccountHolderName,
                bankAccount.Balance
            );

            return new OkObjectResult(response);
        }
    }
}
