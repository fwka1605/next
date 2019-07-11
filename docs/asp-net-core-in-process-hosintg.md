# ASP.NET Core の IIS における In Process Hosting

IIS を 単なる proxy として利用するのではなく、ASP.NET Core の web application を InProcess Hosting することが可能

[ASP.NET Core In Process Hosting on IIS with ASP.NET Core 2.2](https://weblog.west-wind.com/posts/2019/Mar/16/ASPNET-Core-Hosting-on-IIS-with-ASPNET-Core-22)

*.csproj や web.config にも　専用の記述が必要

通常は （IIS の） OutOfProcess として動作する

なお、InProcess でホストした場合、デバッグなどのアタッチができないとのこと。

OutOfPorcess と InProcess でホストした場合の違いは、reverse proxy となる IIS と web application の Kestrel との間で、html の通信がなくなる分 パフォーマンスが改善される。

また、Kestrel では、connection が切れた場合の cancellation が行われるが、IIS を 単なる proxy として利用する場合には、cancellation が行われない。

