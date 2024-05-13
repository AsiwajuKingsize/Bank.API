using FluentValidation;
using Payment.API.DTO;

namespace Payment.API.Validators
{
    public class AccountTransactionValidator : AbstractValidator<AccountTransactionDTO>
    {
        /// <summary>
        /// Account Transaction Model Validator
        /// </summary>
        public AccountTransactionValidator() 
        {
            RuleFor(x => x.debitAccountId).NotNull().WithMessage("Please enter the account to debit");
            RuleFor(x => x.creditAccountId).NotNull().WithMessage("Please enter the account to credit");
            RuleFor(x => x.amount).GreaterThan(0).WithMessage("Please enter a valid amount to Credit receiver's account");
            RuleFor(x => x.transactionPin).Custom((x, context) =>
            {
                if (x.ToString().Length != 4)
                {
                    context.AddFailure("The PIN must be 4 digits long");
                }
            });
            RuleFor(x => x.debitAccountId).NotEqual(x => x.creditAccountId).WithMessage("Enter different account numbers for Transaction");

        }    
    }
}
