CREATE VIEW [enc].[vw_ProcedureField]
    AS
SELECT p.[Id]
      ,p.[ProcedureId]
      ,p.[Archived]
      ,p.[Order]
      ,p.[Type]
      ,p.[Settings]
      ,p.[Name]
      ,p.[Instruction]
      ,(cnt.[Encounters] + cnt.[RoutineOrigins] + cnt.[RoutineTermi]) as [References]
  FROM 
    [enc].[ProcedureField] p INNER JOIN
    (SELECT
        pf.[Id],
        (SELECT COUNT(*) FROM [enc].[EncounterProcedureValue] WHERE [ProcedureFieldId] = pf.[Id]) as [Encounters],
        (SELECT COUNT(*) FROM [org].[RoutineOrigin] WHERE [PropertyId] = pf.[Id]) as [RoutineOrigins],
        (SELECT COUNT(*) FROM [org].[RoutineTerminus] WHERE [PropertyId] = pf.[Id]) as [RoutineTermi]
    FROM
        [enc].[ProcedureField] pf) cnt ON(p.[Id] = cnt.[Id])
GO