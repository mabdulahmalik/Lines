CREATE PROCEDURE [dbo].[sp_reports_GetBottomFivePerformers]
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
        ('Bessie Cooper', 18),
        ('Savannah Nguyen', 8),
        ('Cameron Williamson', 6),
        ('Floyd Miles', 5),
        ('Darlene Robertson', 2);

    -- Return the bottom 5 clinicians based on procedures count
    SELECT TOP 5 
        ClinicianName AS Clinician,    -- Name of the clinician
        ProceduresCount               -- Total number of procedures
    FROM 
        @ClinicianProcedures
    ORDER BY 
        ProceduresCount ASC;           -- Order by lowest procedure count

END;
GO
