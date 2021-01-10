using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using Banking.QueryProcessor.Queries.Handlers;
using System.Linq;
using System;

namespace Banking.QueryProcessor.Functions.HttpTriggers.GetTransactions
{
    public class GetTransactions
    {
        private readonly IMediator mediator;

        public GetTransactions(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("GetTransactions")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{id}/transaction")] HttpRequest req,
            string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var query = new TransactionQuery(id);

                var transactions = await mediator.Send(query);

                var transactionsResponse = transactions.Select(transaction =>
                    new GetTransactionsResponse
                    {
                        Id = transaction.Id,
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Type = transaction.Type.ToString()
                    }
                );

                return new OkObjectResult(transactionsResponse);
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace);
                return new StatusCodeResult(500);
            }
        }
    }
}
