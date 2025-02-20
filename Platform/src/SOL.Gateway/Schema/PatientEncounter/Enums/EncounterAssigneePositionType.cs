using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAssigneePositionType : EnumType<EncounterAssigneePosition>
{
    protected override void Configure(IEnumTypeDescriptor<EncounterAssigneePosition> descriptor)
    {
        descriptor
            .Name("EncounterAssigneePosition")
            .Description("The position of the assignee to a specific Encounter.");

        descriptor
            .Value(EncounterAssigneePosition.Primary)
            .Name("PRIMARY")
            .Description("The assignee is the primary clinician.");

        descriptor
            .Value(EncounterAssigneePosition.Assistant)
            .Name("ASSISTANT")
            .Description("The assignee is a secondary/supporting clinician.");

        descriptor
            .Value(EncounterAssigneePosition.Trainee)
            .Name("TRAINEE")
            .Description("The assignee is in training.");
    }
}