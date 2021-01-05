using System;

namespace Banking.CommandProcessor.Entities
{
    public sealed class DepositId: EntityId
    {
        public DepositId()
        {
            Value = Guid.NewGuid();
        }

        public DepositId(Guid value)
        {
            Value = value;
        }
    }
}
