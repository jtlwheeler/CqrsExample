using System.Collections.Generic;
using FluentValidation.Results;

namespace Banking.CommandProcessor.Functions.HttpTriggers
{
    public class ValidatedRequest<T>
    {
        public bool IsValid { get; private set; }
        public T Value { get; private set; }
        public IList<ValidationFailure> Errors { get; private set; }

        public ValidatedRequest(bool isValid, T value)
        {
            IsValid = isValid;
            Value = value;
        }

        public ValidatedRequest(bool isValid, T value, IList<ValidationFailure> errors)
        {
            IsValid = isValid;
            Value = value;
            Errors = errors;
        }

        public static ValidatedRequest<T> ValidRequest(T value)
        {
            return new ValidatedRequest<T>(true, value);
        }

        public static ValidatedRequest<T> InvalidRequest(T value, IList<ValidationFailure> errors)
        {
            return new ValidatedRequest<T>(false, value, errors);
        }
    }
}
