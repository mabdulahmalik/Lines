using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class CompleteEncounterCmdType : InputObjectType<CompleteEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<CompleteEncounter> descriptor)
    {
        descriptor
            .Name("CompleteEncounterCmd")
            .Description("The Command that COMPLETES the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("id")
            .Description("The unique identifier of the Encounter.");        
    }
}