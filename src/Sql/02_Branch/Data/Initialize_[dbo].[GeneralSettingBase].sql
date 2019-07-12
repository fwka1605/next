
INSERT INTO [dbo].[GeneralSettingBase]
     ( Code
     , Value
     , Length
     , Precision
     , Description )
SELECT u.*
  FROM (
           SELECT N'仮受科目コード'           [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する仮受科目コード'                   [Description]
 UNION ALL SELECT N'仮受部門コード'           [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する仮受部門コード'                   [Description]
 UNION ALL SELECT N'仮受補助コード'           [Code], N''         [Value],  12 [Length], 0 [Precision], N'仕訳の際に使用する仮受補助コード'                   [Description]
 UNION ALL SELECT N'回収予定範囲'             [Code], N'20'       [Value],   2 [Length], 0 [Precision], N'入金日を基準とした回収予定の範囲 ±９９日'          [Description]
 UNION ALL SELECT N'旧消費税率'               [Code], N'0300'     [Value],   4 [Length], 2 [Precision], N'旧税率  ０３．００％'                               [Description]
 UNION ALL SELECT N'手数料誤差'               [Code], N'1000'     [Value],   4 [Length], 0 [Precision], N'一括消込時の許容誤差。入金額＋誤差まで消込可'       [Description]
 UNION ALL SELECT N'振込手数料科目コード'     [Code], N''         [Value],   6 [Length], 0 [Precision], N'仕訳の際に使用する振込手数料科目コード'             [Description]
 UNION ALL SELECT N'振込手数料部門コード'     [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する振込手数料部門コード'             [Description]
 UNION ALL SELECT N'振込手数料補助コード'     [Code], N''         [Value],  12 [Length], 0 [Precision], N'仕訳の際に使用する振込手数料補助コード'             [Description]
 UNION ALL SELECT N'新消費税率'               [Code], N'0500'     [Value],   4 [Length], 2 [Precision], N'新税率  ０５．００％'                               [Description]
 UNION ALL SELECT N'新税率開始年月日'         [Code], N'19970401' [Value],   8 [Length], 0 [Precision], N'新税率の開始日'                                     [Description]
 UNION ALL SELECT N'請求データ検索開始月範囲' [Code], N'10'       [Value],   2 [Length], 0 [Precision], N'請求データ検索に使用する請求日の開始を指定する月数' [Description]
 UNION ALL SELECT N'入金区分前受コード'       [Code], N'99'       [Value],   2 [Length], 0 [Precision], N'入金区分の前受処理用コード'                         [Description]
 UNION ALL SELECT N'入金部門コード'           [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する入金部門コード'                   [Description]
 UNION ALL SELECT N'借方消費税誤差科目コード' [Code], N''         [Value],   5 [Length], 0 [Precision], N'仕訳の際に使用する借方消費税誤差科目コード'         [Description]
 UNION ALL SELECT N'借方消費税誤差部門コード' [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する借方消費税誤差部門コード'         [Description]
 UNION ALL SELECT N'借方消費税誤差補助コード' [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する借方消費税誤差補助コード'         [Description]
 UNION ALL SELECT N'貸方消費税誤差科目コード' [Code], N''         [Value],   5 [Length], 0 [Precision], N'仕訳の際に使用する貸方消費税誤差科目コード'         [Description]
 UNION ALL SELECT N'貸方消費税誤差部門コード' [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する貸方消費税誤差部門コード'         [Description]
 UNION ALL SELECT N'貸方消費税誤差補助コード' [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する貸方消費税誤差補助コード'         [Description]
 UNION ALL SELECT N'金額計算端数処理'         [Code], N'0'        [Value],   1 [Length], 0 [Precision], N'小数点以下端数処理 0:切捨 1:切上 2:四捨五入'        [Description]
 UNION ALL SELECT N'消費税計算端数処理'       [Code], N'0'        [Value],   1 [Length], 0 [Precision], N'小数点以下端数処理 0:切捨 1:切上 2:四捨五入'        [Description]
 UNION ALL SELECT N'消費税誤差'               [Code], N'0'        [Value],   2 [Length], 0 [Precision], N'一括消込時の許容誤差。入金額±誤差まで消込可'       [Description]
 UNION ALL SELECT N'新消費税率2'              [Code], N'0800'     [Value],   4 [Length], 2 [Precision], N'新税率2 ０８．００%'                                [Description]
 UNION ALL SELECT N'新税率開始年月日2'        [Code], N'20140401' [Value],   8 [Length], 0 [Precision], N'新税率2の開始日'                                    [Description]
 UNION ALL SELECT N'新消費税率3'              [Code], N'1000'     [Value],   4 [Length], 2 [Precision], N'新税率3 １０．００％'                               [Description]
 UNION ALL SELECT N'新税率開始年月日3'        [Code], N'20191001' [Value],   8 [Length], 0 [Precision], N'新税率3の開始年月日'                                [Description]
 UNION ALL SELECT N'長期前受金科目コード'     [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する長期前受金科目コード'             [Description]
 UNION ALL SELECT N'長期前受金部門コード'     [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する長期前受金部門コード'             [Description]
 UNION ALL SELECT N'長期前受金補助コード'     [Code], N''         [Value],  10 [Length], 0 [Precision], N'仕訳の際に使用する長期前受金補助コード'             [Description]
 UNION ALL SELECT N'サーバパス'               [Code], N''         [Value], 200 [Length], 0 [Precision], N'サーバパス及びCSV出力パス'                          [Description]
 UNION ALL SELECT N'取込時端数処理'           [Code], N'3'        [Value],   1 [Length], 0 [Precision], N'取込時端数処理 0:切捨 1:切上 2:四捨五入 3:取込不可' [Description]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[GeneralSettingBase] b
        WHERE b.[Code]  = u.[Code] )
 ORDER BY
       u.Code   ASC;
GO
