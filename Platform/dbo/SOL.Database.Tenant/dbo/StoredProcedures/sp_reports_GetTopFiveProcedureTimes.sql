CREATE PROCEDURE [dbo].[sp_reports_GetTopFiveProcedureTimes]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Create inline temporary table for dummy data
    DECLARE @ClinicianProcedureTimes TABLE (
        ClinicianName VARCHAR(100),    -- Name of the clinician
        ProcedureTimeMinutes INT       -- Total procedure time in minutes
    );

    -- Insert dummy data into the temporary table
    INSERT INTO @ClinicianProcedureTimes (ClinicianName, ProcedureTimeMinutes)
    VALUES
        ('Tsvi Michael Reiter', 2),
        ('Jillian Carson', 4),
        ('Linda C Mitchel', 7),
        ('Jose Edvaldo Saraiva', 9),
        ('Shu K Ito', 16);

    -- Return the top 5 clinicians based on total procedure time
    SELECT TOP 5 
        ClinicianName AS Clinician,    -- Name of the clinician
        ProcedureTimeMinutes           -- Total procedure time in minutes
    FROM 
        @ClinicianProcedureTimes
    ORDER BY 
        ProcedureTimeMinutes ASC;     -- Order by highest procedure time

END;
GO
