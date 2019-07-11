〇ログイン時に以下の情報をUserInfoServiceクラスに保存している。
	１．AccessToken：HTTPヘッダーに付与するアクセストークン情報
	２．Company：企業情報
	３．ApplicationControl：アプリケーションコントロール情報
	４．MenyuAuthority：メニュー情報
	５．functionAuthority：機能情報

〇情報の参照方法
コンストラクターでサービスをInjectionして参照してください。

    constructor(
      private userInfoService:UserInfoService,
    ) {
       super(); 
    }