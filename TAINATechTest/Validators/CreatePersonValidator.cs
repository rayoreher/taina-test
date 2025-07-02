using FluentValidation;
using TAINATechTest.Services.ViewModels;

namespace TAINATechTest.Validators
{
    public class CreatePersonValidator : AbstractValidator<CreatePersonViewModel>
    {
        public CreatePersonValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(x =>
                {
                    return x == "male" || x == "female";
                })
                .WithMessage("Gender must be male or female only");

            RuleFor(x => x.EmailAddress)
                .EmailAddress().WithMessage("Email address must have the correct email format")
                .Length(2, 50).WithMessage("Email address must be between 2 and 50 characters");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");
        }
    }
}
