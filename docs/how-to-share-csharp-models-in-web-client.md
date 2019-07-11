# C# で作成した POCO の Model を web client 側で共有する方法

asp.net core web api で、 c# の モデル や モデルの配列を戻り値とした場合

結果として連携される json を、極力少ない労力で、model classs(interface でも)へ デシリアライズする方法を検討する

web client 側は TypeScript が利用できるので、 C# のクラスを TypeScript へ変換する方法を確立したい

実際には ツールや、T4TemplateEngine を使った書き方などの情報をもとに、自前である程度書かないと対応が難しいのかもしれない

web client 側で、deserialize して class に変換するにも、npm でライブラリなどが配布されているので、確認が必要
