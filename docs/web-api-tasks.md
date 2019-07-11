# next vone Web API 側 タスク一覧

2019/03/18 更新

## これから行うこと

1.  仕訳連携処理の検討・実装 ?? 実装時期未定

    汎用仕訳出力対応 工数 30 day

1.  帳票取得の Web API 未実装部分の実装

    ビジネスロジックが クライアント(*.Screen) に記載されているもので、工数がかかるものは未実装

    消込履歴データ検索/請求書/督促関連

1.  ActiveReports の置き換え

    請求残高年齢表 / 滞留明細一覧

1.  利用申請からサービス開始までの手順書作成

    場合によっては、UIを備えた何らかのプログラムを作成する

    申請に必要な項目、どのような処理を行うかの要件決めが必要

1.  ログイン画面の修正

    G4 で実装している、ログインユーザーのパスワード変更

    パスワード失念時の処理を用意するかどうか

        メールアドレスの入力？ ログインユーザーのメールアドレス必須化や、SendGrid を利用した バウンスでの正当性チェックなどをおこなうかどうかなど

1.  Web client 開発 ドキュメント の作成

    現状、G4 ライクであるが、異なる部分などが文書化されていない


## 実施済

1.  .NET 共通ライブラリ 利用方法確認 (.NET Standard/.NET Framework/.NET Core)

    g4 と next vone とで、ソースの個別管理を行わずに済む様に 共通化する方法を確認

1.  ASP.NET Core Middleware / Filter 確認

    asp.net core web api の 本体処理の前後で、処理を挟み込む方法を確認

1.  ASP.NET Core DependencyInjection 確認

    asp.net core での DI 利用方法を確認 SimpleInjector で利用していた箇所の書き換え実施済

1.  ASP.NET Core 認証処理確認 / db 接続変更方法確認 / Cors 対応

    asp.net core web api に カスタムリクエストヘッダーを利用する方法を確認 

1.  ASP.NET Core SignalR 確認

    asp.net core signalr の動作確認 無印 signalr と動作が異なることを確認

1.  ASP.NET Core Cancellation の動作確認

    Controller の メソッドに CancellationToken を 引数として追加しておけば、自動的に接続状態に基づく CancellationTokenSource を結び付けてもらえた。

    Kestrel でしか動作しないようだが、クエリなどがキャンセルされる様子は確認済

1.  Rac.VOne.Web.Common の内部を 非同期で実装

    あわせて Rac.VOne.Web.Common に ビジネスロジックを寄せたので、基本的に WCF や Web API は 表層のみ取り扱う形になった

1.  ASP.NET Web API へ 移行

    帳票を利用する関係上 .NET Core ではなく、.NET Framework で動作するよう ASP.NET Web API で動作するように修正済

    .NET Core のプロジェクトは Rac.VOne.Web.Api とし、 .NET Framework のプロジェクトは Rac.VOne.Web.Api.Legacy とする

1.  帳票を取得するAPI の作成

    ASP.NET Web API で、ActiveReports を利用した PDF ファイルを取得する メソッドを追加

1.  ASP.NET Web API の CORS 対応

    対応済

1.  各Controller の引数対応 FromBody

    対応済 ... reminder ...

1.  帳票取得の Web API 実装 (2019/01 末予定)

    その他 画面のUI によって出力内容が変わるものなどは モデルの定義から対応が必要となる

    一部 ビジネスロジックが クライアント(*.Screen) に記載されているもので、工数がかかるものは抜粋して、移植の工数を出す

1.  ASP.NET Web API の 包括的 例外処理の実装

    ASP.NET Core では Middleware で対応していた 八塚さんに対応してもらった。

1.  import 処理

    各種マスター /EBデータ/入金・請求・入金予定フリーインポーター/口座振替結果取込 の 処理実装済
