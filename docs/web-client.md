# web client

Node.js がインストールされている必要がある

http://nodejs.org/


Visual Studio Code でターミナルウィンドウを表示し、下記コマンドを実施する

## npm-install

package.json ファイルが含まれる ディレクトリへ カレントディレクトリを変更し、下記コマンドを実施する

```terminal
> npm install
```

application に必要な library なども関連して install が行われる

## npm-start

Node.js の アプリケーションを実行

```terminal
> npm start
...
** Angular Live Development Server is listening on hostname:port, open our browser on http://hostname:port/ **
...
```
npm start 後に表示される hostname:port へアクセスすることで web client へアクセス可能

## 設定項目一覧

### web application の hostname, port

hostname, port は、下記で設定してある

/web/client/angular.json

line 56-57

```json
        "serve": {
          "builder": "@angular-devkit/build-angular:dev-server",
          "options": {
            "browserTarget": "client:build",
            "host": "localhost",
            "port": 4200
          },
          "configurations": {
            "production": {
              "browserTarget": "client:build:production"
            }
          }
        },
```
上記の host および port を変更することで、web client の host port を設定可

### web api に利用する パラメータ

/web/client/src/app/common/const

company.const.ts

TenantCode および CompanyCode が定義されている

```ts
export const TENANT_LIST = 
{ 
  192: "G4_406", 
  localhost:"G4_406",
  r_and_ac: "G4_406", 
  r_and_ac2: "G4_406", 
};

export const COMPANY_CODE="0013";
```

http.const.ts

web api の uri などが定義されている

```ts
export const WebApiUrl = 'http://s0017:8626/api/';  // URL to web api
```

### ログイン画面の初期パラメータ

/web/client/src/app/component/login/PA0100/pa0101-login/pa0101-login.component.ts

line 73-78
```ts
    private setControlInit() {

      this.userCodeCtrl = new FormControl("user-code",[Validators.required,Validators.maxLength(10)]);  // ユーザーコード
      this.passwordCtrl = new FormControl("password",[Validators.required,Validators.maxLength(15)]);  // パスワード
  
    }
```

上記の `user-code` および `password` を変更

### Visual Sutdio Code での Debugging

[Debugger For Chrome 拡張機能のインストール](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome)

ターミナルから、web client の起動
```terminal
npm start
```

Visual Sutdio Code の ワーキンググループ `.vscode` フォルダへ `launch.json` の追加

```json
{
    // IntelliSense を使用して利用可能な属性を学べます。
    // 既存の属性の説明をホバーして表示します。
    // 詳細情報は次を確認してください: https://go.microsoft.com/fwlink/?linkid=830387
    "version": "0.2.0",
    "configurations": [
        {
            "type": "chrome",
            "request": "launch",
            "name": "Launch Program",
            "program": "${file}",
            "url": "http://localhost:4200",
            "webRoot": "${workspaceFolder}/web/client/",
        }
    ]
}
```

url や webRoot などを設定し、[デバッグ]-[デバッグの開始]を選択し、起動することで、Visual Studio Code で設定した ブレークポイント などが有効になる

configurations の詳細は、 [Debugger for Chrome](https://marketplace.visualstudio.com/items?itemName=msjsdiag.debugger-for-chrome)を参照
