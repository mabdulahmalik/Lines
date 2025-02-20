CREATE VIEW [org].[vw_Routine]
    AS
SELECT
    r.[Id],
    r.[CreatedAt],
    r.[ModifiedAt],
    r.[Active],
    r.[Name],
    r.[Description],
    r.[PurposeId],
    r.[Settings],
    (SELECT COUNT(*) FROM [org].[FacilityRoutine] fr WHERE fr.[RoutineId] = r.[Id]) as [AssignmentCount]
FROM
    [org].[Routine] r