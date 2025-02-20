CREATE PROCEDURE [dbo].[sp_reports_GetProcedureList]
    @StartDate DATE = NULL,-- Input parameter for the start date of the date range
    @EndDate DATE = NULL,-- Input parameter for the end date of the date range
    @Facility UNIQUEIDENTIFIER = NULL, -- Facility filter (optional)
    @Room UNIQUEIDENTIFIER = NULL,     -- Room filter (optional)
    @Clinician UNIQUEIDENTIFIER = NULL, -- Clinician filter (optional)
    @Procedure UNIQUEIDENTIFIER = NULL, -- Procedure filter (optional)
    @OrderingProvider UNIQUEIDENTIFIER = NULL -- Ordering Provider filter (optional)
AS
BEGIN
    SET NOCOUNT ON;

    -- Query the vwReportSourceProcedures view with the provided filters
    SELECT 
        p.[ProcedureName] AS [Procedure],  -- Name of the procedure
        COUNT(*) AS [Total],               -- Total count of the procedure
        CAST(AVG(DATEDIFF(MINUTE, p.[PerformedAt], p.[EncounterCompletedOn])) AS VARCHAR) + ' min' AS [ProcedureTime],
        CAST((SUM(p.Successful) * 100.0 / COUNT(*)) AS VARCHAR) + '%' AS [SuccessRate] -- Success rate percentage
    FROM 
        [dbo].[vwReportSourceProcedures] p
    WHERE 
        (@StartDate IS NULL OR p.[PerformedAt] >= @StartDate) AND
        (@EndDate IS NULL OR p.[PerformedAt] <= @EndDate) AND
        (@Facility IS NULL OR p.[FacilityId] = @Facility) AND
        (@Room IS NULL OR p.[RoomId] = @Room) AND
        (@Clinician IS NULL OR p.[PerformedBy] = @Clinician) AND
        (@Procedure IS NULL OR p.[ProcedureId] = @Procedure) AND
        (@OrderingProvider IS NULL OR p.[OrderingProvider] = @OrderingProvider)
    GROUP BY 
        p.[ProcedureName]
    ORDER BY 
        [Total] DESC;  -- Sort by total count descending
END
GO
