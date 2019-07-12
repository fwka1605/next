
INSERT INTO [dbo].[BankAccountType]
     ( Id
     , Name
     , UseReceipt
     , UseTransfer )
SELECT u.Id
     , u.Name
     , u.UseReceipt
     , u.UseTransfer
  FROM (
            SELECT 1 [Id], 1 [UseReceipt], 1 [UseTransfer], N'普通預金'     [Name]
 UNION ALL  SELECT 2 [Id], 1 [UseReceipt], 1 [UseTransfer], N'当座預金'     [Name]
 UNION ALL  SELECT 3 [Id], 0 [UseReceipt], 1 [UseTransfer], N'納税準備預金' [Name]
 UNION ALL  SELECT 4 [Id], 1 [UseReceipt], 0 [UseTransfer], N'貯蓄預金'     [Name]
 UNION ALL  SELECT 5 [Id], 1 [UseReceipt], 0 [UseTransfer], N'通知預金'     [Name]
 UNION ALL  SELECT 8 [Id], 0 [UseReceipt], 0 [UseTransfer], N'外貨'         [Name]
 UNION ALL  SELECT 9 [Id], 0 [UseReceipt], 1 [UseTransfer], N'その他'       [Name]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[BankAccountType] b
        WHERE b.Id = u.Id );
GO
