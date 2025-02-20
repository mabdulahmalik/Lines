CREATE PROCEDURE [dbo].[sp_reports_GetAllProcedures]
AS
BEGIN
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy Procedure data
    DECLARE @Procedures TABLE (
        ProcedureId VARCHAR(100),   -- GUID for Procedure ID
        ProcedureName VARCHAR(100)      -- Name of the Procedure
    );

    -- Insert 5 dummy procedures
    INSERT INTO @Procedures (ProcedureId, ProcedureName)
    VALUES 
        (NEWID(), 'Procedure1'),
        (NEWID(), 'Procedure2'),
        (NEWID(), 'Procedure3');

    -- Return the data
    SELECT * FROM @Procedures;
END;
GO
