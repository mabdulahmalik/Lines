using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class RemoveNoteFromJobHandler : CommandHandler<RemoveNoteFromJob>
{
    private readonly IAggregateRepository<Domain.Job> _repository;

    public RemoveNoteFromJobHandler(ILogger<RemoveNoteFromJobHandler> logger
        , IAggregateRepository<Domain.Job> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(RemoveNoteFromJob command, CancellationToken stoppageToken)
    {
        var job = await _repository.Get(command.JobId, stoppageToken);
        var exists = job.Notes.Any(x => x.Id == command.Id);

        if (!exists) return;

        job.RemoveNotes([command.Id]);

        await _repository.Update(job, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}
