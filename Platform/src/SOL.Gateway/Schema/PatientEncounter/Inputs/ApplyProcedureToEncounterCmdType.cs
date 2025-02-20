using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class ApplyProcedureToEncounterCmdType : InputObjectType<ApplyProcedureToEncounter>
{
    protected override void Configure(IInputObjectTypeDescriptor<ApplyProcedureToEncounter> descriptor)
    {
        descriptor
            .Name("ApplyProcedureToEncounterCmd")
            .Description("The Command that applies a Procedure to the Encounter.");

        descriptor
            .Field(t => t.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the Encounter.");

        descriptor
            .Field(t => t.ProcedureId)
            .Name("procedureId")
            .Description("The unique identifier of the Procedure.");

        descriptor
            .Field(t => t.Values)
            .Type<ListType<EncounterProcedureValuePrmType>>()
            .Name("values")
            .Description("The list of Procedure Values.");
        
        descriptor
            .Field(t => t.RoutinesAssigned)
            .Name("routinesAssigned")
            .Description("The a List of unique identifiers of the Routine Assignment(s) that should be ADDED. This is only necessary when the Procedure triggers a follow up.");
        
        descriptor
            .Field(t => t.RoutinesRemoved)
            .Name("routinesRemoved")
            .Description("The a List of the unique identifiers of the Routine Assignments(s) that should be REMOVED. This is only necessary when the Procedure triggers a follow up.");        
        
        descriptor
            .Field(t => t.RemovedLineId)
            .Name("removedLineId")
            .Description("The unique identifier of the existing Line being removed. This is only necessary when the Procedure is a Removal.");        
        
        descriptor
            .Field(t => t.InsertedLineName)
            .Name("insertedLineName")
            .Description("The Name of the Line being inserted by this Procedure. This is only necessary when the Procedure is an Insertion.");        
    }
}
