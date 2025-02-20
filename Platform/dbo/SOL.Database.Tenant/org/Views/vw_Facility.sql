CREATE VIEW [org].[vw_Facility]
    AS
SELECT [Id]
      ,[TypeId]
      ,[CreatedAt]
      ,[ModifiedAt]
      ,[Order]
      ,[Archived]
      ,[Name]
      ,[TimeZone]
      ,[RequiredData]
      ,[Street]
      ,[City]
      ,[State]
      ,[ZipCode]
      ,ISNULL((SELECT
            (SUM([Jobs]) + SUM([Encounters]) + SUM([Lines]) + SUM([MedicalRecords]))
        FROM [org].[vw_FacilityRoomReferences]
        WHERE [FacilityId] = f.[Id]
        GROUP BY [FacilityId]), 0) as [ReferenceCount]
  FROM [org].[Facility] f
  GO