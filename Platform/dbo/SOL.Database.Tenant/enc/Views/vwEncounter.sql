CREATE VIEW [enc].[vw_Encounter]
  AS 
SELECT
    e.[Id],
    e.[JobId],
    e.[PurposeId],
    e.[MedicalRecordId],
    e.[RoomId],
    (SELECT
        r.[FacilityId]
    FROM
        [org].[FacilityRoom] r
    WHERE
        r.[Id] = e.[RoomId]) as [FacilityId],
    e.[CreatedAt],
    e.[ModifiedAt],
    j.[DeletedAt],
    (SELECT TOP 1
        [Stage]
    FROM
        [enc].[EncounterStage]
    WHERE
        [EncounterId] = e.[Id]
    ORDER BY
        [Timestamp] DESC) as [Stage],
    (SELECT TOP 1
        [Priority]
    FROM
        [enc].[EncounterPriority]
    WHERE
        [EncounterId] = e.[Id]
    ORDER BY
        [Timestamp] DESC) as [Priority]        
FROM 
    [enc].[Encounter] as e INNER JOIN [enc].[Job] as j ON(e.[JobId] = j.[Id])