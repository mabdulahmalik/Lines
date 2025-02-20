using MassTransit;
using NodaTime;
using SOL.Abstractions.Messaging;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Workflow.JobIntake.Activities;

class JobPreperationActivity : IStateMachineActivity<JobIntakeState>
{
    private readonly ICommandBus _commandBus;

    public JobPreperationActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(JobPreperationActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobIntakeState> context, IBehavior<JobIntakeState> next)
    {
        var prepareJobCmd = new PrepareJob {
            RoomId = context.Saga.RoomId,
            FacilityId = context.Saga.FacilityId,
            PurposeId = context.Saga.PurposeId,
            OrderingProvider = context.Saga.OrderingProvider,
            Contact = context.Saga.Contact,
            PreNote = context.Saga.PreNote,
            Urgency = new IntakeUrgency {
                IsStat = context.Saga.IsStat,
                IsOnHold = context.Saga.IsOnHold,
                StatReason = context.Saga.StatReason,
                HoldReason = context.Saga.HoldReason
            },            
            Schedule = !context.Saga.ScheduledDate.HasValue
                ? null
                : new IntakeSchedule {
                    Date = LocalDate.FromDateOnly(context.Saga.ScheduledDate.Value),
                    Time = context.Saga.ScheduledTime.HasValue
                        ? LocalTime.FromTimeOnly(context.Saga.ScheduledTime.Value)
                        : null
                },
            MedicalRecord = !String.IsNullOrWhiteSpace(context.Saga.MedicalRecordNumber) || context.Saga.MedicalRecordId.HasValue
                ? new IntakeMedicalRecord {
                    Id = context.Saga.MedicalRecordId,
                    Number = context.Saga.MedicalRecordNumber
                }
                : null,
            Line = context.Saga.LineId.HasValue || context.Saga.ExternalLinePlaced
                ? new IntakeLine {
                    IsExternallyPlaced = context.Saga.ExternalLinePlaced,
                    InsertedOn = context.Saga.ExternalLineInsertedOn.HasValue
                        ? Instant.FromDateTimeUtc(context.Saga.ExternalLineInsertedOn.Value)
                        : null,
                    PlacedBy = context.Saga.ExternalLinePlacedBy,
                    Name = context.Saga.ExternalLineName,
                    LineId = context.Saga.LineId
                }
                : null
        };
        
        await _commandBus.SendAsync(prepareJobCmd, context.CancellationToken);
        await next.Execute(context);
    }

    public Task Execute<T>(BehaviorContext<JobIntakeState, T> context, IBehavior<JobIntakeState, T> next) where T : class
    {
        throw new NotImplementedException();
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobIntakeState, TException> context, IBehavior<JobIntakeState> next) where TException : Exception
    {
        await next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<JobIntakeState, T, TException> context, IBehavior<JobIntakeState, T> next) where T : class where TException : Exception
    {
        throw new NotImplementedException();
    }
}