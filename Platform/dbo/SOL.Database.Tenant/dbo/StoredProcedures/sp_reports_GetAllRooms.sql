CREATE PROCEDURE [dbo].[sp_reports_GetAllRooms]
AS
BEGIN
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy Room data
    DECLARE @Rooms TABLE (
        RoomId VARCHAR(100),   -- GUID for Room ID
        RoomName VARCHAR(100)      -- Name of the Room
    );

    -- Insert 5 dummy rooms
    INSERT INTO @Rooms (RoomId, RoomName)
    VALUES 
        (NEWID(), 'Room1'),
        (NEWID(), 'Room2'),
        (NEWID(), 'Room3');

    -- Return the data
    SELECT * FROM @Rooms;
END;
GO
