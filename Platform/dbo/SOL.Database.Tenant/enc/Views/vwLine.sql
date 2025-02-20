CREATE VIEW [enc].[vw_Line]
  AS
SELECT
    l.[Id]
    ,l.[CreatedAt]
    ,l.[ModifiedAt]
    ,l.[MedicalRecordId]
    ,r.[Id] as [RoomId]
    ,r.[FacilityId]
    ,l.[Name]
    ,l.[Type]
    ,CONVERT(TINYINT, (CASE
        WHEN l.[InsertedOn] IS NULL THEN 0
        WHEN l.[RemovedOn] IS NULL THEN 1
        ELSE 2 
    END)) as [Dwelling]
    ,CONVERT(DATE, l.[InsertedOn]) as [InsertedOn]
    ,epi.[EncounterId] as [InsertedWithEncounterId]
    ,epi.[Id] as [InsertedWithEncounterProcedureId]
    ,CONVERT(DATE, l.[RemovedOn]) as [RemovedOn]
    ,epr.[EncounterId] as [RemovedWithEncounterId]
    ,epr.[Id] as [RemovedWithEncounterProcedureId]    
    ,(CASE l.[InsertedOn]
        WHEN NULL THEN NULL
        ELSE DATEDIFF(DAY, l.[InsertedOn], ISNULL(l.[RemovedOn], SYSUTCDATETIME())) + 1 
    END) as [DwellTime]    
    ,j.[ScheduledDate] as [FollowUpDate]
    ,j.[PurposeId] as [FollowUpPurposeId]
    ,j.[Id] as [FollowUpJobId]
    ,l.[InfectedOn]
    ,l.[ExternallyPlaced]
    ,l.[ExternallyPlacedBy]
FROM
    [enc].[Line] l
INNER JOIN
    (SELECT
        [LineId] as [Id], MAX([Timestamp]) as [TS]
    FROM
        [enc].[LineLocation]
    GROUP BY
        [LineId]) as [lr] ON(l.[Id] = lr.[Id])
INNER JOIN
    [enc].[LineLocation] ll ON(lr.[Id] = ll.[LineId] AND lr.[TS] = ll.[Timestamp])
INNER JOIN
    [org].[FacilityRoom] r ON(ll.[RoomId] = r.[Id])
LEFT OUTER JOIN
    [enc].[Job] j ON(l.[FollowUpId] = j.[Id])
LEFT OUTER JOIN
    [enc].[EncounterProcedure] epi ON(l.[InsertedWith] = epi.[Id])
LEFT OUTER JOIN
    [enc].[EncounterProcedure] epr ON(l.[RemovedWith] = epr.[Id])

GO