using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Procedure.Orchestration.Commands;

public class ArchiveProcedureHandler : CommandHandler<ArchiveProcedure>
{
    private readonly IAggregateRepository<Domain.Procedure> _repository;

    public ArchiveProcedureHandler(ILogger<ArchiveProcedureHandler> logger
        , IAggregateRepository<Domain.Procedure> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ArchiveProcedure command, CancellationToken stoppageToken)
    {
        await _repository.Archive(new SingleInstanceSpecification<Domain.Procedure>(command.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}