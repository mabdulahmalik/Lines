using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation.Activities;

public class CreateEncounterActivity : IStateMachineActivity<JobFormationState, PrepareJob>
{
    private readonly IAggregateRepository<Encounter.Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public CreateEncounterActivity(IOperationContextFactory operationContextFactory
        , IAggregateRepository<Encounter.Domain.Encounter> repository)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateEncounterActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobFormationState, PrepareJob> context, IBehavior<JobFormationState, PrepareJob> next)
    {
        var encounter = Encounter.Domain.Encounter.Create(context.Saga.JobId, context.Message.RoomId
            , context.Message.PurposeId, context.Saga.MedicalRecordId);
        
        if (context.Message?.Urgency != null)
        {
            if (context.Message.Urgency.IsStat) {
                encounter.Escalate(context.Message.Urgency?.StatReason);
            }

            if (context.Message.Urgency.IsOnHold) {
                encounter.PlaceOnHold(_operationContext.Value.ActorId, context.Message.Urgency?.HoldReason);
            }
        }

        await _repository.Add(encounter, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);
        
        context.Saga.EncounterId = encounter.Id;
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobFormationState, PrepareJob, TException> context, IBehavior<JobFormationState, PrepareJob> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}