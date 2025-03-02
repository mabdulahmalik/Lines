using HotChocolate.Data.Filters;
using SOL.Gateway.Views.OrganizationMgmt.Routine;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RoutinesFilterType : FilterInputType<RoutineView>
{
    protected override void Configure(IFilterInputTypeDescriptor<RoutineView> descriptor)
    {
        descriptor
            .Name("RoutinesFilter")
            .Description("Filters the Routine Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Routine.");

        descriptor
            .Field(x => x.PurposeId)
            .Name("purposeId")
            .Description("The unique identifier of the Job Purpose that will be created by the Routine.");

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
            .Name("active")
            .Description("Whether the Routine is active.");
        
        descriptor
            .Field(x => x.FollowUp)
            .Name("followUp")
            .Description("Whether the Routine is a Follow Up.");
        
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