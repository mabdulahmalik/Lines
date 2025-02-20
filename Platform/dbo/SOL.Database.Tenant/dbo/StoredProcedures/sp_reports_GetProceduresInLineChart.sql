CREATE PROCEDURE [dbo].[sp_reports_GetProceduresInLineChart]
    @StartDate DATE = NULL,            -- Input parameter for the start date of the date range (optional)
    @EndDate DATE = NULL,              -- Input parameter for the end date of the date range (optional)
    @Facility UNIQUEIDENTIFIER = NULL, 
    @Room UNIQUEIDENTIFIER = NULL, 
    @Clinician UNIQUEIDENTIFIER = NULL, 
    @Procedure UNIQUEIDENTIFIER = NULL, 
    @OrderingProvider UNIQUEIDENTIFIER = NULL
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Return grouped data for the specified date range and individual procedures
    SELECT 
        CONCAT(
            LEFT(DATENAME(MONTH, PerformedAt), 3), 
            '. ', 
            DAY(PerformedAt)
        ) AS ProcedureDate,         -- X-axis: the date of the procedure
        ProcedureName,              -- Line groups: the name of the procedure
        COUNT(*) AS ProcedureCount  -- Y-axis: the total number of procedures on that date for each procedure name
    FROM 
        [dbo].[vwReportSourceProcedures]
    WHERE 
        (@StartDate IS NULL OR PerformedAt >= @StartDate) -- Apply StartDate filter if provided
        AND (@EndDate IS NULL OR PerformedAt <= @EndDate) -- Apply EndDate filter if provided
        AND (@Facility IS NULL OR FacilityId = @Facility)
        AND (@Room IS NULL OR RoomId = @Room)
        AND (@Clinician IS NULL OR PerformedBy = @Clinician)
        AND (@Procedure IS NULL OR ProcedureId = @Procedure)
        AND (@OrderingProvider IS NULL OR OrderingProvider = @OrderingProvider)
    GROUP BY 
        PerformedAt, ProcedureName  -- Group by date and procedure name
    ORDER BY 
        PerformedAt, ProcedureName; -- Order by date first, then by procedure name
END;
GO
