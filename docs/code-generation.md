# C# でのコード自動生成

Web API / Web Front end で Model の共有を行いたいが、Web Front 側は、TypeScript を利用するといっても javascript

C#(.NET)の Refrection を利用して、コードの自動生成を行う手段を確立する

[C#で半自動ソースコード生成を行う](https://qiita.com/skitoy4321/items/bea79049d2b156f64d78)

やり方を まとめて記録しておくこと

## T4 Template Engine を利用する方法

G4 では Windows Forms 側で、WCF サービスの URL を変更するのに T4 Template Engine を利用して、ServiceClient に partial class を作成し、 attribute を付与している。

[T4 テキスト テンプレートの作成](https://docs.microsoft.com/ja-jp/visualstudio/modeling/writing-a-t4-text-template)

T4 Template 本体に、複数ファイル出力の機能はない

T4 Template コードブロックで利用可能な `StringBuilder GenerationEnvironment { get; }` プロパティにアクセスすると、生成された文字列を取得することが可能

上記を利用して、適切なタイミングでファイルを出力する記述が必要

[How to generate multiple outputs from single T4 template(archive)](https://web.archive.org/web/20160501025241/http://www.olegsych.com/2008/03/how-to-generate-multiple-outputs-from-single-t4-template/)


T4 template engine を利用して、複数ファイルに分ける方法

https://stackoverflow.com/a/44340464

Reflection を利用して、クラス名の Pascal -> lower camel 変換などの実施

型情報の変更などを実施する

TypeScript で対応可能な 型などを調べておく

TypeScript では、strictNullCheck などの機能があるので、null可能な項目については、わざわざ 型共有で null を指定する必要がありそう

```typescript
public class Sample {
    public NullableInt: number | null
}
```

メソッドの変換処理は難しそう

その他、コード自動生成の方法についても一通り確認しておく

## Roslyn Analyer を利用する方法


## Scripty (CSX) を利用する方法

## Buildalyer を利用する方法

## System.Reflection を 利用した 型情報などの取得について

今回実施したい TypeScript への変換は、型情報として、`string`, `number`, `Date`, `object`, `any` などを想定

.NET 側では、コレクション系の表現方法が多様であるが、TypeScript では配列だけなので、クラスの置き換えをおこなうのに注意が必要

### 大まかな方針

*   .NET 側の数値系の型は、すべて `number` にする
*   コレクション系はすべて単純な配列になる `T[]`
*   `Nullable<T>` は、`type | null` として型定義
*   プロパティは getter/setter `(get/set)` を使って定義
*   .NET では string は参照型なので、null 許可

### 以下は変換にまつわるあれこれ

#### `Nullable<T>` かどうかの判定方法

```cs
public stati bool IsNullable(this Type type)
    => Nullable.GetUnderlyingType(type) != null;
```

#### generics の `<T>` を取得する方法

```cs
Type type
var elementType = type.IsGenericType ? type.GetGenericArguments().First() : type;
```

#### 配列かどうかの判定と、型情報の取得

```cs
Type type
var elementType = type.IsArray ? type.GetElementType() : type;
```

#### コレクション系かどうか

```cs
Type type
var isCollection = type.GetInterfaces().Any(x
    => x.IsGenericType
    && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
```
配列 `T[]` や ジェネリック型 コレクション系のクラスは、すべて `IEnumerable<T>`を実装しているので、`IEnumerable<>` を実装しているかを確認している。

なお、`string` は、`IEnumerable<char>` を実装しているので、上記ではコレクション系として判定されてしまう

そのため、型情報が `string`ではないことを事前に確認する必要がある



下記 参考にした情報

[How to: Identify a nullable type (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/nullable-types/how-to-identify-a-nullable-type)

[Find type of nullable properties via reflection](https://stackoverflow.com/a/5644623)

[NrsLib.CSharpToTypescriptInterface](https://github.com/nrslib/NrsLib.CSharpToTypescriptInterface)

