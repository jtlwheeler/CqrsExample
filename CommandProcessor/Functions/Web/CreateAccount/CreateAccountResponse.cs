using System;

namespace CommandProcessor.Functions.Web.CreateAccount
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
