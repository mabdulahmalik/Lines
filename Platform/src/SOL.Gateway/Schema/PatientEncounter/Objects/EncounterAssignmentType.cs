using SOL.Gateway.Views.PatientEncounter.Encounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterAssignmentType : ObjectType<EncounterAssignmentView>
{
    protected override void Configure(IObjectTypeDescriptor<EncounterAssignmentView> descriptor)
    {
        descriptor
            .Name("EncounterAssignment")
            .Description("The clinician assigned to the Encounter.");

        descriptor
            .Field(t => t.ClinicianId)
            .Name("clinicianId")
            .Description("The unique identifier for the clinician.");

        descriptor
            .Field(t => t.Position)
            .Type<EncounterAssigneePositionType>()
            .Name("position")
            .Description("The position of the clinician.");

        descriptor
            .Field(t => t.AssignedAt)
            .Name("assignedAt")
            .Description("The date and time the clinician was assigned to the encounter.");
    }
}