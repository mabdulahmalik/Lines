CREATE PROCEDURE [dbo].[sp_reports_GetAverageProcedureTimePerDay]
    @StartDate DATE = NULL,     -- Input parameter for the start date of the date range (optional)
    @EndDate DATE = NULL,       -- Input parameter for the end date of the date range (optional)
    @Facility UNIQUEIDENTIFIER = NULL,
    @Room UNIQUEIDENTIFIER = NULL,
    @Clinician UNIQUEIDENTIFIER = NULL,
    @Procedure UNIQUEIDENTIFIER = NULL,
    @OrderingProvider UNIQUEIDENTIFIER = NULL
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Retrieve and calculate the average procedure time per day
    SELECT 
        PerformedAt AS ProcedureDate,
        AVG(DATEDIFF(MINUTE, PerformedAt, EncounterCompletedOn)) AS AverageProcedureTime
    FROM 
        [dbo].[vwReportSourceProcedures]
    WHERE 
        (@StartDate IS NULL OR PerformedAt >= @StartDate)
        AND (@EndDate IS NULL OR PerformedAt <= @EndDate)
        AND (@Facility IS NULL OR FacilityId = @Facility)
        AND (@Room IS NULL OR RoomId = @Room)
        AND (@Clinician IS NULL OR PerformedBy = @Clinician)
        AND (@Procedure IS NULL OR ProcedureId = @Procedure)
        AND (@OrderingProvider IS NULL OR OrderingProvider = @OrderingProvider)
    GROUP BY 
        PerformedAt
    ORDER BY 
        ProcedureDate;
END;
GO
