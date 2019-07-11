# Azure Logic Apps

Azure での Task Scheduler をどのように実装するか調査するにあたり、Azure Scheduler という機能が検索にヒットした。

Azure Scheduler は Azure Logic Apps で代替可能なようで、Azure Logic Apps を利用することになる模様

Azure Logic Apps は Web の画面からや、PowerShell から ジョブを作成することが可能な模様

ジョブにトリガーを設定することで、 Task Scheduler のように定時で実行されるジョブを定義できる模様

どこかでテストを行わないといけない

ジョブ自体は C#, F#, JavaScript (Node.js) などで定義することが可能な模様

[Azure Logic Apps ドキュメント](https://docs.microsoft.com/ja-jp/azure/logic-apps/)

[Azure Logic Apps REST API](https://docs.microsoft.com/ja-jp/rest/api/logic/)

REST での API が用意してあるので、ジョブ自体を定義することで、api を経由して 処理をキックすることが可能な模様
