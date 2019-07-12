# 概要

## このスタイルガイドについて
[SC5スタイルガイドジェネレータ](http://styleguide.sc5.io/)にて作成。コメントの記述方法などは[こちら](https://github.com/SC5/sc5-styleguide/#inserted-markup)


## BEMの記法について
1. [MindBEMdingシンタックス](https://csswizardry.com/2013/01/mindbemding-getting-your-head-round-bem-syntax/)で記述する

        .block {}
        .block--modifier {}
        .block__element {}
        .block__element--modifier {}


1. クラス名は原則シングルクラスとする（block名が異なるクラス名は併記可能）

        例）block名"button"が重複しているので、不可
        <span class="button button--forward">
        例）block名が重複していないので、可
        <span class="button mt--10">

### クラス命名ルール
1. block名、element名が長くなる場合はキャメルクラスとする（ハイフン区切りは使用しない）

        例）.definitionForm

1. modifier名はキャメルクラスとハイフン区切りを併用

        例） .basicTable--tableData-grandTotal


## ディレクトリ、ファイル名
作成中
