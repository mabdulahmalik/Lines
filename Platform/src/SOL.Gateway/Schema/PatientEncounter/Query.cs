using SOL.Gateway.Schema.Common;
using SOL.Gateway.Views.PatientEncounter.Encounter;
using SOL.Gateway.Views.PatientEncounter.Job;
using SOL.Gateway.Views.PatientEncounter.Line;
using SOL.Gateway.Views.PatientEncounter.MedicalRecord;
using SOL.Gateway.Views.PatientEncounter.Procedure;
using SOL.Gateway.Views.PatientEncounter.Purpose;

namespace SOL.Gateway.Schema.PatientEncounter;

public class QueryJobsExtensions : ObjectTypeExtension<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field("jobs")
            .Description("Get all Jobs.")
            .UseOffsetPaging<JobType>()
            .UseProjection()
            .UseFiltering<JobsFilterType>()
            .UseSorting<JobsSorterType>()
            .ResolveWith<QueryResolver<JobView>>(x => x.Results(default!));
        
        descriptor
            .Field("encounters")
            .Description("Gets all Encounters.")
            .UseOffsetPaging<EncounterType>()
            .UseProjection()
            .UseFiltering<EncountersFilterType>()
            .UseSorting<EncountersSorterType>()
            .ResolveWith<QueryResolver<EncounterView>>(x => x.Results(default!));     
        
        descriptor
            .Field("lines")
            .Description("Gets all Central Lines.")
            .UseOffsetPaging<LineType>()
            .UseProjection()
            .UseFiltering<LinesFilterType>()
            .UseSorting<LinesSorterType>()
            .ResolveWith<QueryResolver<LineView>>(x => x.Results(default!));

        descriptor
            .Field("medicalRecords")
            .Description("Gets all Medical Records.")
            .UseOffsetPaging<MedicalRecordType>()
            .UseProjection()
            .UseFiltering<MedicalRecordsFilterType>()
            .UseSorting<MedicalRecordsSorterType>()
            .ResolveWith<QueryResolver<MedicalRecordView>>(x => x.Results(default!));

        descriptor
            .Field("purposes")
            .Description("Gets all Job Purposes.")
            .UseOffsetPaging<PurposeType>()
            .UseProjection()
            .UseFiltering<PurposesFilterType>()
            .ResolveWith<QueryResolver<PurposeView>>(x => x.OrderedResults(default!));

        descriptor
            .Field("procedures")
            .Description("Gets all Procedures.")
            .UseOffsetPaging<ProcedureType>()
            .UseProjection()
            .UseFiltering<ProceduresFilterType>()
            .ResolveWith<QueryResolver<ProcedureView>>(x => x.OrderedResults(default!));
    }
}