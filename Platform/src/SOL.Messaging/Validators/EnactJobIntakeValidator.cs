using FluentValidation;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Messaging.Validators;

public class EnactJobIntakeValidator : AbstractValidator<EnactJobIntake>
{
    public EnactJobIntakeValidator()
    {
        /*
        RuleFor(x => x.PurposeId)
            .NotEmpty()
            .WithMessage("A Purpose must be specified.");

        RuleFor(x => x.ExternalLine.PlacedBy)
            .Custom((value, context) => { 
                if(String.IsNullOrWhiteSpace(value)) {
                    context.AddFailure(context.PropertyPath, "Must provided a name for who Externally Inserted the Line.");
                }
            });
        
        RuleFor(x => x.Location.FacilityId)
            .NotEmpty()
            .WithMessage("A Facility must be specified.");

        RuleFor(x => x.Location.RoomId)
            .Custom((value, context) => {
                if(!value.HasValue && String.IsNullOrWhiteSpace(context.InstanceToValidate.Location.RoomName)) {
                    context.AddFailure(context.PropertyPath, "A Room must be specified.");
                }
            });

        RuleFor(x => x.ScheduledFor)
            .Custom((value, context) => {
                if(value.HasValue && value.Value.ToDateTimeUtc() <= DateTime.Today) {
                    context.AddFailure(context.PropertyPath, "A future date must be specified for a scheduled Job.");
                }

                if(value.HasValue && (context.InstanceToValidate.Urgency.IsStat || context.InstanceToValidate.Urgency.IsOnHold)) {
                    context.AddFailure("Cannot schedule this Job AND mark it as 'Stat' or 'On Hold'.");
                }
            });
        */
    }
}