using Microsoft.Extensions.Logging;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Encounter.Domain;

namespace SOL.Service.PatientEncounter.Encounter.Orchestration.Commands;

public class ModifyProcedureForEncounterHandler : CommandHandler<ModifyProcedureForEncounter>
{
    private readonly IAggregateRepository<Domain.Encounter> _repository;

    public ModifyProcedureForEncounterHandler(ILogger<ModifyProcedureForEncounterHandler> logger
        , IAggregateRepository<Domain.Encounter> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyProcedureForEncounter command, CancellationToken stoppageToken)
    {
        var encounter = await _repository.Get(command.EncounterId, stoppageToken);
        var procedureValues = command.Values
            .Select(x => new ProcedureValue(x.FieldId, x.Value))
            .ToArray();
        
        encounter.ModifyProcedure(command.Id, procedureValues);
        
        await _repository.Update(encounter, stoppageToken);
        await _repository.Commit(stoppageToken);
    }
}