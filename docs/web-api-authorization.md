# Web API の 認証処理まとめ

1.  フロントエンドサーバーのみから、ログイン系のアクセスを認める

    IPAddress を制限した Filter を作成する

1.  ログイン成功時に、アクセストークンを発行する

    発行した アクセストークンを使って、他 Web API の処理を行う

1.  セキュリティ的な懸念事項を洗い出しておく

1.  Middleware や ActionFilter の実装について

    おおまかな実装方法についてまとめる

    *  Middleware の実装

        Middleware は、特段何かの interface や class の継承を必要としない

        メソッド名で、`Invoke` という名称を実装すると呼ばれる

        条件付けで Middleware を適用したい場合、`MapWhen` や `UseWhen` を利用する

        `MapWhen` は 完全に分岐するので、条件に合致するものは、その後の処理も記述する必要がある

        `UseWhen` は 通常の処理に特定の処理を差し込む場合に利用する

        [Conditional middleware based on request in ASP.NET Core](https://www.devtrends.co.uk/blog/conditional-middleware-based-on-request-in-asp.net-core)

        上記条件付けでの動作は、context.Request.Path などの Uri などを利用する

        Middleware は Filter よりも 外側に位置し、処理前は　Middleware > Filter

        処理後は Filter > Middleware の順に処理が行われる

        `HttpContext` を利用することで、request の情報へアクセスする

        Middleware と Filter との実装の違いは MVC Context を所持するか否か

        Filter 系は、 MVC の Context を利用したい場合に利用する

        ※ DB の接続先切替を Interface へ値を設定することで対応する想定だが、Middleware だと、static な状態で動作するため、Filter で実装した。

        また、Middleware 本体への DI は、SimpleInjector で設定したものが注入されなかったので、asp.net core の DI コンテナを利用した。

        https://stackoverflow.com/a/42583583

    * Filter の実装

        Filter 実装についてのあれこれをまとめておく

1.  Authorization in ASP.NET Core

    [Authorization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/)

    上記 URL に関連する内容をまとめる


middleware / filter の naming について

何を実施するか

custom header 2種類 で DB の接続先を決定するパティーン

access_token から DB接続先を決定するパティーン

上記　2種類 の middleware で期待される動き filter で期待される動きに 適切な名前を付ける


