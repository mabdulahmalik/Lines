CREATE VIEW [enc].[vw_JobRoutine]
  AS
SELECT 
     jr.[Timestamp]
    ,jr.[JobId]
    ,ep.[EncounterId]
    ,jr.[EncounterProcedureId]
    ,fr.[RoutineId]
    ,jr.[FacilityRoutineId]
  FROM 
    [enc].[JobRoutine] jr
    INNER JOIN [org].[FacilityRoutine] fr ON(jr.[FacilityRoutineId] = fr.[Id])
    INNER JOIN [enc].[EncounterProcedure] ep ON(jr.[EncounterProcedureId] = ep.[Id])
GO