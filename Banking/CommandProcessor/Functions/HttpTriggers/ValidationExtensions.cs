using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Banking.CommandProcessor.Functions.HttpTriggers
{
    public static class ValidationExtensions
    {
        public static BadRequestObjectResult ToBadRequest<T>(this ValidatedRequest<T> request)
        {
            return new BadRequestObjectResult(request.Errors.Select(e => new {
                Field = e.PropertyName,
                Error = e.ErrorMessage
            }));
        }
    }
}
