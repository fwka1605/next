
DELETE ExportFieldSettingBase
GO

INSERT INTO ExportFieldSettingBase
(ExportFileType, ColumnName, Caption, ColumnOrder, AllowExport, DataType)
          SELECT 1 [ExportFileType], 'CompanyCode'                  [ColumnName], '会社コード'         [Caption], 1  ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'SlipNumber'                   [ColumnName], '伝票番号'           [Caption], 2  ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'CustomerCode'                 [ColumnName], '得意先コード'       [Caption], 3  ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'CustomerName'                 [ColumnName], '得意先名'           [Caption], 4  ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'InvoiceCode'                  [ColumnName], '請求書番号'         [Caption], 5  ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BilledAt'                     [ColumnName], '請求日'             [Caption], 6  ColumnOrder, 1 AllowExport, 1 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptCategoryCode'          [ColumnName], '入金区分コード'     [Caption], 7  ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptCategoryName'          [ColumnName], '入金区分名'         [Caption], 8  ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'RecordedAt'                   [ColumnName], '入金日'             [Caption], 9  ColumnOrder, 1 AllowExport, 1 DataType
UNION ALL SELECT 1 [ExportFileType], 'DueAt'                        [ColumnName], '期日'               [Caption], 10 ColumnOrder, 1 AllowExport, 1 DataType
UNION ALL SELECT 1 [ExportFileType], 'Amount'                       [ColumnName], '消込額'             [Caption], 11 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'DepartmentCode'               [ColumnName], '請求部門コード'     [Caption], 12 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'DepartmentName'               [ColumnName], '請求部門名'         [Caption], 13 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'CurrencyCode'                 [ColumnName], '通貨コード'         [Caption], 14 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptAmount'                [ColumnName], '入金額'             [Caption], 15 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptId'                    [ColumnName], '入金データID'       [Caption], 16 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillingNote1'                 [ColumnName], '請求備考1'          [Caption], 17 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillingNote2'                 [ColumnName], '請求備考2'          [Caption], 18 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillingNote3'                 [ColumnName], '請求備考3'          [Caption], 19 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillingNote4'                 [ColumnName], '請求備考4'          [Caption], 20 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptNote1'                 [ColumnName], '入金備考1'          [Caption], 21 ColumnOrder, 1 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptNote2'                 [ColumnName], '入金備考2'          [Caption], 22 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptNote3'                 [ColumnName], '入金備考3'          [Caption], 23 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptNote4'                 [ColumnName], '入金備考4'          [Caption], 24 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillNumber'                   [ColumnName], '手形番号'           [Caption], 25 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillBankCode'                 [ColumnName], '券面銀行コード'     [Caption], 26 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillBranchCode'               [ColumnName], '券面支店コード'     [Caption], 27 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillDrawAt'                   [ColumnName], '振出日'             [Caption], 28 ColumnOrder, 0 AllowExport, 1 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillDrawer'                   [ColumnName], '振出人'             [Caption], 29 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BillingMemo'                  [ColumnName], '請求メモ'           [Caption], 30 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptMemo'                  [ColumnName], '入金メモ'           [Caption], 31 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'MatchingMemo'                 [ColumnName], '消込メモ'           [Caption], 32 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BankCode'                     [ColumnName], '銀行コード'         [Caption], 33 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BankName'                     [ColumnName], '銀行名'             [Caption], 34 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BranchCode'                   [ColumnName], '支店コード'         [Caption], 35 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'BranchName'                   [ColumnName], '支店名'             [Caption], 36 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'AccountNumber'                [ColumnName], '口座番号'           [Caption], 37 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'SourceBankName'               [ColumnName], '仕向銀行'           [Caption], 38 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'SourceBranchName'             [ColumnName], '仕向支店'           [Caption], 39 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'VirtualBranchCode'            [ColumnName], '仮想支店コード'     [Caption], 40 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'VirtualAccountNumber'         [ColumnName], '仮想口座番号'       [Caption], 41 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'SectionCode'                  [ColumnName], '入金部門コード'     [Caption], 42 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'SectionName'                  [ColumnName], '入金部門名'         [Caption], 43 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'ReceiptCategoryExternalCode'  [ColumnName], '入金区分外部コード' [Caption], 44 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'OriginalReceiptId'            [ColumnName], '元入金データID'     [Caption], 45 ColumnOrder, 0 AllowExport, 0 DataType
UNION ALL SELECT 1 [ExportFileType], 'JournalizingCategory'         [ColumnName], '仕訳種別'           [Caption], 46 ColumnOrder, 0 AllowExport, 0 DataType
;

GO