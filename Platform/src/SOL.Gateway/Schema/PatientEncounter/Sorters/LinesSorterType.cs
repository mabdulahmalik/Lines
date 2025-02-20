using HotChocolate.Data.Sorting;
using SOL.Service.PatientEncounter.Line.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LinesSorterType : SortInputType<LineView>
{
    protected override void Configure(ISortInputTypeDescriptor<LineView> descriptor)
    {
        descriptor
            .Name("LinesSorter")
            .Description("Sorting the Lines Query.");
        
        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Line.");

        descriptor
            .Field(x => x.Type)
            .Name("type")
            .Description("The type of the Line.");

        descriptor
            .Field(x => x.Dwelling)
            .Name("dwelling")
            .Description("The Dwelling of the Line.");  

        descriptor
            .Field(x => x.InsertedOn)
            .Name("insertedOn")
            .Description("The date the Line was inserted.");
        
        descriptor
            .Field(x => x.RemovedOn)
            .Name("removedOn")
            .Description("The date the Line was removed.");
        
        descriptor
            .Field(x => x.DwellTime)
            .Name("dwellTime")
            .Description("The amount of time (in Days) the Line was in the patient.");
        
        descriptor
            .Field(x => x.FollowUp)
            .Type<LineFollowUpSorterType>()
            .Name("followUp")
            .Description("The date the Line needs to have a Follow Up.");

        descriptor
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date the Line was created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date the Line was last modified.");

        descriptor
            .Field(x => x.InfectedOn)
            .Name("hasInfection")
            .Description("Whether the Line has an infection.");

        descriptor
            .Field(x => x.ExternallyPlaced)
            .Name("externallyPlaced")
            .Description("Whether the line is externally placed.");
        
        descriptor
            .Field(x => x.ExternallyPlacedBy)
            .Name("externallyPlacedBy")
            .Description("The Name of the person/entity who placed the Line externally.");
    }
}