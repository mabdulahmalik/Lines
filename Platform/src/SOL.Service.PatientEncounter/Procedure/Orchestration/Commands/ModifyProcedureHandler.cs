using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.PatientEncounter;
using SOL.Messaging.Infrastructure;
using SOL.Service.PatientEncounter.Procedure.Domain;

namespace SOL.Service.PatientEncounter.Procedure.Orchestration.Commands;

public class ModifyProcedureHandler : CommandHandler<ModifyProcedure>
{
    private readonly IAggregateRepository<Domain.Procedure> _procedureRepository;

    public ModifyProcedureHandler(ILogger<ModifyProcedureHandler> logger, IAggregateRepository<Domain.Procedure> procedureRepository)
        : base(logger)
    {
        _procedureRepository = procedureRepository;
    }

    protected override async Task HandleAsync(ModifyProcedure command, CancellationToken stoppageToken)
    {
        var procedure = await _procedureRepository.Get(command.Id, stoppageToken);

        RequiredPatientData requiredData = default;
        command.RequiredData.ForEach(data => {
            requiredData |= data;
        });

        procedure.Modify(command.Name, requiredData
            , command.EnablePerformanceReporting, command.IsInsertion, command.IsRemoval);

        HandleFields(procedure, command);        

        _procedureRepository.Update(procedure);
        await _procedureRepository.Commit(stoppageToken);
    }

    private void HandleFields(Domain.Procedure procedure, ModifyProcedure command)
    {
        // Remove fields from domain model that are not present in the request payload
        var requestFieldIds = command.Fields.Select(f => f.Id).ToList();        
        var fieldsToRemove = procedure.Fields.Where(f => !requestFieldIds.Contains(f.Id)).Select(f => f.Id).ToArray();
        if (fieldsToRemove.Any()) procedure.RemoveFields(fieldsToRemove);

        // Add new fields to domain model
        var fieldsToAdd = command.Fields.Where(x => x.Id is null).Select(field =>
        {
            var fieldOptions = field.Options?.Select(o => new FieldOption(o.Text)).ToArray();
            var procedureField = new Field(field.Name, field.Type, field.Instruction);
            procedureField.IsRequired(field.Required);

            if (field.Type == ProcedureFieldType.List)
            {
                procedureField.AllowMultiSelect(field.AllowMultiSelect);

                if (fieldOptions != null)
                {
                    procedureField.AddOptions(fieldOptions);
                }
            }

            return procedureField;
        });

        if (fieldsToAdd.Any()) procedure.AddFields(fieldsToAdd.ToArray());
        

        // Update existing fields
        foreach (var modifyField in command.Fields.Where(x => x.Id.HasValue))
        {
            var field = procedure.Fields.FirstOrDefault(x => x.Id == modifyField.Id);
            if (field is null)
            {
                continue;
            }

            field.Rename(modifyField.Name);
            field.IsArchived(modifyField.Archived);
            field.IsRequired(modifyField.Required);
            field.ModifyInstruction(modifyField.Instruction);

            if (field.Type == ProcedureFieldType.List)
            {
                field.AllowMultiSelect(modifyField.AllowMultiSelect);

                // Remove options
                var optionIds = modifyField.Options?.Select(x => x.Id);
                var optionsToRemove = field.Options.Where(x => !optionIds.Contains(x.Id)).Select(x => x.Id).ToArray();
                field.RemoveOptions(optionsToRemove);

                // Add new options
                var optionsToAdd = modifyField.Options?
                    .Where(x => !x.Id.HasValue)
                    .Select(o => new FieldOption(o.Text))
                    .ToArray();

                field.AddOptions(optionsToAdd);

                // Update options
                foreach (var modifyOption in modifyField.Options.Where(x => x.Id.HasValue))
                {
                    var fieldOpt = field.Options.Single(x => x.Id == modifyOption.Id);
                    fieldOpt.IsArchived(modifyOption.Archived);
                    fieldOpt.ChangeText(modifyOption.Text);
                }
            }
        }
    }
}