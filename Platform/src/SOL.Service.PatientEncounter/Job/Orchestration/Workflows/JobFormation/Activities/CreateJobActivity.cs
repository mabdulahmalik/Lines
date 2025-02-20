using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Service.PatientEncounter.Job.Domain;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.JobFormation.Activities;

public class CreateJobActivity : IStateMachineActivity<JobFormationState, PrepareJob>
{
    private readonly Lazy<IOperationContext> _operationContext;
    private readonly IAggregateRepository<Domain.Job> _repository;

    public CreateJobActivity(IOperationContextFactory operationContextFactory, IAggregateRepository<Domain.Job> repository)
    {
        _operationContext = new(operationContextFactory.Get);
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateJobActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<JobFormationState, PrepareJob> context, IBehavior<JobFormationState, PrepareJob> next)
    {
        var job = Domain.Job.Create(context.Message.PurposeId, context.Message.RoomId, context.Saga.LineId
            , context.Saga.MedicalRecordId, context.Message.Contact, context.Message.OrderingProvider
            , context.Message.Schedule?.Date, context.Message.Schedule?.Time);

        if(!String.IsNullOrWhiteSpace(context.Message.PreNote)) {
            job.PinNote(new Note(_operationContext.Value.ActorId, context.Message.PreNote));
        }

        await _repository.Add(job, context.CancellationToken);
        await _repository.Commit(context.CancellationToken);
        
        context.Saga.JobId = job.Id;
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<JobFormationState, PrepareJob, TException> context, IBehavior<JobFormationState, PrepareJob> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}