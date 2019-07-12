
INSERT INTO [dbo].[MasterImportSettingBase]
     ( [ImportFileType]
     , [ImportFileName]
     , [ImportMode]
     , [ExportErrorLog]
     , [ErrorLogDestination]
     , [Confirm] )
SELECT u.[ftype]    [ImportFileType]
     , u.[fname]    [ImportFileName]
     , 2            [ImportMode]
     , 1            [ExportErrorLog]
     , u.[dest]     [ErrorLogDestination]
     , 1            [Confirm]
  FROM (
           SELECT  1 [ftype], N'請求部門マスター'             [fname], 0 [dest]
 UNION ALL SELECT  2 [ftype], N'営業担当者マスター'           [fname], 0 [dest]
 UNION ALL SELECT  3 [ftype], N'ログインユーザーマスター'     [fname], 0 [dest]
 UNION ALL SELECT  4 [ftype], N'得意先マスター'               [fname], 0 [dest]
 UNION ALL SELECT  5 [ftype], N'得意先マスター登録手数料'     [fname], 0 [dest]
 UNION ALL SELECT  6 [ftype], N'得意先マスター歩引設定'       [fname], 0 [dest]
 UNION ALL SELECT  7 [ftype], N'銀行口座マスター'             [fname], 0 [dest]
 UNION ALL SELECT  8 [ftype], N'科目マスター'                 [fname], 0 [dest]
 UNION ALL SELECT  9 [ftype], N'債権代表者マスター'           [fname], 0 [dest]
 UNION ALL SELECT 10 [ftype], N'学習履歴マスター'             [fname], 0 [dest]
 UNION ALL SELECT 11 [ftype], N'入金・請求部門対応マスター'   [fname], 0 [dest]
 UNION ALL SELECT 12 [ftype], N'入金部門・担当者対応マスター' [fname], 0 [dest]
 UNION ALL SELECT 13 [ftype], N'入金部門マスター'             [fname], 0 [dest]
 UNION ALL SELECT 14 [ftype], N'長期前受契約マスター'         [fname], 0 [dest]
 UNION ALL SELECT 15 [ftype], N'除外カナマスター'             [fname], 1 [dest]
 UNION ALL SELECT 16 [ftype], N'カレンダーマスター'           [fname], 1 [dest]
 UNION ALL SELECT 17 [ftype], N'通貨マスター'                 [fname], 0 [dest]
 UNION ALL SELECT 18 [ftype], N'法人格マスター'               [fname], 1 [dest]
 UNION ALL SELECT 19 [ftype], N'銀行・支店マスター'           [fname], 0 [dest]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[MasterImportSettingBase] misb
        WHERE misb.[ImportFileType] = u.[ftype] )
;
GO
