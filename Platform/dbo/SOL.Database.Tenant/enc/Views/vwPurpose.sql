CREATE VIEW [enc].[vw_Purpose]
    AS
SELECT
    prp.[Id],
    [CreatedAt],
    [ModifiedAt],
    [Archived],
    [Order],
    [Name],
    (cnt.[Jobs] + cnt.[Encounters] + cnt.[Routines]) as [References]
FROM
    [enc].[Purpose] prp INNER JOIN
    (SELECT
        p.[Id],
        (SELECT COUNT(*) FROM [enc].[Job] WHERE [PurposeId] = p.[Id]) as [Jobs],
        (SELECT COUNT(*) FROM [enc].[Encounter] WHERE [PurposeId] = p.[Id]) as [Encounters],
        (SELECT COUNT(*) FROM [org].[Routine] WHERE [PurposeId] = p.[Id]) as [Routines]
    FROM
        [enc].[Purpose] p) cnt ON(prp.[Id] = cnt.[Id])
GO