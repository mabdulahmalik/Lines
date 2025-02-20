using SOL.Service.PatientEncounter.Line.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineProcedureType : ObjectType<LineProcedureView>
{
    protected override void Configure(IObjectTypeDescriptor<LineProcedureView> descriptor)
    {
        descriptor
            .Name("LineProcedure")
            .Description("An applied Procedure that implicitly carries more weight because it indicates more (ie. triggered a follow up, is a Line insertion, etc).");
        
        descriptor
            .Field(x => x.EncounterId)
            .Name("encounterId")
            .Description("The unique identifier of the referenced Encounter.");
        
        descriptor
            .Field(x => x.EncounterProcedureId)
            .Name("encounterProcedureId")
            .Description("The unique identifier of the applied Procedure.");        
    }
}