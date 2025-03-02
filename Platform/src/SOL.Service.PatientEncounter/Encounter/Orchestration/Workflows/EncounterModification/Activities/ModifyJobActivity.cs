using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Workflows.EncounterModification.Activities;

public class ModifyJobActivity : IStateMachineActivity<EncounterModificationState, ModifyEncounter>
{
    private readonly IAggregateRepository<Job.Domain.Job> _repository;

    public ModifyJobActivity(IAggregateRepository<Job.Domain.Job> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(ModifyJobActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncounterModificationState, ModifyEncounter> context, IBehavior<EncounterModificationState, ModifyEncounter> next)
    {
        var job = await _repository.Get(context.Message.JobId, context.CancellationToken);
        job.Modify(context.Message.RoomId, job.LineId, context.Saga.MedicalRecordId
            , context.Message.OrderingProvider, context.Message.Contact);
        
        await _repository.Update(job, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<EncounterModificationState, ModifyEncounter, TException> context, IBehavior<EncounterModificationState, ModifyEncounter> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}