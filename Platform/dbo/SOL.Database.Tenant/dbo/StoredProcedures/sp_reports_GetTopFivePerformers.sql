CREATE PROCEDURE [dbo].[sp_reports_GetTopFivePerformers]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Create inline temporary table for dummy data
    DECLARE @ClinicianProcedures TABLE (
        ClinicianName VARCHAR(100),    -- Name of the clinician
        ProceduresCount INT            -- Total count of procedures
    );

    -- Insert dummy data into the temporary table
    INSERT INTO @ClinicianProcedures (ClinicianName, ProceduresCount)
    VALUES
        ('Tsvi Michael Reiter', 156),
        ('Jillian Carson', 98),
        ('Linda C Mitchel', 74),
        ('Jose Edvaldo Saraiva', 32),
        ('Shu K Ito', 30);

    -- Return the top 5 clinicians based on procedures count
    SELECT TOP 5 
        ClinicianName AS Clinician,    -- Name of the clinician
        ProceduresCount               -- Total number of procedures
    FROM 
        @ClinicianProcedures
    ORDER BY 
        ProceduresCount DESC;          -- Order by highest procedure count

END;
GO
