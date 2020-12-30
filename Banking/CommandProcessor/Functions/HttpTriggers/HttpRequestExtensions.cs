using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace Banking.CommandProcessor.Functions.HttpTriggers
{
    public static class HttpRequestExtensions
    {
        public static async Task<ValidatedRequest<T>> ValidateRequest<T, V>(this HttpRequest httpRequest)
            where V: AbstractValidator<T>, new()
            where T: new()
        {
            var body = await httpRequest.ParseBody<T>();

            var validator = new V();

            var validationResult = validator.Validate(body);

            if (validationResult.IsValid)
            {
                return ValidatedRequest<T>.ValidRequest(body);
            }

            return ValidatedRequest<T>.InvalidRequest(body, validationResult.Errors);
        }

        private static async Task<T> ParseBody<T>(this HttpRequest request) where T: new()
        {
            var content = await request.ReadAsStringAsync();
            var jsonBody = JsonConvert.DeserializeObject<T>(content);

            if (jsonBody == null)
            {
                return new T();
            }

            return jsonBody;
        }
    }
}
