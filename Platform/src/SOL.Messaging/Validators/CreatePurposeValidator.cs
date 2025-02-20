using FluentValidation;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Messaging.Validators;

public class CreatePurposeValidator : AbstractValidator<CreatePurpose>
{
    public CreatePurposeValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty()
            .WithMessage("A Name must be specified.");        
    }
}