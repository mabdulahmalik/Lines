using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.Common;

public class AddressType : ObjectType<Address>
{
    protected override void Configure(IObjectTypeDescriptor<Address> descriptor)
    {
        descriptor
            .Name("Address")
            .Description("A physical location based in the USA.");

        descriptor
            .Field(x => x.Street)
            .Name("street")
            .Description("The Street name, suite #, etc.");

        descriptor
            .Field(x => x.City)
            .Name("city")
            .Description("The name of the City.");

        descriptor
            .Field(x => x.State)
            .Name("state")
            .Description("The State abbreviation.");

        descriptor
            .Field(x => x.ZipCode)
            .Name("zipCode")
            .Description("The 5 or 9 digit Zip Code.");
    }
}