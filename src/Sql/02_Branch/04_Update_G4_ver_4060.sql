--USE [VOneG4]
--GO

BEGIN TRANSACTION
GO

INSERT INTO [dbo].[EBFormat]
SELECT u.[id], u.[name], u.[dispOrder], u.[rqBank], u.[rqYear], u.[slDate], u.[types], u.[values]
  FROM (
            SELECT  12 [id], 12 [dispOrder], 0 [rqBank], 0 [rqYear], 1 [slDate], 1 [types], N'01' [values], N'キューピーネット 入出金明細' [name]
) u
WHERE NOT EXISTS (
      SELECT 1 FROM [dbo].[EBFormat] ef
       WHERE ef.[Id]    = u.[id] )
GO

COMMIT
