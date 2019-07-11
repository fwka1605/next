# 追加テーブル

フリーインポーター


|table_name|column_id|column_name|column_name_jp|type|nullable|remarks|
|:---------|--------:|:----------|:-------------|:---|:-------|:------|
|ImportData|1|Id|インポートデータID|bigint|not null|identity(1,1)|
|ImportData|2|CompanyId|会社ID|int|not null||
|ImportData|3|FileName|ファイル名|nvarchar(255)|not null||
|ImportData|4|FileSize|ファイルサイズ|int|not null||
|ImportData|5|CreateBy|作成者ID|int|not null||
|ImportData|6|CreateAt|作成日時|datetime2(3)|not null||
|ImportDataDetail|1|ImportDataId|インポートデータID|bigint|not null|外部キー delete cascade|
|ImportDataDetail|2|ObjectType|オブジェクトタイプ|int|not null|基本0 請求フリーインポーター用 得意先を保持|
|ImportDataDetail|3|LineNumber|行番号|int|not null|取込ファイルの行番号|
|ImportDataDetail|4|RecordItem|行の変換したオブジェクト|varbinary(max)|not null|MessagePack で serialize|


