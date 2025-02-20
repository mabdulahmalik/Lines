using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ClearLineInfectionCmdType : InputObjectType<ClearLineInfection>
{
    protected override void Configure(IInputObjectTypeDescriptor<ClearLineInfection> descriptor)
    {
        descriptor
            .Name("ClearLineInfectionCmd")
            .Description("The Command that CLEARS a Line Infection.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Line.");        
    }
}