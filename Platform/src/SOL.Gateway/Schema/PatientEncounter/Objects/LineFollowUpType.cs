using SOL.Gateway.Views.PatientEncounter.Line;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineFollowUpType : ObjectType<LineFollowUpView>
{
    protected override void Configure(IObjectTypeDescriptor<LineFollowUpView> descriptor)
    {
        descriptor
            .Name("LineFollowUp")
            .Description("The Follow Up details for a Line.");
        
        descriptor
            .Field(x => x.JobId)
            .Name("jobId")
            .Description("The unique identifier of the Job.");
        
        descriptor
            .Field(x => x.PurposeId)
            .Name("purposeId")
            .Description("The unique identifier of the Purpose.");
        
        descriptor
            .Field(x => x.Date)
            .Name("date")
            .Description("The DATE of the scheduled Follow Up.");        
    }
}