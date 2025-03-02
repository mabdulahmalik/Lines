using SOL.Gateway.Views.PatientEncounter.Line;

namespace SOL.Gateway.Schema.PatientEncounter;

public class LineType : ObjectType<LineView>
{
    protected override void Configure(IObjectTypeDescriptor<LineView> descriptor)
    {
        descriptor
            .Name("Line")
            .Description("A Central Line.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Line.")
            .IsProjected();

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
            .Field(x => x.CreatedAt)
            .Name("createdAt")
            .Description("The date the Line was created.");

        descriptor
            .Field(x => x.ModifiedAt)
            .Name("modifiedAt")
            .Description("The date the Line was last modified.");

        descriptor
            .Field(x => x.InfectedOn)
            .Name("infectedOn")
            .Description("Whether the date the Line was infected.");

        descriptor
            .Field(x => x.ExternallyPlaced)
            .Name("externallyPlaced")
            .Description("Whether the line is externally placed.");
        
        descriptor
            .Field(x => x.ExternallyPlacedBy)
            .Name("externallyPlacedBy")
            .Description("The Name of the person/entity who placed the Line externally.");

        descriptor
            .Field(x => x.MedicalRecordId)
            .Name("medicalRecordId")
            .Description("The Medical Record associated with the Line.");

        descriptor
            .Field(t => t.FollowUp)
            .Type<LineFollowUpType>()
            .Name("followUp")
            .Description("The Follow Up details the Line.");        
        
        descriptor
            .Field(t => t.Location)
            .Type<LocationType>()
            .Name("location")
            .Description("The Location of the Line.");

        descriptor
            .Field(x => x.InsertedWith)
            .Type<LineProcedureType>()
            .Name("insertedWith")
            .Description("The Encounter and Procedure used to INSERT the Line.");
        
        descriptor
            .Field(x => x.RemovedWith)
            .Type<LineProcedureType>()
            .Name("removedWith")
            .Description("The Encounter and Procedure used to REMOVE the Line.");        
    }
}