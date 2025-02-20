using MassTransit;
using SOL.Abstractions.Application;
using SOL.Abstractions.Persistence;
using SOL.Service.PatientEncounter.Job.Domain;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Workflows.AttachNoteToJob.Activities;

public class CreateJobNoteActivity : IStateMachineActivity<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob>
{
    private readonly IAggregateRepository<Domain.Job> _repository;
    private readonly Lazy<IOperationContext> _operationContext;
    
    public CreateJobNoteActivity(IAggregateRepository<Domain.Job> repository, IOperationContextFactory operationContextFactory)
    {
        _repository = repository;
        _operationContext = new(operationContextFactory.Get);
    }
    
    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(CreateJobNoteActivity));
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob> context
        , IBehavior<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob> next)
    {
        var job = await _repository.Get(context.Message.JobId, context.CancellationToken);
        var note = new Note(_operationContext.Value.ActorId, context.Message.Text);

        if (context.Message.IsPinned) {
            job.PinNote(note);
        } else {
            job.AddNotes(note);
        }
        
        _repository.Update(job);
        await _repository.Commit(context.CancellationToken);
        
        context.Saga.Note = note;
        
        await next.Execute(context);
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob, TException> context
        , IBehavior<AttachNoteToJobState, Messaging.Contracts.PatientEncounter.AttachNoteToJob> next) where TException : Exception
    {
        await next.Faulted(context);
    }
}