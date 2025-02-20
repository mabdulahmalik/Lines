CREATE PROCEDURE [dbo].[sp_reports_GetAllOrderingProviders]
AS
BEGIN
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy OrderingProvider data
    DECLARE @OrderingProviders TABLE (
        OrderingProviderId VARCHAR(100),   -- GUID for OrderingProvider ID
        OrderingProviderName VARCHAR(100)      -- Name of the OrderingProvider
    );

    -- Insert 5 dummy ordering providers
    INSERT INTO @OrderingProviders (OrderingProviderId, OrderingProviderName)
    VALUES 
        (NEWID(), 'OrderingProvider1'),
        (NEWID(), 'OrderingProvider2'),
        (NEWID(), 'OrderingProvider3');

    -- Return the data
    SELECT * FROM @OrderingProviders;
END;
GO
