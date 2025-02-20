CREATE PROCEDURE [dbo].[sp_reports_GetNoOfProcedures]
    @StartDate DATE = NULL,                  -- Optional input parameter for the start date of the date range
    @EndDate DATE = NULL,                    -- Optional input parameter for the end date of the date range
    @Facility UNIQUEIDENTIFIER = NULL,       -- Optional filter for Facility
    @Room UNIQUEIDENTIFIER = NULL,           -- Optional filter for Room
    @Clinician UNIQUEIDENTIFIER = NULL,      -- Optional filter for Clinician
    @Procedure UNIQUEIDENTIFIER = NULL,      -- Optional filter for Procedure
    @OrderingProvider UNIQUEIDENTIFIER = NULL -- Optional filter for OrderingProvider
AS
BEGIN
    SET NOCOUNT ON;

    -- Query the live data source
    SELECT 
        PerformedAt AS [Date], -- Grouped by procedure date
        COUNT(*) AS [TotalProcedures]       -- Total number of procedures for each date
    FROM 
        [dbo].[vwReportSourceProcedures]
    WHERE 
        (@StartDate IS NULL OR PerformedAt >= @StartDate) -- Apply StartDate filter if provided
        AND (@EndDate IS NULL OR PerformedAt <= @EndDate) -- Apply EndDate filter if provided
        AND (@Facility IS NULL OR FacilityId = @Facility)              -- Optional Facility filter
        AND (@Room IS NULL OR RoomId = @Room)                          -- Optional Room filter
        AND (@Clinician IS NULL OR PerformedBy = @Clinician)           -- Optional Clinician filter
        AND (@Procedure IS NULL OR ProcedureId = @Procedure)           -- Optional Procedure filter
        AND (@OrderingProvider IS NULL OR OrderingProvider = @OrderingProvider) -- Optional OrderingProvider filter
    GROUP BY 
        PerformedAt -- Group data by procedure date
    ORDER BY 
        PerformedAt; -- Sort results by date
END;
GO
