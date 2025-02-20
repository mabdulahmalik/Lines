CREATE PROCEDURE [dbo].[spSortEntityItems]
  @entityTable NVARCHAR(255),
  @entityId UNIQUEIDENTIFIER,
  @currPos INT,
  @prevPos INT
AS
SET NOCOUNT ON;
DECLARE @SQL NVARCHAR(MAX)

IF @currPos < @prevPos
    SET @SQL = CONCAT('UPDATE ', @entityTable, ' SET [Order] = [Order] + 1 WHERE [Order] >= ', @currPos, ' AND [Order] < ', @prevPos, ';');
ELSE
    SET @SQL = CONCAT('UPDATE ', @entityTable, ' SET [Order] = [Order] - 1 WHERE [Order] > ', @prevPos, ' AND [Order] <= ', @currPos, ';');

SET @SQL = CONCAT(@SQL, CHAR(10), 'UPDATE ', @entityTable, ' SET [Order] = ', @currPos, ' WHERE [Id] = ''', @entityId, ''';');

SET @SQL = @SQL + CHAR(10) + '
WITH Renumbered AS (
  SELECT [Id], ROW_NUMBER() OVER (ORDER BY [Order]) as new_order
  FROM ' + @entityTable + '
)
UPDATE ' + @entityTable + '
SET [Order] = new_order
FROM Renumbered
WHERE ' + @entityTable + '.[Id] = [Renumbered].[Id];';

--PRINT @SQL
EXEC sp_executesql @SQL
GO