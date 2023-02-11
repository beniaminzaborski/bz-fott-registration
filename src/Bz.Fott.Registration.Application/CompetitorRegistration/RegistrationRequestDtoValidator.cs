using FluentValidation;

namespace Bz.Fott.Registration.Application.CompetitorRegistration;

public class RegistrationRequestDtoValidator : AbstractValidator<RegistrationRequestDto>
{
	public RegistrationRequestDtoValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("Cannot be empty");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Cannot be empty")
            .MaximumLength(100).WithMessage("Maximum lenght is 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Cannot be empty")
            .MaximumLength(150).WithMessage("Maximum lenght is 150 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Cannot be empty")
            .MaximumLength(17).WithMessage("Maximum lenght is 17 characters");

        RuleFor(x => x.ContactPersonNumber)
            .NotEmpty().WithMessage("Cannot be empty")
            .MaximumLength(17).WithMessage("Maximum lenght is 17 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Cannot be empty")
            .MaximumLength(100).WithMessage("Maximum lenght is 100 characters");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Cannot be empty")
            .LessThan(x => DateTime.UtcNow.AddYears(-12)).WithMessage("You have to be at least 12 years old");
    }
}
