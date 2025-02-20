using SOL.Abstractions.Domain;

namespace SOL.Gateway.Schema.Common;

public class RequiredPatientDataType : EnumType<RequiredPatientData>
{
    protected override void Configure(IEnumTypeDescriptor<RequiredPatientData> descriptor)
    {
        descriptor
            .Name("RequiredPatientData")
            .Description("The required Patient data fields.");

        descriptor
            .Value(RequiredPatientData.MRN)
            .Name("MRN")
            .Description("The Medical Record Number of the patient.");

        descriptor
            .Value(RequiredPatientData.FirstName)
            .Name("FIRST_NAME")
            .Description("The FIRST name of the patient.");

        descriptor
            .Value(RequiredPatientData.LastName)
            .Name("LAST_NAME")
            .Description("The LAST name of the patient.");

        descriptor
            .Value(RequiredPatientData.DateOfBirth)
            .Name("DATE_OF_BIRTH")
            .Description("The Date of Birth of the patient.");

        descriptor
            .Value(RequiredPatientData.OrderingProvider)
            .Name("ORDERING_PROVIDER")
            .Description("The provider who ordered the Procedure.");
    }
}