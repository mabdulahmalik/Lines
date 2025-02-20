using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.PatientEncounter.Activities;

public class AssignMeToEncounterActivity : IStateMachineActivity<PatientEncounterState, AssignMeToEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;
    private readonly Lazy<IOperationContext> _operationContext;

    public AssignMeToEncounterActivity(IAggregateRepository<Domain.Encounter> repository, IOperationContextFactory operationContextFactory)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(AssignMeToEncounterActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<PatientEncounterState, AssignMeToEncounter> context, IBehavior<PatientEncounterState, AssignMeToEncounter> next)
    {
        var encounter = await _repository.Get(context.Message.EncounterId, context.CancellationToken);

        var position = encounter.Assignments.All(x => x.Position != EncounterAssigneePosition.Primary)
            ? EncounterAssigneePosition.Primary
            : EncounterAssigneePosition.Assistant;

        encounter.Assign([new Assignment(_operationContext.Value.ActorId, position)]);

        _repository.Update(encounter);
        await _repository.Commit(context.CancellationToken);
        
        context.Saga.FacilityRoomId = context.Message.FacilityRoomId;
        context.Saga.MedicalRecordId = context.Message.MedicalRecordId;
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<PatientEncounterState, AssignMeToEncounter, TException> context
        , IBehavior<PatientEncounterState, AssignMeToEncounter> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}