using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class RecordLineInfectionCmdType : InputObjectType<RecordLineInfection>
{
    protected override void Configure(IInputObjectTypeDescriptor<RecordLineInfection> descriptor)
    {
        descriptor
            .Name("RecordLineInfectionCmd")
            .Description("The Command that RECORDS a Line Infection.");

        descriptor
            .Field(t => t.Id)
            .Name("id")
            .Description("The unique identifier of the Line.");
        
        descriptor
            .Field(t => t.InfectedOn)
            .Name("infectedOn")
            .Description("The date the Line was infected.");        
    }
}