using Microsoft.Extensions.Logging;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;
using SOL.Messaging.Contracts.OrganizationMgmt;
using SOL.Messaging.Infrastructure;
using SOL.Service.OrganizationMgmt.Facility.Domain;
using Domain_RoutineAssignment = SOL.Service.OrganizationMgmt.Facility.Domain.RoutineAssignment;
using Facility_Domain_RoutineAssignment = SOL.Service.OrganizationMgmt.Facility.Domain.RoutineAssignment;
using OrganizationMgmt_Facility_Domain_RoutineAssignment = SOL.Service.OrganizationMgmt.Facility.Domain.RoutineAssignment;
using RoutineAssignment = SOL.Service.OrganizationMgmt.Facility.Domain.RoutineAssignment;

namespace SOL.Service.OrganizationMgmt.Facility.Orchestration.Commands;

public class ModifyFacilityHandler : CommandHandler<ModifyFacility>
{
    private readonly IAggregateRepository<Domain.Facility> _repository;

    public ModifyFacilityHandler(ILogger<ModifyFacilityHandler> logger, IAggregateRepository<Domain.Facility> repository)
        : base(logger)
    {
        _repository = repository;
    }

    protected override async Task HandleAsync(ModifyFacility command, CancellationToken stoppageToken)
    {
        var facility = await _repository.Get(command.Id, stoppageToken);

        RequiredPatientData requiredData = default;
        command.RequiredData.ForEach(data => {
            requiredData |= data;
        });        

        facility.Modify(command.Name, command.TimeZone, command.Address, requiredData);
        facility.ExcludePurpose([.. command.PurposeIds]);
        facility.ExcludeProcedure([.. command.ProcedureIds]);

        HandleAssignments(command, facility);

        _repository.Update(facility);
        await _repository.Commit(stoppageToken);
    }

    private void HandleAssignments(ModifyFacility command, Domain.Facility facility)
    {
        var existingAssignment = facility.Routines.Select(x => x.Id).ToList();

        // Remove Existing Assignments
        var assignmentsToRemove = existingAssignment.Where(id => !command.Assignments.Any(x => x.Id == id)).ToArray();
        if (assignmentsToRemove.Any()) facility.RemoveRoutine(assignmentsToRemove);


        // Add New Assignments
        var assignmentsToAdd = command.Assignments
            .Where(x => x.Id is null)
            .Select(x =>
            {
                var specs = x.Specs.Select(s => new AssignmentSpec(s.PropertyId, s.PropertyValue)).ToArray();
                var assignment = new OrganizationMgmt_Facility_Domain_RoutineAssignment(x.RoutineId, x.Name, specs);
                return assignment;
            })
            .ToArray();

        if (assignmentsToAdd.Any()) facility.AssignRoutine(assignmentsToAdd);


        // Modify Existing Assignments
        var assignmentsToModify = command.Assignments.Where(x => x.Id is not null).ToList();
        assignmentsToModify.ForEach(x =>
        {
            var assignment = facility.Routines.FirstOrDefault(x => x.Id == x.Id);

            if (assignment is null)
            {
                return;
            }

            assignment.Rename(x.Name);

            x.Specs.ForEach(s => assignment[s.PropertyId] = s.PropertyValue);
        });
    }
}