CREATE PROCEDURE [dbo].[sp_reports_GetBottomFiveProcedureTimes]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Create inline temporary table for dummy data
    DECLARE @ClinicianProcedures TABLE (
        ClinicianName VARCHAR(100),    -- Name of the clinician
        ProcedureTimeMinutes INT            -- Total count of procedures
    );

    -- Insert dummy data into the temporary table
    INSERT INTO @ClinicianProcedures (ClinicianName, ProcedureTimeMinutes)
    VALUES
        ('Bessie Cooper', 38),
        ('Savannah Nguyen', 32),
        ('Cameron Williamson', 25),
        ('Floyd Miles', 23),
        ('Darlene Robertson', 21);

    -- Return the bottom 5 clinicians based on procedures count
    SELECT TOP 5 
        ClinicianName AS Clinician,    -- Name of the clinician
        ProcedureTimeMinutes               -- Total number of procedures
    FROM 
        @ClinicianProcedures
    ORDER BY 
        ProcedureTimeMinutes DESC;           -- Order by lowest procedure count

END;
GO
