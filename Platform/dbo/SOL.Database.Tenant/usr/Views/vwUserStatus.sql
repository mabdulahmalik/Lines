CREATE VIEW [usr].[vw_UserStatus]  
AS  
SELECT  
    userId,  
    Availability Status,  
    Message,
    changedAt  
FROM usr.userstatus AS us1  
WHERE changedAt = (  
    SELECT MAX(changedAt)  
    FROM usr.userstatus AS us2  
    WHERE us2.userId = us1.userId  
);  
GO  
