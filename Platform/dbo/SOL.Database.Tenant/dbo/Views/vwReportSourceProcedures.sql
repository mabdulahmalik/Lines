CREATE VIEW [dbo].[vw_ReportSourceProcedures]
  AS
SELECT 
    p.[Id] as [ProcedureId],
    p.[Name] as [ProcedureName],
    DATEDIFF(mi, es.[Attending], ep.[PerformedAt]) as [ProcedureDuration],
    ep.[PerformedBy],
    ep.[PerformedAt],
    ep.[EncounterId],
    es.[Completed] as [EncounterCompletedOn],
    r.[Id] as [RoomId],
    r.[Name] as [RoomName],
    f.[Id] as [FacilityId],
    f.[Name] as [FacilityName],
    j.[OrderingProvider],
    ISNULL(s.[Successful], 1) as [Successful]
FROM 
    [enc].[EncounterProcedure] ep 
    INNER JOIN [enc].[Procedure] p ON(ep.[ProcedureId] = p.[Id]) 
    INNER JOIN [enc].[Encounter] e ON(e.[Id] = ep.[EncounterId]) 
    INNER JOIN [enc].[vw_EncounterProgress] es ON(es.[EncounterId] = e.[Id])
    INNER JOIN [enc].[Job] j ON(e.[JobId] = j.[Id]) 
    INNER JOIN [org].[FacilityRoom] r ON(r.[Id] = e.[RoomId])
    INNER JOIN [org].[Facility] f ON(r.[FacilityId] = f.[Id])
    LEFT OUTER JOIN (
        SELECT epv.[EncounterProcedureId] as [Id],(CASE epv.[Value] WHEN 'off' THEN 1 ELSE 0 END) as [Successful]
        FROM [enc].[ProcedureField] pf INNER JOIN [enc].[EncounterProcedureValue] epv ON(pf.[Id] = epv.[ProcedureFieldId])
        WHERE [Type] = 3 AND [Name] = 'Unsuccessful'        
    ) s ON(s.[Id] = ep.[Id])
WHERE
    (p.[Settings] & 128) <> 0
    AND es.[Completed] IS NOT NULL
GO