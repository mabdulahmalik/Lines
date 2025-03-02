using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Procedure.Domain;

namespace SOL.Service.PatientEncounter.Procedure.Orchestration.Commands;

public class CreateProcedureHandler : CommandHandler<CreateProcedure>
{
    private readonly IAggregateRepository<Domain.Procedure> _procedureRepository;

    public CreateProcedureHandler(ILogger<CreateProcedureHandler> logger, IAggregateRepository<Domain.Procedure> procedureRepository) 
        : base(logger)
    {
        _procedureRepository = procedureRepository;
    } 

    protected override async Task HandleAsync(CreateProcedure command, CancellationToken stoppageToken)
    {
        var fields = command.Fields.Select(field => 
        {
            var fieldOptions = field.Options?.Select(o => new FieldOption(o)).ToArray();            
            var procedureField = new Field(field.Name, field.Type, field.Instruction);
            procedureField.IsRequired(field.Required);
            
            if(field.Type == ProcedureFieldType.List) {
                procedureField.AllowMultiSelect(field.AllowMultiSelect);

                if (fieldOptions != null) {
                    procedureField.AddOptions(fieldOptions);
                }
            }

            return procedureField;
        });

        RequiredPatientData requiredData = default;
        command.RequiredData.ForEach(data => {
            requiredData |= data;
        });

        var procedure = Domain.Procedure.Create(command.Name, command.Type, fields
            , requiredData, command.EnablePerformanceReporting);

        await _procedureRepository.Add(procedure, stoppageToken);
        await _procedureRepository.Commit(stoppageToken);
    }
}