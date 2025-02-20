using HotChocolate.Data.Filters;
using SOL.Service.PatientEncounter.Line.View;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LinesFilterType : FilterInputType<LineView>
{
    protected override void Configure(IFilterInputTypeDescriptor<LineView> descriptor)
    {
        descriptor
            .Name("LinesFilter")
            .Description("Filters for the Line Query.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Line.");

        descriptor
            .Field(x => x.Name)
            .Name("name")
            .Description("The name of the Line.");

        descriptor
            .Field(x => x.Type)
            .Name("type")
            .Description("The type of the Line.");
        
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
            .Field(x => x.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The Medical Record associated with the Line.");

        descriptor
            .Field(t => t.Location)
            .Type<LocationFilterType>()
            .Name("location")
            .Description("The Location of the Line.");   
        
        descriptor
            .Field(t => t.FollowUp)
            .Type<LineFollowUpFilterType>()
            .Name("followUp")
            .Description("The Follow Up details of the Line.");        
    }
}