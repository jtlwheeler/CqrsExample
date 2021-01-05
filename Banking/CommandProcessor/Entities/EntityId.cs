using System;

namespace Banking.CommandProcessor.Entities
{
    public abstract class EntityId
    {
        public Guid Value { get; protected set; }
    }
}
