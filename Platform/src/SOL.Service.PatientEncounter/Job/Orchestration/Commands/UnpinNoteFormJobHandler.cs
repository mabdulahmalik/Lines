using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class UnpinNoteFormJobHandler : CommandHandler<UnpinNoteFromJob>
{
    private readonly IAggregateRepository<Domain.Job> _repository;

    public UnpinNoteFormJobHandler(ILogger<UnpinNoteFormJobHandler> logger
        , IAggregateRepository<Domain.Job> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(UnpinNoteFromJob command, CancellationToken stoppageToken)
    {
        var job = await _repository.Get(command.JobId, stoppageToken);
        var note = job.Notes.Single(x => x.Id == command.Id);

        job.UnpinNote(note);
        
        await _repository.Update(job, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}