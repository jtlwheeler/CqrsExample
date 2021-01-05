using System;

namespace Banking.CommandProcessor.Entities
{
    public sealed class AccountId: EntityId
    {
        public AccountId(Guid value)
        {
            Value = value;
        }

        public AccountId()
        {
            Value = Guid.NewGuid();
        }
    }
}
