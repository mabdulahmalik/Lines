CREATE PROCEDURE [dbo].[sp_reports_GetAllFacilities]
AS
BEGIN
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy facility data
    DECLARE @Facilities TABLE (
        FacilityId VARCHAR(100),   -- GUID for Facility ID
        FacilityName VARCHAR(100)      -- Name of the Facility
    );

    -- Insert 5 dummy facilities
    INSERT INTO @Facilities (FacilityId, FacilityName)
    VALUES 
        (NEWID(), 'Central Health Clinic'),
        (NEWID(), 'Downtown Medical Center'),
        (NEWID(), 'Green Valley Hospital'),
        (NEWID(), 'Northside Clinic'),
        (NEWID(), 'Riverside Healthcare');

    -- Return the data
    SELECT * FROM @Facilities;
END;
GO
