CREATE VIEW [dbo].[vw_ReportSourceResponseTime]
  AS
SELECT 
    ea.[ClinicianId],
    p.[Id] as [ProcedureId],
    p.[Name] as [ProcedureName],
    ep.[EncounterId],
    DATEDIFF(mi, e.[CreatedAt], es.[Attending]) as [EncounterResponseTime],
    es.[Completed] as [EncounterCompletedOn],    
    r.[Id] as [RoomId],
    r.[Name] as [RoomName],
    f.[Id] as [FacilityId],
    f.[Name] as [FacilityName],
    j.[OrderingProvider]
FROM 
    [enc].[EncounterAssignment] ea
    INNER JOIN [enc].[Encounter] e ON(e.[Id] = ea.[EncounterId])
    INNER JOIN [enc].[vw_EncounterProgress] es ON(es.[EncounterId] = e.[Id])
    INNER JOIN [enc].[EncounterProcedure] ep ON(ep.[EncounterId] = e.[Id]) 
    INNER JOIN [enc].[Procedure] p ON(ep.[ProcedureId] = p.[Id])
    INNER JOIN [enc].[Job] j ON(e.[JobId] = j.[Id]) 
    INNER JOIN [org].[FacilityRoom] r ON(r.[Id] = e.[RoomId])
    INNER JOIN [org].[Facility] f ON(r.[FacilityId] = f.[Id])
WHERE
    (p.[Settings] & 128) <> 0
    AND ea.[WithdrawnAt] IS NULL
    AND es.[Completed] IS NOT NULL
GO