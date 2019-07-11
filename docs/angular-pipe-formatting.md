# Angular での 数値/日付などの フォーマット

[`number`](https://angular.jp/api/common/DecimalPipe) や [`date`](https://angular.jp/api/common/DatePipe) などの `Pipe` を利用すると簡単にフォーマット指定が可能

## 日付の書式指定

カスタム書式設定のオプションを指定したい場合は、下記リファレンス参照

https://angular.jp/api/common/DatePipe

```html
<label>
  請求日
</label>
<div>
  {{ billing.billedAt | date }}
</div>
```

`LOCAL_ID` に `'ja'` が設定されていれば、書式を設定せずに `yyyy/MM/dd` 形式となる

通常は  `Apr 11, 2019` といった 米国基準の書式となる

時分秒などを表示したい場合は、`date:'yyyy/MM/dd HH:mm:ss'` など 書式設定を行う

LOCALE_ID を設定した 古い記述(v4時点)を参照するに、単純に date としたときに yyyy年M月d日 となったという記述があったので、書式設定を行った方が安全か…。

## 数値の書式指定

Angular には標準で 金額表示用の [`CurrencyPipe`](https://angular.jp/api/common/CurrencyPipe) が存在するが、 v5 以降通貨記号の非表示が行えなくなったため、[`DecimalPipe`](https://angular.jp/api/common/DecimalPipe)を利用する。

実際に html側で記述する場合は `number` とだけ書く。

数字の 桁区切りが行われて表示される。

```html
<label>
  請求額
</label>
<div>
  {{ billing.billingAmount | number }}
</div>
```

### 外貨対応

小数点以下桁数を任意の値にしたい場合、 `DecimalPipe` の [`digitsInfo`](https://angular.jp/api/common/DecimalPipe#parameters) オプションを指定する。

digitsInfo は string で、次のようなフォーマットで指定する

`{minIntegerdigits}.{minFractionDigits}-{maxFractionDigits}`

小数点以下桁数 2 の場合は `'1.2-2'` と指定

円貨で、小数点以下の桁数を表示したくない場合は、 `'1.0-0'` とすると、小数点も表示しない

円貨、外貨ともに対応したい場合、小数点以下桁数を component.ts 側に用意し、下記のように設定することで対応可能

```ts
// component.ts
private precision = 0;
```

```html
<!-- component.html -->
<label>
  請求額
</label>
<div>
  {{ billing.billingAmount | number: ('1.' + precision + '-' + precision) }}
</div>
```
通貨コードなどを呼び出し、通貨に設定されている小数点以下桁数 (VONE では presicion としてしまった)を設定することで、動的に 小数点以下桁数を変更できる。

## ts 側での利用

pipe インジェクトして、 transform メソッドを利用可能

