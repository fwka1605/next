

INSERT INTO CategoryBase
(CategoryType, Code, Name, TaxClassId, UseLimitDate, UseAdvanceReceived, UseInput)
SELECT u.*
  FROM (
          SELECT 1 [CategoryType], '01' [Code], '売上'     [Name], 1    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '01' [Code], '振込'     [Name], 4    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '02' [Code], '手形'     [Name], 4    [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '03' [Code], '期日現金' [Name], 4    [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '04' [Code], '小切手'   [Name], 4    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '05' [Code], '相殺'     [Name], 4    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '99' [Code], '前受'     [Name], 4    [TaxClassId], 0 [UseLimitDate], 1 [UseAdvanceReceived], 0 [UseInput]
UNION ALL SELECT 3 [CategoryType], '00' [Code], '約定'     [Name], NULL [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '01' [Code], '振込'     [Name], NULL [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '02' [Code], '手形'     [Name], NULL [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '03' [Code], '期日現金' [Name], NULL [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '04' [Code], '小切手'   [Name], NULL [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
) u
WHERE NOT EXISTS (
    SELECT 1
      FROM [dbo].[CategoryBase] b
     WHERE u.[CategoryType] = b.[CategoryType]
       AND u.[Code] = b.[Code]
)
     ORDER BY u.[CategoryType], u.[Code]
GO