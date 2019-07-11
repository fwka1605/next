# 帳票

今回 Web API 側での帳票作成は、帳票のPDF を作成し、DLできるようにすること

## ActiveReports

ActiveReports では、HTML5ビューワ (Professional限定) を持っており、javascriptライブラリとしてWeb アプリケーションにビューワを組み込めるということ。

現在(2018/12/11) .NET Core への対応は行われていない


[ActiveReports for .NET 12.0J](https://www.grapecity.co.jp/developer/activereports)

フレームワーク	.NET Framework 4.5.2／4.6／4.6.1／4.6.2／4.7／4.7.1／4.7.2


[ActiveReportsの「ASP.NET Core 非対応」を回避する方法](https://devlog.grapecity.co.jp/entry/2018/07/27/activereports-dotnet-core)

概要

ASP.NET Core を利用する Web アプリケーションと .NET Framework で動作するレポートサーバーを2台用意し、HTML5ビューワを介してプレビューを表示するという方針

[.NET Core対応に向け開発進行中！ActiveReportsの構成要素をおさらいする](https://devlog.grapecity.co.jp/entry/2018/08/20/activereports-components)

概要

.NET Core の対応は、クラスライブラリのみで、帳票エンジンとエクスポートライブラリのみ.NET Standard で書き直す想定だそう。

[How to use ActiveReports in ASP.NET Core 2.0 and Razor pages](https://www.grapecity.com/en/blogs/how-to-use-activereports-in-aspnet-core-20)

ASP.NET (.NET Framework)でレポート用サービスを作成し、.NET Core から参照する方法のtutorial

HTML5 Viewer を利用するので、Professional ライセンスが必要

ASP.NET を .NET Framework で動作させる場合は、ActiveReports での動作が行える

下記 プロジェクトで、動作するソースをアップ済

http://gitlab.r-ac.local/sasaki/activereports-to-pdf

ASP.NET Core から ASP.NET Web API へ移行することで...

※ Kestrel を利用した Cancellation などが利用できないので、 web api に仕込んだ CancellationToken が無駄に...

→ 一括で置換して あとあと 復元するようにした方がよいかも...

swagger (/swashbuckle) が動作するかが不明


## DinkToPdf

.NET Core 対応
WebKit エンジンを使って HTML から PDF へ変換
出力する帳票イメージの HTML を作成する必要がある

[DinkToPdf](https://github.com/rdvojmoc/DinkToPdf)

MIT License

[How to Easily Create a PDF Document in ASP.NET Core Web API](https://code-maze.com/create-pdf-dotnetcore/)

tutorial

## SQL Server Reporting Services

[Reporting Services Web Services with .NET CORE 2](https://blogs.msdn.microsoft.com/dataaccesstechnologies/2017/09/19/reporting-services-web-services-with-net-core-2/)

SQL Server Reporting Services を Windows インスタンスに構築し、reporting させるという方法

ActiveReports の レポートサーバーを立てるイメージと近しいかもしれない
