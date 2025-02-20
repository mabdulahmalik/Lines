using MassTransit;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation.Activities;

public class CreateExternalLineActivity : IStateMachineActivity<JobFormationState, PrepareJob>
{
    private readonly IAggregateRepository<Line.Domain.Line> _repository;

    public CreateExternalLineActivity(IAggregateRepository<Line.Domain.Line> repository)
    {
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateExternalLineActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobFormationState, PrepareJob> context, IBehavior<JobFormationState, PrepareJob> next)
    {
        var line = Line.Domain.Line.Create(context.Message.RoomId, context.Message.Line.Name
            , insertedOn: context.Message.Line.InsertedOn
            , externallyPlacedBy: context.Message.Line.PlacedBy
            , medicalRecordId: context.Saga.MedicalRecordId
            , externallyPlaced: true);
        
        await _repository.Add(line, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);
        
        context.Saga.LineId = line.Id;

        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobFormationState, PrepareJob, TException> context, IBehavior<JobFormationState, PrepareJob> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}