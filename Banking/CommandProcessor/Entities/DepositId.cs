using System;
namespace Banking.CommandProcessor.Entities
{
    public class DepositId
    {
        public Guid Id { get; private set; }

        public DepositId()
        {
            Id = Guid.NewGuid();
        }
    }
}
