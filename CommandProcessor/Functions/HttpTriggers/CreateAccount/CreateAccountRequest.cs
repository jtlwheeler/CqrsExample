using FluentValidation;

namespace CommandProcessor.Functions.HttpTriggers.CreateAccount
{
    public class CreateAccountRequest
    {
        public string Name { get; set; }
    }

    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
