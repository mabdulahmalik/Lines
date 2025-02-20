CREATE VIEW [enc].[vw_MedicalRecordObservation]
AS
SELECT 
    [MedicalRecordId],
    [ObjectId],
    [Type],
    [Timestamp],
    (SELECT [Text], [AuthorId]
        FROM [enc].[JobNote] WHERE [Id] = [ObjectId]
        FOR JSON PATH) as [Data]
FROM 
    [enc].[MedicalRecordObservation]
WHERE
    [Type] = 1
UNION ALL
SELECT 
    [MedicalRecordId],
    [ObjectId],
    [Type],
    [Timestamp],
    (SELECT CONCAT([Id], '.', [ContentType]) as [Photo], '' as [Caption]
        FROM [enc].[EncounterPhoto] WHERE [Id] = [ObjectId]
        FOR JSON PATH) as [Data]
FROM 
    [enc].[MedicalRecordObservation]
WHERE
    [Type] = 2