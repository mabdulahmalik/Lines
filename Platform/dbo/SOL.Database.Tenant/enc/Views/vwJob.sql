CREATE VIEW [enc].[vw_Job]
  AS
SELECT
    j.[Id],
    j.[CreatedAt],
    j.[ModifiedAt],
    j.[DeletedAt],
    j.[PurposeId],
    r.[FacilityId],
    j.[RoomId],
    r.[Name] as [Room],
    j.[LineId],
    j.[MedicalRecordId],
    ep.[Id] as [OriginEncounterProcedureId],
    ep.[EncounterId] as [OriginEncounterId],
    (SELECT TOP 1
        [Timestamp]
    FROM
        [enc].[JobStatus]
    WHERE
        [JobId] = j.[Id]
    ORDER BY
        [Timestamp] DESC) as [StatusChangedAt],
    (SELECT TOP 1
        [Status]
    FROM
        [enc].[JobStatus]
    WHERE
        [JobId] = j.[Id]
    ORDER BY
        [Timestamp] DESC) as [Status],
    j.[ScheduledDate],
    j.[ScheduledTime],        
    j.[OrderingProvider],
    j.[Contact]
FROM 
    [enc].[Job] j 
    INNER JOIN [org].[FacilityRoom] r ON(j.[RoomId] = r.[Id])
    LEFT OUTER JOIN [enc].[EncounterProcedure] ep ON(j.[OriginProcedureId] = ep.[Id])
GO