using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.PatientEncounter;

public class EncounterPriorityType : EnumType<EncounterPriority>
{
    protected override void Configure(IEnumTypeDescriptor<EncounterPriority> descriptor)
    {
        descriptor
            .Name("EncounterPriority")
            .Description("The priority of the Encounter.");

        descriptor
            .Value(EncounterPriority.Stat)
            .Name("STAT")
            .Description("The encounter is URGENT, highest priority.");

        descriptor
            .Value(EncounterPriority.Normal)
            .Name("NORMAL")
            .Description("The encounter has a normal/medium priority.");

        descriptor
            .Value(EncounterPriority.Routine)
            .Name("ROUTINE")
            .Description("The encounter has a low priority.");
    }
}