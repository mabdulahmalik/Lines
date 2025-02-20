using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Purpose.Orchestration.Commands;

public class CreatePurposeHandler : CommandHandler<CreatePurpose>
{
    private readonly IAggregateRepository<Domain.Purpose> _purposeRepository;

    public CreatePurposeHandler(ILogger<CreatePurposeHandler> logger
        , IAggregateRepository<Domain.Purpose> purposeRepository) 
        : base(logger)
    {
        _purposeRepository = purposeRepository;
    }

    protected override async Task HandleAsync(CreatePurpose command, CancellationToken stoppageToken)
    {
        var purpose = Domain.Purpose.Create(command.Name);
        await _purposeRepository.Add(purpose, stoppageToken);

        _purposeRepository.Sort(purpose.Id, Int32.MaxValue, command.InsertOnTop ? 1 : Int32.MaxValue);
        
        await _purposeRepository.Commit(stoppageToken);
    }
}