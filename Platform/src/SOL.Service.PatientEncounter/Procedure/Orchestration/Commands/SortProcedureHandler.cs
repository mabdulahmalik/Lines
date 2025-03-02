using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;

namespace SOL.Service.PatientEncounter.Procedure.Orchestration.Commands;

public class SortProcedureHandler : CommandHandler<SortProcedure>
{
    private readonly IAggregateRepository<Domain.Procedure> _repository;
    
    public SortProcedureHandler(ILogger<SortProcedureHandler> logger
        , IAggregateRepository<Domain.Procedure> repository) 
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(SortProcedure command, CancellationToken stoppageToken)
    {
        await _repository.Sort(command.Id, command.From, command.To, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}