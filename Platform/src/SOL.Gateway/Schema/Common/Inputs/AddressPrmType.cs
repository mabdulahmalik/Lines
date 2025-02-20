using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.Common;

public class AddressPrmType : InputObjectType<Address>
{
    protected override void Configure(IInputObjectTypeDescriptor<Address> descriptor)
    {
        descriptor
            .Name("AddressPrm")
            .Description("The 'Address' object as a Command parameter.");

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