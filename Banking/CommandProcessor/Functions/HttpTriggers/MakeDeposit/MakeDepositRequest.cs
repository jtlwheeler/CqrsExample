using System;
using FluentValidation;

namespace Banking.CommandProcessor.Functions.HttpTriggers.CreateAccount
{
    public class MakeDepositRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }

    public class MakeDepositRequestValidator : AbstractValidator<MakeDepositRequest>
    {
        public MakeDepositRequestValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0m);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
