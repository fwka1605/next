DECLARE
 @ymd DATETIME2(3) = DATEADD(DAY, -1, GETDATE())

DELETE          d
FROM            [dbo].[ImportData] d
WHERE           d.[CreateBy] <= @ymd
