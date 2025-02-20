using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobActivation.Activities;

public class CreateEncounter : IStateMachineActivity<JobActivationState, ActivateScheduledJobs>
{
    private readonly IAggregateRepository<Encounter.Domain.Encounter> _repository;

    public CreateEncounter(IAggregateRepository<Encounter.Domain.Encounter> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateEncounter));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobActivationState, ActivateScheduledJobs> context, IBehavior<JobActivationState, ActivateScheduledJobs> next)
    {
        foreach (var jobEncounterData in context.Saga.EncounterData.OrderBy(x => x.Order))
        {
            var encounter = Encounter.Domain.Encounter.Create(jobEncounterData.JobId, jobEncounterData.FacilityRoomId
                , jobEncounterData.PurposeId, jobEncounterData.MedicalRecordId);
            await _repository.Add(encounter, context.CancellationToken);
        }

        await _repository.Commit(context.CancellationToken);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobActivationState, ActivateScheduledJobs, TException> context
        , IBehavior<JobActivationState, ActivateScheduledJobs> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}