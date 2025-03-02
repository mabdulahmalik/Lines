using HotChocolate.Data;
using HotChocolate.Data.Filters;
using SOL.Abstractions.Domain;
using ProcedureFieldType = SOL.Abstractions.Domain.ProcedureFieldType;

namespace SOL.Gateway.Conventions;

public class EnumFilterConvention : FilterConvention
{
    protected override void Configure(IFilterConventionDescriptor descriptor)
    {
        descriptor.AddDefaults();
        descriptor.BindRuntimeType<DurationUnit, EnumOperationFilterInputType<DurationUnit>>();
        descriptor.BindRuntimeType<RequiredPatientData, EnumOperationFilterInputType<RequiredPatientData>>();
        
        descriptor.BindRuntimeType<EncounterAssigneePosition, EnumOperationFilterInputType<EncounterAssigneePosition>>();
        descriptor.BindRuntimeType<EncounterPriority, EnumOperationFilterInputType<EncounterPriority>>();
        descriptor.BindRuntimeType<EncounterStage, EnumOperationFilterInputType<EncounterStage>>();
        descriptor.BindRuntimeType<JobStatus, EnumOperationFilterInputType<JobStatus>>();
        descriptor.BindRuntimeType<LineDwelling, EnumOperationFilterInputType<LineDwelling>>();
        descriptor.BindRuntimeType<ProcedureFieldSetting, EnumOperationFilterInputType<ProcedureFieldSetting>>();
        descriptor.BindRuntimeType<ProcedureFieldType, EnumOperationFilterInputType<ProcedureFieldType>>();
        descriptor.BindRuntimeType<ProcedureSetting, EnumOperationFilterInputType<ProcedureSetting>>();
    }
}