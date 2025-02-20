using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.DataAccess.Specifications;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Procedure.Orchestration.Commands;

public class UnarchiveProcedureHandler : CommandHandler<UnarchiveProcedure>
{
    private readonly IAggregateRepository<Domain.Procedure> _repository;
    
    public UnarchiveProcedureHandler(ILogger<UnarchiveProcedureHandler> logger
        , IAggregateRepository<Domain.Procedure> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(UnarchiveProcedure command, CancellationToken stoppageToken)
    {
        await _repository.Unarchive(new SingleInstanceSpecification<Domain.Procedure>(command.Id), stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}