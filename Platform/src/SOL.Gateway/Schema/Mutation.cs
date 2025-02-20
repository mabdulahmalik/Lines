namespace SOL.Gateway.Schema;

public class Mutation : ObjectType
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        base.Configure(descriptor);
    }
}

public class MutationResponseType : ObjectType<MutationResponse>
{
    protected override void Configure(IObjectTypeDescriptor<MutationResponse> descriptor)
    {
        descriptor
            .Name("MutationResponse")
            .Description("The response from a mutation.");

        descriptor
            .Field(t => t.CorrelationId)
            .Name("correlationId")
            .Description("The identifier of the operation being performed.");
    }
}

public class MutationResponse
{
    public Guid CorrelationId { get; set; }
}