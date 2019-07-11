# pdf generator

どこで、PDFファイルを生成するか？

web api ? or web client ?

PDFファイルの生成は、そこそこのコンピュータ資源を要求するため、負荷がかかる


web api -> c# や その他

web client の場合は JavaScript や PHP その他


----

## c# で利用可能な pdf 作成ライブラリのまとめ

*   Azure web app sandbox unsupported frameworks

    https://github.com/projectkudu/kudu/wiki/Azure-Web-App-sandbox#unsupported-frameworks

    github などで公開されている pfd library は Rotativa などに代表されるように、wkhtmltopdf を利用しているものが多い

    wkhtmltopdf は、C++ で作成された LGPL v3 のライブラリ

    cloud なので 配布 はしないが、オンプレミスで帳票も利用したいという用途の場合、配布にあたるため、LGPL v3 に沿う必要がある

    また、上記記事にある通り、wkhtmltopdf は font に関する問題もある

*   wkhtmltopdf c++ LGPLv3

    https://wkhtmltopdf.org

    https://github.com/wkhtmltopdf/wkhtmltopdf

*   webgio/Rotativa.AspNetCore

    Rotativa for Asp.Net Core  (wkthmltopdf利用)

    https://github.com/webgio/Rotativa.AspNetCore

*   pruiz/WkHtmlToXSharp

    C# wrapper wrapper (using P/Invoke) for the excelent Html to PDF conversion library wkhtmltopdf library.

    https://github.com/pruiz/WkHtmlToXSharp

*   codaxy/wkhtmltopdf

    C# wrapper around excellent wkhtmltopdf console utility.

    https://github.com/codaxy/wkhtmltopdf

*   rdvojmoc/DinkToPdf

    C# .NET Core wrapper for wkhtmltopdf library that uses Webkit engine to convert HTML pages to PDF.

    https://github.com/rdvojmoc/DinkToPdf


*   ArthurHub/HTML-Renderer

    Cross framework (WinForms/WPF/PDF/Metro/Mono/etc.), Multipurpose (UI Controls / Image generation / PDF generation / etc.), 100% managed (C#), High performance HTML Rendering library.

    https://github.com/ArthurHub/HTML-Renderer


*   gmanny/Pechkin

    .NET Wrapper for WkHtmlToPdf static DLL. Allows you to utilize full power of the library.

    https://github.com/gmanny/Pechkin

*   tuespetre/TuesPechkin 

    A .NET wrapper for the wkhtmltopdf library with an object-oriented API.

    https://github.com/tuespetre/TuesPechkin

*   Api2Pdf/api2pdf.dotnet

    C# client library for the Api2Pdf.com REST API - Convert HTML to PDF, URL to PDF, Office Docs to PDF, Merge PDFs with AWS Lambda https://www.api2pdf.com

    api2pdf という Saas の client

    帳票ごとに課金が発生する

    https://github.com/Api2Pdf/api2pdf.dotnet

*   GZidar/CorePDF

    A basic PDF library that works with .net core

    MITライセンス html to pdf というわけではなく、直接PDFの要素を記述する必要があり、一つの帳票作成に大幅な工数が見込まれそう

    star 数や、1年以上更新されていない点などが不安

    https://github.com/GZidar/CorePDF

*   itext/itextsharp AGPL Commercial Use Dual License

    .NET port of the iText library http://itextpdf.com

    古い情報では、ライセンス料が 20-30万 かかるとのこと

    詳細は要問合せ 日本語での問合せは XLSoft などが代理店となっている模様？

    https://github.com/itext/itextsharp

*   Open XML SDK -> PDF

    microsoft により、office document の標準化が行われた

    nuget により、Open XML SDK を利用し、*.xlsx を作成することが可能となった。

    暫定的に *.xlsx を dl し、pdf へ変換してもらうという手もある

    Open XML to PDF は iTextSharp などの third-party 製の library が必要になる

----

備考

Running an EXE in a WebRole on Windows Azure

https://www.codeproject.com/Articles/331425/Running-an-EXE-in-a-WebRole-on-Windows-Azure

wkhtlmtopdf などを 上記のような仕組みで動作させているものも多い


ラクス様から頂いたPDF作成ライブラリについての情報

働くDBという、ユーザーDBのクラウドサービスで、
テンプレートのエクセルをユーザーがアップロードし、
DBの情報を帳票としてPDF形式で取得する機能がある

上記機能で、PDF作成ライブラリの検討を行った結果

|ライブラリ名|検討結果|注意点|再現率|価格|開発容易性|ユーザー利用容易性|保守性|
|:-----------|:-------|:-----|:-----|:---|:---------|:-----------------|:-----|
|LibreOffice|再現率はOpenOfficeに比べて高いが、表示幅が変わることで、改ページがずれやすい。他はOpenOfficeと同じ|Linux機からの表示は、特にずれるので、工夫が必要（フォントを配置するとか）ずれないように調整するのは至難。実行時にCPUを占有する。Xlsm形式のファイルは利用できない。|△|〇|△|〇|△|
|Aspose|再現率は高く、プログラムも簡単に対応できる。|CPU負荷はLibreOffice同様にかかる。Windowsフォント（MSゴシックなど）を使っていると、異なるフォントとなる。ノウハウがあまりない。|〇|×|〇|〇|△|
|easyPDF|再現率は最も高い（そもそもExcelを中継してPDF出力しているので、当然）xlsmも変換可能|Windows機上でしか動作しない 海外製 Microsoft Office 2010 以降が推奨 サーバにOfficeのインストールが必要|〇|×|△|〇|△|
|OpenOffice|結合セルに弱い フォントの再現率が低い Linux機からの表示は、特にずれるので、工夫が必要（フォントを配置するとか）|Linux機からの表示は、特にずれるので、工夫が必要（フォントを配置するとか）実行時にCPUを占有する。Xlsm形式のファイルは利用できない。|△|〇|△|〇|△|
|cloudconvert|xlsmも変換可能|クラウド連携となる。改ページの設定が無視された海外製|×|△|△|〇|〇
|Fleekform|デザインは専用UIではなく、Excel。そのため設定代行は不要な模様。||〇|△|×|〇|〇|
|OPROARTS|salesforceやkintoneも採用している。デザインはユーザ側（もしくは設定代行）で実施。その後に働くDBでキーワードへの埋込を実施。||〇|△|×|△|〇|
|帳票スクエア|salesforceやkintoneも採用している。デザインはユーザ側（もしくは設定代行）で実施。その後に働くDBでキーワードへの埋込を実施。||〇|△|×|△|〇|
|SVF Cloud|salesforceやkintoneも採用している。デザインはユーザ側（もしくは設定代行）で実施。その後に働くDBでキーワードへの埋込を実施。||〇|△|×|△|〇|
|JasperReports|上と同じ。デザインを自分で作ってそこにデータを流し込む。||〇|〇|×|×|×|


java 系の library が多いが、 Aspose は .net 向けの library あり

使用方法も簡単なので、これ一択となりそう

```console
> install-package Aspose.Cells
```

nuget での install が可能

Pdf への変換は、下記のコードのようにすれば可能

```cs
using Aspose.Cells;
using System.IO;

private void TestConversion() {
  var source = "temp.xlsx";
  var destination = "temp.pdf";
  if (File.Exists(destination))
    File.Delete(destination);

  var workbook = new Workbook(source);
  var option = new PdfSaveOptions(SaveFormat.Pdf);
  workbook.Save(destination, option);
}
```
Save メソッドに、Stream を引数に受けるものがあるので、MemoryStream を渡せば、Temporary なファイルを作成する必要もない

なお、ライセンスがない環境でビルドしたものは、PDFなどに評価用ですよという文言が表示される