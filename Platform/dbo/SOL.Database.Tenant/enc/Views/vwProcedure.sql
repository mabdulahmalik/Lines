CREATE VIEW [enc].[vw_Procedure]
    AS
SELECT p.[Id]
      ,p.[CreatedAt]
      ,p.[ModifiedAt]
      ,p.[Archived]
      ,p.[Order]
      ,p.[Name]
      ,p.[Settings]
      ,p.[RequiredData]
      ,(cnt.[Encounters] + cnt.[RoutineOrigins] + cnt.[RoutineTermi]) as [References]
  FROM 
    [enc].[Procedure] p INNER JOIN
    (SELECT
        pr.[Id],
        (SELECT COUNT(*) FROM [enc].[EncounterProcedure] ep WHERE ep.[ProcedureId] = pr.[Id]) as [Encounters],
        (SELECT COUNT(*) FROM [org].[RoutineOrigin] WHERE [ProcedureId] = pr.[Id]) as [RoutineOrigins],
        (SELECT COUNT(*) FROM [org].[RoutineTerminus] WHERE [ProcedureId] = pr.[Id]) as [RoutineTermi]
    FROM
        [enc].[Procedure] pr) cnt ON(p.[Id] = cnt.[Id])
GO