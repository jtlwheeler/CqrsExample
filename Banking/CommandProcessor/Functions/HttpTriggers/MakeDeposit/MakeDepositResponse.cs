using System;

namespace Banking.CommandProcessor.Functions.HttpTriggers.CreateAccount
{
    public class MakeDepositResponse
    {
        public Guid DepositId { get; private set; }

        public MakeDepositResponse(Guid depositId)
        {
            DepositId = depositId;
        }
    }
}
