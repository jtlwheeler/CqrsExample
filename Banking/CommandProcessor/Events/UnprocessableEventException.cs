using System;
namespace Banking.CommandProcessor.Events
{
    public class UnprocessableEventException: Exception
    {
        public UnprocessableEventException(string message) : base(message)
        {
        }
    }
}
