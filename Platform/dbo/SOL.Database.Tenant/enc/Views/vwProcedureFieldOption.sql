CREATE VIEW [enc].[vw_ProcedureFieldOption]
    AS
SELECT pfo.[Id]
      ,pfo.[FieldId]
      ,[Archived]
      ,[Order]
      ,[Text]
      ,(ISNULL(epv.[Encounters], 0) + cnt.[RoutineOrigins] + cnt.[RoutineTermi])  as [References]
FROM 
    [enc].[ProcedureFieldOption] pfo INNER JOIN
    (SELECT
        pf.[Id],
        (SELECT COUNT(*) FROM [org].[RoutineOrigin] WHERE [PropertyValue] = pf.[Id]) as [RoutineOrigins],
        (SELECT COUNT(*) FROM [org].[RoutineTerminus] WHERE [PropertyValue] = pf.[Id]) as [RoutineTermi]
    FROM
        [enc].[ProcedureFieldOption] pf) cnt ON(pfo.[Id] = cnt.[Id])    
    LEFT OUTER JOIN
    (SELECT 
        epv.ProcedureFieldId as [FieldId],
        s.[value] as [OptionId],
        COUNT(*) as [Encounters]
    FROM 
        [enc].[EncounterProcedureValue] epv 
        INNER JOIN [enc].[ProcedureField] pf ON(epv.[ProcedureFieldId] = pf.[Id])
        CROSS APPLY STRING_SPLIT(epv.[Value], ',') s
    WHERE 
        pf.[Type] = 4
    GROUP BY
        epv.ProcedureFieldId,
        s.[value]) epv ON(pfo.[FieldId] = epv.[FieldId] AND epv.[OptionId] = pfo.[Id])
GO