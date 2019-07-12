# Victory ONE ®

## 概要

入金情報 特に EB データをもとに消込作業の効率化を果たす 債権管理システム

一般の入金消込処理では、請求データと一対一になるよう、事前に得意先情報の付与などが必要となるが、Victory ONE (以降 VONE)では、照合処理で得意先情報を付与することで、事前の入金情報への色付けを行わずに効率的に消込処理が行える

辞書情報などを利用して、事前に得意先情報の設定なども可能

照合処理の概要としては、カナ情報、カナ学習履歴、専用口座情報、EBデータ上の数値との照合、得意先コードでの照合などが行える

EBデータなどの、得意先情報に乏しいデータに対しても、カナ学習履歴などを利用することで、効率的に得意先情報の特定が可能

得意先のグルーピング機能や、入金情報の担当者分割のための入金部門などの機能を所持している

## プロジェクト構成

現在、オンプレミスでの動作を想定している G4 (WCF サービスを利用する) と、pure cloud を目指す Web API とで、資産の共有を目指している

pure cloud 側の Web API では、 .NET Core (2.2) を採用している

帳票出力用に Rac.VOne.Web.Api.Legacy があるが、ライセンス使用料の問題で、利用しない

パフォーマンスや ライフサイクルの観点で、.NET Core を利用する方が望ましいため、現状は Rac.VOne.Web.Api の利用を行う

帳票出力を行う前に、Excelファイルの downloadができるよう Open XML SDK を利用して、*.xlsx ファイルの作成を行う

WCFサービスは、.NET Framework (4.7.1 or later) に依存している

そのため、共通で利用する DLL は .NET Standard (2.0) で定義している

|project|type|target framework|clinet/server|summary|
|-------|----|----------------|-------------|-------|
|Rac.Util.CodeGen|exe|framework|client|Rac.VOne.Web.Models のモデルを TypeScript へ変換するプロジェクト|
|Rac.VOne.Common|dll|standard|both|どちらでも共通の処理実装|
|Rac.VOne.EbData.Core|dll|standard|both|EBデータ取込用 Core DLL namespace は Rac.VOne.EbData のまま|
|Rac.VOne.AccountTransfer|dll|standard|both|口座振替 データ作成・結果取込のビジネスロジック|
|Rac.VOne.Web.Models|dll|standard|both|モデルの定義|
|Rac.VOne.Client.Reports.Core|dll|standard|both|帳票の設定などの定義 namespace は Rac.VOne.Client.Reports のまま|
|Rac.VOne.Client.Reports|dll|framework|both|帳票の実装 ActiveReports なので、.NET Framework 縛り *Api.Legacy で利用|
|Rac.VOne.Data|dll|standard|server|CRUDのinterface を定義|
|Rac.VOne.Data.SqlServer|dll|standard|server|CRUDのSQL Server 向け実装|
|Rac.VOne.Web.Common|dll|standard|server|web service のビジネスロジック実装 帳票出力、インポート処理の interface を定義|
|Rac.VOne.Web.Spreadsheets|dll|standard|server|web api の spreadsheet 出力ロジック Open XML SDK を利用して *.xlsx ファイルを作成|
|Rac.VOne.Web.Api|web app|core|server|web api の server 実装|
|Rac.VOne.Web.Reports|dll|framework|server|ActiveReports の出力処理実装|
|Rac.VOne.Web.Api.Legacy|web app|framework|server|ASP.NET Web API の server 実装 帳票対応のため、ActiveReports の PdfExport を実装 インポート処理も実装|
|Rac.VOne.Web.Service|wcf service|framework|server|WCF service 実装|
|Rac.VOne.Clinet|dll|framework|clinet|client用のinterface定義 third-party との連携用|
|Rac.VOne.Clinet.Common|dll|framework|client|client側共通処理|
|Rac.VOne.EbData|dll|framework|client|EBデータ取込用DLL WCF の サービス呼び出し実装が主な点|
|Rac.VOne.Export|dll|framework|client|CSV出力用実装|
|Rac.VOne.Import|dll|framework|client|CSVインポート処理実装|
|Rac.VOne.Message|dll|framework|client|client側メッセージ|
|Rac.VOne.Client.Screen|dll|framework|client|Windows Forms の内部で表示する画面のコンポーネント定義 クライアント側実装の本体|
|Rac.VOne.Clinet.Batch|exe|framework|client|バッチ処理の本体|
|Rac.VOne.Clinet.Forms|exe|framework|client|Windows Forms client 本体|
