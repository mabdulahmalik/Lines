CREATE PROCEDURE [dbo].[sp_reports_GetAllClinicians]
AS
BEGIN
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy Clinician data
    DECLARE @Clinicians TABLE (
        ClinicianId VARCHAR(100),   -- GUID for Clinician ID
        ClinicianName VARCHAR(100)      -- Name of the Clinician
    );

    -- Insert 5 dummy clinicians
    INSERT INTO @Clinicians (ClinicianId, ClinicianName)
    VALUES 
        (NEWID(), 'Clinician1'),
        (NEWID(), 'Clinician2'),
        (NEWID(), 'Clinician3');

    -- Return the data
    SELECT * FROM @Clinicians;
END;
GO
