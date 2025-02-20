CREATE PROCEDURE [dbo].[sp_reports_GetClinicianProcedureBreakdownForStackedBarChart]
    @StartDate DATE,     -- Input parameter for the start date of the date range
    @EndDate DATE        -- Input parameter for the end date of the date range
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy data for testing
    DECLARE @ClinicianProcedureData TABLE (
        Clinician NVARCHAR(100),  -- Name of the clinician
        ProcedureType NVARCHAR(100), -- Type of procedure performed
        ProcedureCount INT          -- Number of times the procedure was performed
    );

    -- Insert dummy data into the temporary table
    INSERT INTO @ClinicianProcedureData (Clinician, ProcedureType, ProcedureCount)
    VALUES 
        ('Tsvi Michael Reiter', 'Blood Culture 1st set', 40),
        ('Tsvi Michael Reiter', 'Lab Draw', 35),
        ('Tsvi Michael Reiter', 'IV (2nd)', 50),
        ('Jillian Carson', 'Dressing Change', 25),
        ('Jillian Carson', 'Line Check', 30),
        ('Linda C Mitchell', 'Insertion Procedure', 60),
        ('Linda C Mitchell', 'PICC/CVL/Midline Removal', 14),
        ('José Edvaldo Saraiva', 'Port-A-Cath', 20),
        ('Bessie Cooper', 'Blood Culture 2nd set', 10),
        ('Cameron Williamson', 'Lab Draw', 18),
        ('Floyd Miles', 'Blood Culture 1st set', 5),
        ('Darlene Robertson', 'Dressing Change', 2),
        ('Savannah Nguyen', 'Insertion Procedure', 8),
        ('Shu K Ito', 'Lab Draw', 30);

    -- Return the list of clinicians and the breakdown of their procedures
    SELECT 
        Clinician,
        ProcedureType,
        ProcedureCount
    FROM 
        @ClinicianProcedureData
    ORDER BY 
        Clinician, ProcedureType;  -- Order by clinician name and procedure type
END;
GO
