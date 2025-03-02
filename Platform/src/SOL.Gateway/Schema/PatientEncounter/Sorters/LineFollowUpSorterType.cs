using HotChocolate.Data.Sorting;
using SOL.Gateway.Views.PatientEncounter.Line;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineFollowUpSorterType : SortInputType<LineFollowUpView>
{
    protected override void Configure(ISortInputTypeDescriptor<LineFollowUpView> descriptor)
    {
        descriptor
            .Name("LineFollowUpSorter")
            .Description("Sorting the Line Follow Up Query.");
        
        descriptor
            .Field(x => x.Date)
            .Name("date")
            .Description("The DATE of the scheduled Follow Up.");
    }
}