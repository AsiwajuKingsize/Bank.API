using FluentValidation;
using Payment.API.DTO;

namespace Payment.API.Validators
{
    public class AccountValidator :  AbstractValidator<AccountDTO> 
    {
        public AccountValidator()
        {
            RuleFor(x => x.surname).Length(2,40).NotNull().WithMessage("Please enter surname");
            RuleFor(x => x.firstName).Length(2,40).NotNull().WithMessage("Please enter first name");
            RuleFor(x => x.transactionPin).NotNull().GreaterThan(0);
            RuleFor(x => x.transactionPin).Custom((x, context) =>
            {
                if (x.ToString().Length != 4)
                {
                    context.AddFailure("The PIN must be 4 digits long");
                }
            });
        }
    }
}
