using FluentValidation;

namespace CommandProcessor.Functions.Web
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
