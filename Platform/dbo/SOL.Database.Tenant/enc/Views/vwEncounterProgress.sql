CREATE VIEW [enc].[vw_EncounterProgress]
    AS
SELECT
    [EncounterId],
    [Waiting],
    DATEDIFF(mi, [Waiting], [Assigned]) as [Waiting_Duration],
    [Assigned],
    DATEDIFF(mi, [Assigned], [Attending]) as [Assigned_Duration],
    [Attending],
    DATEDIFF(mi, [Attending], [Charting]) as [Attending_Duration],
    [Charting],
    DATEDIFF(mi, [Charting], [Completed]) as [Charting_Duration],
    [Completed]
FROM (
    SELECT [EncounterId], [Timestamp], CASE [Stage] 
        WHEN 1 THEN 'Waiting'
        WHEN 2 THEN 'Assigned'
        WHEN 3 THEN 'Attending'
        WHEN 4 THEN 'Charting'
        WHEN 5 THEN 'Completed'
    END as [Stage]
    FROM   [enc].[EncounterStage]
) as SourceTable
PIVOT (
    MAX([Timestamp])
    FOR [Stage] IN([Waiting], [Assigned], [Attending], [Charting], [Completed])
) as PivotTable
GO