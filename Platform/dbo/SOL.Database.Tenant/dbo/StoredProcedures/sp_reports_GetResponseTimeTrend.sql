CREATE PROCEDURE [dbo].[sp_reports_GetResponseTimeTrend]
    @StartDate DATE,     -- Input parameter for the start date of the date range
    @EndDate DATE        -- Input parameter for the end date of the date range
AS
BEGIN
    -- Disable counting of affected rows to improve performance
    SET NOCOUNT ON;

    -- Inline temporary table to hold dummy data for testing
    DECLARE @ResponseTimeData TABLE (
        ResponseDate DATE,        -- Date of the response
        ResponseTime INT          -- Response time in seconds
    );

    -- Insert dummy data into the temporary table
    INSERT INTO @ResponseTimeData (ResponseDate, ResponseTime)
    VALUES 
        -- Data for October 21, 2024
        ('2024-10-21', 2),
        ('2024-10-21', 4),
        ('2024-10-21', 1),
        ('2024-10-21', 3),

        -- Data for October 20, 2024
        ('2024-10-20', 5),
        ('2024-10-20', 2),
        ('2024-10-20', 3),
        ('2024-10-20', 6),

        -- Data for October 19, 2024
        ('2024-10-19', 3),
        ('2024-10-19', 4),
        ('2024-10-19', 3),
        ('2024-10-19', 2),

        -- Data for October 18, 2024
        ('2024-10-18', 7),
        ('2024-10-18', 6),
        ('2024-10-18', 5),
        ('2024-10-18', 8),

        -- Data for October 17, 2024
        ('2024-10-17', 2),
        ('2024-10-17', 1),
        ('2024-10-17', 4),
        ('2024-10-17', 3),

        -- Data for October 16, 2024
        ('2024-10-16', 9),
        ('2024-10-16', 10),
        ('2024-10-16', 8),
        ('2024-10-16', 7);

    -- Calculate metrics and transform into the required format
    SELECT 
        ResponseDate AS Date,
        'Average' AS Metric,
        AVG(ResponseTime) AS Value
    FROM 
        @ResponseTimeData
    GROUP BY 
        ResponseDate
    UNION ALL
    SELECT 
        ResponseDate AS Date,
        'Minimum' AS Metric,
        MIN(ResponseTime) AS Value
    FROM 
        @ResponseTimeData
    GROUP BY 
        ResponseDate
    UNION ALL
    SELECT 
        ResponseDate AS Date,
        'Maximum' AS Metric,
        MAX(ResponseTime) AS Value
    FROM 
        @ResponseTimeData
    GROUP BY 
        ResponseDate
    ORDER BY 
        Date, Metric;  -- Order the results by date and metric
END;
GO
