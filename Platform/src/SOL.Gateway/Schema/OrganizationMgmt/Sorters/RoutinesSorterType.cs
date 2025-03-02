using HotChocolate.Data.Sorting;
using SOL.Gateway.Views.OrganizationMgmt.Routine;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutinesSorterType : SortInputType<RoutineView>
{
    protected override void Configure(ISortInputTypeDescriptor<RoutineView> descriptor)
    {
        descriptor
            .Name("RoutinesSorter")
            .Description("Sorts the Routine Query Results.");
        
        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The Name of the Routine.");

        descriptor
            .Field(x => x.Description)
            .Name("description")
            .Description("The Description of the Routine.");

        descriptor
            .Field(x => x.Active)
            .Name("isActive")
            .Description("Whether the Routine is active.");
        
        descriptor
            .Field(x => x.AssignmentCount)
            .Name("assignmentCount")
            .Description("The number of Assignments to Facilities.");        

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date and time the Routine was Created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date and time the Routine was last Modified.");        
    }
}