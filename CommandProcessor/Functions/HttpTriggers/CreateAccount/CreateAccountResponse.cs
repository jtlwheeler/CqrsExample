using System;

namespace CommandProcessor.Functions.HttpTriggers.CreateAccount
{
    public class CreateAccountResponse
    {
        public Guid AccountId { get; private set; }

        public CreateAccountResponse(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
