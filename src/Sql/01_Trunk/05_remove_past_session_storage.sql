USE [ScarletTrunk]

DECLARE @ymd DATETIME = DATEADD(DAY, 2, GETDATE());
DELETE FROM SessionStorage WHERE CreatedAt <= @ymd
GO
