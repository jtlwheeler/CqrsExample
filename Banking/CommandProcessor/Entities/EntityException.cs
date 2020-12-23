using System;

namespace Banking.CommandProcessor.Entities
{
    public class EntityException: Exception
    {
        public EntityException(string message) : base(message)
        {
        }
    }
}
