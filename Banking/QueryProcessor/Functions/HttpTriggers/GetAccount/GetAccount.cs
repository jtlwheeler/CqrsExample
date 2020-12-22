using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Banking.QueryProcessor.Domain.BankAccount;

namespace Banking.QueryProcessor.Functions.HttpTriggers.GetAccount
{
    public class GetAccount
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public GetAccount(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        [FunctionName("GetAccount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            var bankAccount = await bankAccountRepository.Get(id);

            var response = new AccountResponse(bankAccount.Id, bankAccount.AccountHolderName);

            return new OkObjectResult(response);
        }
    }
}
