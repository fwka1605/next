# ASP.NET Core の JSON Serilaizer 変更

`ASP.NET Core 3` で 標準の JSON Serializer が `Json.NET` から変更される

[The future of JSON in .NET Core 3.0 #33115](https://github.com/dotnet/corefx/issues/33115)

`Json.NET` は、パフォーマンス上問題があるため、Jil や Utf8Json などへの変更を行うことでパフォーマンス向上が期待できる。

なお、現状の Web API/ web client では、Web API のパラメータの変更 web client 側の呼び出し方 変更など、単純に serializer を変更する以上に工数がかかる。

## Utf8Json への変更

### nuget package の install

```console
> install-package Utf8Json
> install-package Utf8Josn.AspNetCoreMvcFormatter
```

ASP.NET Core Web API で、serializer を変更する場合、上記のように２つの nuget package を install する。

### Startup の編集

[Setup of AspNetCoreMvcFormatter #150](https://github.com/neuecc/Utf8Json/issues/150)

`Json.NET` の 登録を削除し、`Utf8Json` を serializer として登録する

```cs
// C#
using Microsoft.AspNetCore.Mvc.Formatters;
using Utf8Json.AspNetCoreMvcFormatter;
using Utf8Json.Resolvers;
public class Startup {
  ...
  public void ConfigureSerices(IServiceCollection services) {
    ...

    services.AddMvc()
      .AddMvcOptions(options => {
        options.OutputFormatters.RemoveType(typeof(JsonOutputFormatter));
        options.InputFormatters.RemoveType(typeof(JsonInputFormatter));

        var resolver = CompositResolver.Create(
          EnumResolver.UnderlyingValue,
          StandardResolver.ExcludeNullCamelCase
        );

        options.OutputFormatter.Add(new JsonOutputFormatter(resolver));
        options.InputFormatter.Add(new JsonInputFormatter(resolver));
      })
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
  }
}
```

#### IJsonFormatter の設定

日付など、json で定義されている ISO8601形式 以外の日付を連携する場合、`IJsonFormatter` を resolver へ登録する

[Utf8Josn - Resolver](https://github.com/neuecc/Utf8Json#resolver)


```cs
// C#
  public void ConfigureServices(IServiceCollectin services) {
    ...
    services.AddMvc()
      .AddMvcOptions(options => {
        ...
        var resolver = CompositResolver.Create(
          new IJsonFormatter[] {
            new DateTimeFormatter("yyyy/MM/dd"),
            new NullableDateTimeFormatter("yyyy/MM/dd"),
          },
          new IJsonFormatterResolver[] {
            EnumResolver.UnderlyingValue,
            StandardResolver.ExcludeNullCamelCase
          }
        );
      })
      ...
  }

```


## C# と TypeScript の違い

C# では、class に記載された primitive(struct) 型は、new した時点で初期化される。

TypeScript では、new しても プロパティの値は `undefined` となっている

[TypeScript 2.7: Strict Property Initialization](https://mariusschulz.com/blog/typescript-2-7-strict-property-initialization)

初期化していない場合に TypeScript のコンパイルエラーを表示することは可能だが、プロパティは `undefined` のまま

```cs
// C#
public class Receipt {
  public long Id { get; set; }
  public int CustomerId { get; set; }
  public decimal ReceiptAmount { get; set; }
}

static void Main(string[] args) {
  var receipt = new Receipt();
  Console.WriteLine($"id:{receipt.Id}, customerId:{receipt.CustomerId}, receiptAmount:{receipt.ReceiptAmount}");
  // id:0, customerId:0, receiptAmount:0
}
```

```ts
// TypeScript
public export Recipet {
  public id: number;
  public customerId: number;
  public receiptAmount: number;
}

private test() {
  const receipt = new Receipt();
  console.log(`id:${receipt.id}, customerId:${receipt.customerId}, receiptAmount:${receipt.receiptAmount}`);
  // id:undefiend, customerId:undefined, receiptAmount:undefined
}
```
# 現状 判明している問題点

C# 上で、検索用のパラメータなどで、nullable であるべき項目が nullable となっていない

T4 Template engine で、not nullable として連携されるので、プリミティブ型を連携する際にデシリアライズできないとしてエラーとなる

TypeScript 上で、property が登録されているが、初期化されていない場合、undefined = null として連携される

```ts
const receipt = new Receipt();
receipt.id = 1,
receipt.customerId = this.customerId;

httpClient.post(uri, receipt);
```

```json
{
  "id":1,
  "customerId":1008,
  "receiptAmount":null
}
```

上記を Web API でデシリアライズする際に、例外が発生してしまう。

上記の `Receipt` の例では、連携されるJSON上、プロパティが省略されている場合は、C# の Web API でデシリアライズする際に default 値となる

```json
{
  "id":1,
  "customerId":1008
}
```

```cs
// web api
[HttpPost]
public post(Receipt receipt) {
  Console.WriteLine($"amount:{receipt.Amount}");
  // amount: 0
}
```
正常に デシリアライズできない場合、TypeScript側からのWeb API呼び出しは 500 Internal Server Error として失敗する。

