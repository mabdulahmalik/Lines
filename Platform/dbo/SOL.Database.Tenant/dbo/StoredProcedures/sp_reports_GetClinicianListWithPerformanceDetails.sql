CREATE PROCEDURE [dbo].[sp_reports_GetClinicianListWithPerformanceDetails]
    @StartDate DATE,     -- Input parameter for the start date of the date range
    @EndDate DATE        -- Input parameter for the end date of the date range
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy data for testing
    DECLARE @ClinicianData TABLE (
        Clinician NVARCHAR(100),     -- Name of the clinician
        ProcedureName NVARCHAR(100), -- Name of the procedure
        TotalProcedures INT,         -- Total number of procedures performed
        ProcedureTime DECIMAL(5, 1), -- Average time taken for the procedure in minutes
        SuccessRate DECIMAL(5, 1)    -- Success rate as a percentage
    );

    -- Insert dummy data into the temporary table for each clinician and procedure
    INSERT INTO @ClinicianData (Clinician, ProcedureName, TotalProcedures, ProcedureTime, SuccessRate)
    VALUES 
        ('Tsvi Michael Reiter', 'Blood Culture 1st Set', 50, 3.2, 78.5),
        ('Tsvi Michael Reiter', 'Lab Draw', 40, 5.1, 80.0),
        ('Tsvi Michael Reiter', 'Line Check', 35, 6.3, 79.0),
        ('Tsvi Michael Reiter', 'Dressing Change', 31, 4.8, 77.2),
        
        ('Jillian Carson', 'Blood Culture 1st Set', 30, 10.5, 85.0),
        ('Jillian Carson', 'Lab Draw', 25, 9.3, 83.0),
        ('Jillian Carson', 'Line Check', 22, 11.2, 82.5),
        ('Jillian Carson', 'Dressing Change', 21, 10.1, 84.0),
        
        ('Linda C Mitchell', 'Blood Culture 1st Set', 20, 7.8, 80.3),
        ('Linda C Mitchell', 'Lab Draw', 18, 7.1, 78.5),
        ('Linda C Mitchell', 'Line Check', 16, 8.2, 79.0),
        ('Linda C Mitchell', 'Dressing Change', 15, 8.0, 81.5),
        
        ('José Edvaldo Saraiva', 'Blood Culture 1st Set', 10, 24.2, 65.4),
        ('José Edvaldo Saraiva', 'Lab Draw', 8, 23.0, 64.0),
        ('José Edvaldo Saraiva', 'Line Check', 7, 25.1, 63.5),
        ('José Edvaldo Saraiva', 'Dressing Change', 7, 22.8, 66.0),
        
        ('Shu K Ito', 'Blood Culture 1st Set', 10, 24.2, 65.4),
        ('Shu K Ito', 'Lab Draw', 8, 23.0, 64.0),
        ('Shu K Ito', 'Line Check', 7, 25.1, 63.5),
        ('Shu K Ito', 'Dressing Change', 7, 22.8, 66.0),
        
        ('Bessie Cooper', 'Blood Culture 1st Set', 10, 24.2, 65.4),
        ('Bessie Cooper', 'Lab Draw', 8, 23.0, 64.0),
        ('Bessie Cooper', 'Line Check', 7, 25.1, 63.5),
        ('Bessie Cooper', 'Dressing Change', 7, 22.8, 66.0),
        
        ('Floyd Miles', 'Blood Culture 1st Set', 10, 24.2, 65.4),
        ('Floyd Miles', 'Lab Draw', 8, 23.0, 64.0),
        ('Floyd Miles', 'Line Check', 7, 25.1, 63.5),
        ('Floyd Miles', 'Dressing Change', 7, 22.8, 66.0)
        
        -- Add entries for remaining clinicians
        -- ('Shu K Ito', 'Blood Culture 1st Set', 5, 24.2, 72.1),
        -- ('Bessie Cooper', 'Lab Draw', 5, 24.2, 70.0),
        -- ('Savannah Nguyen', 'Line Check', 4, 24.2, 54.0),
        -- ('Cameron Williamson', 'Dressing Change', 3, 24.2, 54.0),
        -- ('Floyd Miles', 'Blood Culture 1st Set', 3, 24.2, 54.0),
        -- ('Darlene Robertson', 'Lab Draw', 2, 24.2, 54.0);

    -- Return the list of clinicians and their details, with each procedure recorded separately
    SELECT 
        Clinician,
        ProcedureName,
        TotalProcedures,
        ProcedureTime,
        SuccessRate
    FROM 
        @ClinicianData
    ORDER BY 
        Clinician, ProcedureName;  -- Order by clinician name and procedure name
END;
GO
