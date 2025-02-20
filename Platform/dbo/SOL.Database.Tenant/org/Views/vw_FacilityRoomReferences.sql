CREATE VIEW [org].[vw_FacilityRoomReferences]
    AS
SELECT [FacilityId]
      ,r.[Id]
      ,r.[Name]
      ,(SELECT COUNT(*) FROM [enc].[Job] WHERE [RoomId] = r.[Id]) as [Jobs]
      ,(SELECT COUNT(*) FROM [enc].[Encounter] WHERE [RoomId] = r.[Id]) as [Encounters]
      ,(SELECT COUNT(*) FROM [enc].[LineLocation] WHERE [RoomId] = r.[Id]) as [Lines]
      ,(SELECT COUNT(*) FROM [enc].[MedicalRecord] WHERE [FacilityId] = f.[Id]) as [MedicalRecords]
  FROM [org].[FacilityRoom] r INNER JOIN [org].[Facility] f ON(r.[FacilityId] = f.[Id])
GO