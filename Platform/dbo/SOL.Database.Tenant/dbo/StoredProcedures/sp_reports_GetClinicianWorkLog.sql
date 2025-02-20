CREATE PROCEDURE [dbo].[sp_reports_GetClinicianWorkLog]
    @StartDate DATE,     -- Input parameter for the start date of the date range
    @EndDate DATE        -- Input parameter for the end date of the date range
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy data for testing
    DECLARE @WorkLogData TABLE (
        Clinician NVARCHAR(100),  -- Clinician's name
        UnderClinician NVARCHAR(100),  -- Clinician's name
        LED INT,                  -- Number of procedures led by the clinician
        Assisted INT,             -- Number of procedures assisted
        Training INT,             -- Number of training sessions conducted
        LedWithTrainee INT,       -- Procedures led by clinician with a trainee
        LedWithAssistant INT      -- Procedures led by clinician with an assistant
    );

    -- Insert dummy data into the temporary table
    INSERT INTO @WorkLogData (Clinician, UnderClinician, LED, Assisted, Training, LedWithTrainee, LedWithAssistant)
    VALUES 
        ('Tsvi Michael Reiter', NULL, 15, NULL, NULL, NULL, NULL),
        ('Tsvi Michael Reiter', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Tsvi Michael Reiter', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Tsvi Michael Reiter', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('Jillian Carson', NULL, 15, NULL, NULL, NULL, NULL),
        ('Jillian Carson', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Jillian Carson', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Jillian Carson', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('Linda C Mitchell', NULL, 15, NULL, NULL, NULL, NULL),
        ('Linda C Mitchell', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Linda C Mitchell', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Linda C Mitchell', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('José Edvaldo Saraiva', NULL, 15, NULL, NULL, NULL, NULL),
        ('José Edvaldo Saraiva', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('José Edvaldo Saraiva', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('José Edvaldo Saraiva', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('Shu K Ito', NULL, 15, NULL, NULL, NULL, NULL),
        ('Shu K Ito', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Shu K Ito', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Shu K Ito', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('Bessie Cooper', NULL, 15, NULL, NULL, NULL, NULL),
        ('Bessie Cooper', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Bessie Cooper', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Bessie Cooper', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('Savannah Nguyen', NULL, 15, NULL, NULL, NULL, NULL),
        ('Savannah Nguyen', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Savannah Nguyen', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Savannah Nguyen', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('Cameron Williamson', NULL, 15, NULL, NULL, NULL, NULL),
        ('Cameron Williamson', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Cameron Williamson', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Cameron Williamson', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),



        ('Floyd Miles', NULL, 15, NULL, NULL, NULL, NULL),
        ('Floyd Miles', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Floyd Miles', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Floyd Miles', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL),


        
        ('Darlene Robertson', NULL, 15, NULL, NULL, NULL, NULL),
        ('Darlene Robertson', 'Kelsey Shoneck',  NULL, 1,     NULL,   1,   NULL),
        ('Darlene Robertson', 'Tifanny Johnson', NULL, 2,     NULL,   NULL,   1),
        ('Darlene Robertson', 'Kelly Powell',    NULL, NULL,  1,      NULL,      NULL);

    -- Select the work log data
    SELECT 
        Clinician,
        UnderClinician,
        LED,
        Assisted,
        Training,
        LedWithTrainee,
        LedWithAssistant
    FROM 
        @WorkLogData
    ORDER BY 
        Clinician;  -- Order the results by clinician name
END;
GO
