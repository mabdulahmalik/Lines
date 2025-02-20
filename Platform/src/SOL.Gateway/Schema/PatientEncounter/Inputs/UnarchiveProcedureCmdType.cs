using SOL.Messaging.Contracts.PatientEncounter;

namespace SOL.Gateway.Schema.PatientEncounter;

public class UnarchiveProcedureCmdType : InputObjectType<UnarchiveProcedure>
{
    protected override void Configure(IInputObjectTypeDescriptor<UnarchiveProcedure> descriptor)
    {
        descriptor
            .Name("UnarchiveProcedureCmd")
            .Description("Restores (from Archive) a Procedure by its unique identifier.");

        descriptor
            .Field(x => x.Id)
            .Name("id")
            .Description("The unique identifier of the Procedure.");
    }
}