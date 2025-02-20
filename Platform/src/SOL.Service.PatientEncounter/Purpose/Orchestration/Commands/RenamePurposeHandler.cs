using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Purpose.Orchestration.Commands;

public class RenamePurposeHandler : CommandHandler<RenamePurpose>
{
    private readonly IAggregateRepository<Domain.Purpose> _purposeRepository;

    public RenamePurposeHandler(ILogger<RenamePurposeHandler> logger
        , IAggregateRepository<Domain.Purpose> purposeRepository) 
        : base(logger)
    {
        _purposeRepository = purposeRepository;
    }

    protected override async Task HandleAsync(RenamePurpose command, CancellationToken stoppageToken)
    {
        var purpose = await _purposeRepository.Get(command.PurposeId, stoppageToken);
        purpose.Rename(command.Name);

        _purposeRepository.Update(purpose);
        await _purposeRepository.Commit(stoppageToken);
    }
}
