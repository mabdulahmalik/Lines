using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class DeleteProcedureCmdType : InputObjectType<DeleteProcedure>
{
    protected override void Configure(IInputObjectTypeDescriptor<DeleteProcedure> descriptor)
    {
        descriptor
            .Name("DeleteProcedureCmd")
            .Description("Deletes a Procedure by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure.");        
    }
}