
INSERT INTO [dbo].[PaymentFileFormat]
     ( Id
     , Name
     , DisplayOrder
     , Available
     , IsNeedYear)
SELECT u.Id
     , u.Name
     , u.DisplayOrder
     , u.Available
     , u.IsNeedYear
  FROM (
            SELECT  1 [Id], N'全銀（口座振替 カンマ区切り）'   [Name],  1 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  2 [Id], N'全銀（口座振替 固定長）'         [Name],  2 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  3 [Id], N'みずほファクター（Web伝送）'     [Name],  3 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  4 [Id], N'三菱UFJファクター'               [Name],  5 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  5 [Id], N'SMBC（口座振替 固定長）'         [Name],  7 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  6 [Id], N'三菱UFJニコス'                   [Name],  6 [DisplayOrder], 1 [Available], 0 [IsNeedYear]
 UNION ALL  SELECT  7 [Id], N'みずほファクター（ASPサービス）' [Name],  4 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  8 [Id], N'リコーリースコレクト！'          [Name],  8 [DisplayOrder], 1 [Available], 0 [IsNeedYear]
 UNION ALL  SELECT  9 [Id], N'インターネット伝送ゆうちょ形式'  [Name],  9 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT 10 [Id], N'りそなネット'                    [Name], 10 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT 11 [Id], N'しんきん情報サービス'            [Name], 11 [DisplayOrder], 1 [Available], 0 [IsNeedYear]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[PaymentFileFormat] t
        WHERE t.Id      = u.Id )
 ORDER BY
       u.Id;
GO
