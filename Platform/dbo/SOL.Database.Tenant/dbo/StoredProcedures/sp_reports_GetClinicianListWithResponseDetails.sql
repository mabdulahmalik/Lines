CREATE PROCEDURE [dbo].[sp_reports_GetClinicianListWithResponseDetails]
    @StartDate DATE,     -- Input parameter for the start date of the date range
    @EndDate DATE        -- Input parameter for the end date of the date range
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy data for testing
    DECLARE @ClinicianResponseData TABLE (
        Clinician NVARCHAR(100),       -- Name of the clinician
        ProcedureName NVARCHAR(100),   -- Name of the procedure
        TotalProcedures INT,           -- Total number of procedures performed
        MinimumTime DECIMAL(5, 1),     -- Minimum time taken for the procedure in minutes
        MaximumTime DECIMAL(5, 1),     -- Maximum time taken for the procedure in minutes
        AverageTime DECIMAL(5, 1)      -- Average time taken for the procedure in minutes
    );

    -- Insert dummy data into the temporary table with multiple clinicians and procedures
    INSERT INTO @ClinicianResponseData (Clinician, ProcedureName, TotalProcedures, MinimumTime, MaximumTime, AverageTime)
    VALUES 
        -- Tsvi Michael Reiter
        ('Tsvi Michael Reiter', 'Blood Culture 1st Set', 156, 2.5, 5.0, 3.2),
        ('Tsvi Michael Reiter', 'Lab Draw', 140, 4.0, 6.5, 5.1),
        ('Tsvi Michael Reiter', 'Line Check', 125, 3.5, 7.0, 5.3),
        ('Tsvi Michael Reiter', 'Dressing Change', 120, 3.0, 5.5, 4.8),

        -- Jillian Carson
        ('Jillian Carson', 'Blood Culture 1st Set', 130, 6.0, 11.0, 8.5),
        ('Jillian Carson', 'Lab Draw', 115, 7.5, 13.0, 10.5),
        ('Jillian Carson', 'Line Check', 110, 5.0, 12.0, 8.0),
        ('Jillian Carson', 'Dressing Change', 105, 6.5, 11.0, 9.0),

        -- Linda C Mitchell
        ('Linda C Mitchell', 'Blood Culture 1st Set', 90, 4.5, 9.0, 6.8),
        ('Linda C Mitchell', 'Lab Draw', 85, 3.5, 8.5, 6.0),
        ('Linda C Mitchell', 'Line Check', 78, 4.0, 9.5, 7.2),
        ('Linda C Mitchell', 'Dressing Change', 75, 3.8, 8.5, 6.9),

        -- José Edvaldo Saraiva
        ('José Edvaldo Saraiva', 'Blood Culture 1st Set', 70, 20.0, 30.0, 24.2),
        ('José Edvaldo Saraiva', 'Lab Draw', 65, 18.0, 28.0, 23.5),
        ('José Edvaldo Saraiva', 'Line Check', 60, 19.0, 26.5, 22.1),
        ('José Edvaldo Saraiva', 'Dressing Change', 58, 20.5, 27.0, 23.0);

    -- Return the list of clinicians and their details
    SELECT 
        Clinician,
        ProcedureName,
        TotalProcedures AS Count,
        MinimumTime,
        MaximumTime,
        AverageTime
    FROM 
        @ClinicianResponseData
    ORDER BY 
        Clinician, ProcedureName;  -- Order by clinician name and procedure name
END;
GO
