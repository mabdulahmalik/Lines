using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Job.Orchestration.Commands;

public class ModifyNoteForJobHandler : CommandHandler<ModifyNoteForJob>
{
    private readonly IAggregateRepository<Domain.Job> _repository;

    public ModifyNoteForJobHandler(ILogger<ModifyNoteForJobHandler> logger
        , IAggregateRepository<Domain.Job> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyNoteForJob command, CancellationToken stoppageToken)
    {
        var job = await _repository.Get(command.JobId, stoppageToken);
        job.ModifyNote(command.Id, command.Text);

        _repository.Update(job);
        await _repository.Commit(stoppageToken);
    }
}