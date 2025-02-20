CREATE PROCEDURE [dbo].[sp_reports_GetProceduresInPieChart]
    @StartDate DATE = NULL,                 -- Optional start date parameter
    @EndDate DATE = NULL,                   -- Optional end date parameter
    @Facility UNIQUEIDENTIFIER = NULL,      -- Facility filter
    @Room UNIQUEIDENTIFIER = NULL,          -- Room filter
    @Clinician UNIQUEIDENTIFIER = NULL,     -- Clinician filter
    @Procedure UNIQUEIDENTIFIER = NULL,     -- Procedure filter
    @OrderingProvider UNIQUEIDENTIFIER = NULL -- Ordering provider filter
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Return grouped data for the specified date range and individual procedures
    SELECT 
        ProcedureName,                  -- Line groups: the name of the procedure
        COUNT(*) AS ProcedureCount     -- Y-axis: the total number of procedures for each procedure name
    FROM 
        [dbo].[vwReportSourceProcedures]
    WHERE 
        (@StartDate IS NULL OR PerformedAt >= @StartDate) -- Include if @StartDate is provided
        AND (@EndDate IS NULL OR PerformedAt <= @EndDate) -- Include if @EndDate is provided
        AND (@Facility IS NULL OR FacilityId = @Facility)
        AND (@Room IS NULL OR RoomId = @Room)
        AND (@Clinician IS NULL OR PerformedBy = @Clinician)
        AND (@Procedure IS NULL OR ProcedureId = @Procedure)
        AND (@OrderingProvider IS NULL OR OrderingProvider = @OrderingProvider)
    GROUP BY 
        ProcedureName                  -- Group by procedure name
    ORDER BY 
        ProcedureName;                 -- Order by procedure name
END;
GO
