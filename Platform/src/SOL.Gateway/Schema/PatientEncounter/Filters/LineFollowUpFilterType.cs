using HotChocolate.Data.Filters;
using SOL.Service.PatientEncounter.Line.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineFollowUpFilterType : FilterInputType<LineFollowUpView>
{
    protected override void Configure(IFilterInputTypeDescriptor<LineFollowUpView> descriptor)
    {
        descriptor
            .Name("LineFollowUpFilter")
            .Description("Filters for the Line Follow Up Query.");

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

