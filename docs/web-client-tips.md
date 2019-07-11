# web client の tips

## Angular Material の利用

[Angular Material](https://material.angular.io) は google が推進する material design の Angular 用コンポーネント群

```node
> npm install --save @angular/material @angular/cdk @angular/animations
```
上記コマンドを実施することで利用可能

### Expansion Panel

[Expansion Panel（展開パネル）](https://material.angular.io/components/expansion/overview)を利用することで、検索条件などを折りたたむことができる

コンポーネントは `mat-expansion-panel` から利用可能

プログラム側で、閉会を操作したい場合 `[expanded]` や、`[collapsed]` に、条件式を追加すると対応可能

また、 `mat-expansion-panel` では、標準の動きとして、複数の パネルが存在する場合に、開くことができるパネルは一つとなっている。

プロパティを変更することで、複数パネルを同時に開くことが可能となる。

https://material.angular.io/components/expansion/examples

```html
...
  <section class="contents invoice-data-search">
    <mat-expansion-panel
      [expanded]="isSearchPanelExpanded === true"
      (opened)="isSearchPanelExpanded = true"
      (closed)="isSearchPanelExpanded = false">
...
```

上記の例では、対となる component.ts へ、isSearchPanelExpanded という boolean 変数を用意して対応した。

### Drag and Drop

[@angular/cdk/drag-drop](https://material.angular.io/cdk/drag-drop/overview)

グリッド表示設定や、照合設定などでの 表示順の変更を Drag and Drop を利用して実装しようと検討した。

html 標準の table の中で、row `<tr>` を Drag and Drop 対応しようとしたが、対応が難しかった。

AppModule へ、`DragDropModule` を import する

drag and drop の領域を `cdkDropList` で指定する

drop のハンドラを `(cdkDropListDropped)` で指定する

drag 対象を html に記述する際、Reactive Froms を利用しているのであれば、対応する FormArray を `*ngFor` に指定する

ドラッグする項目が複数ある場合は、 `FormArray` には `FromGroup` を配列で格納しておく

子要素の入力項目を指定する際は、`[fromGroup]` へ let で取得した FormGroup を指定する

ドロップハンドラでは、下記のように `FormArray` の control と value を入れ替える

```ts
  private onDropInner(event: CdkDragDrop<string[]>, orders: FormArray) {
    moveItemInArray(orders.controls,  event.previousIndex, event.currentIndex);
    moveItemInArray(orders.value,     event.previousIndex, event.currentIndex);
  }
```