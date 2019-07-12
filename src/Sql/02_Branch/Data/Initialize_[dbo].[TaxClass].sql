
INSERT INTO [dbo].[TaxClass]
     ( Id
     , Name )
SELECT u.Id
     , u.Name
  FROM (
            SELECT 0 [Id], N'外税課税' [Name]
 UNION ALL  SELECT 1 [Id], N'内税課税' [Name]
 UNION ALL  SELECT 2 [Id], N'非課税'   [Name]
 UNION ALL  SELECT 3 [Id], N'免税'     [Name]
 UNION ALL  SELECT 4 [Id], N'対象外'   [Name]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[TaxClass] t
        WHERE t.Id      = u.Id )
 ORDER BY
       u.Id;
GO
