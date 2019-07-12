export  class AutoGetSetUtil{

    public static autoGetSet():void{

        console.log("autoGetSet");
        console.log(this);

        //宣言済みの内部変数全体を対象とする
        Object.keys(this).forEach(function (prop) {
            //内部変数の先頭から"_var"を切取って外部アクセス用変数名を定義する
            let realprop = prop.replace(/^_+/, '');

        //先頭文字から"__"である場合、読込専用なのでgetterのみ定義する
        if (
           prop[0] === '_' &&
           prop[1] === '_'
        ){
            console.log("autoGetSet - ");
            Object.defineProperty(
               this, realprop, 
               {
                    get: function () {
                        return this[prop];
                    }
                }
            )
       }
       //先頭から"_"である場合、getterとsetterを定義する
       else if (
           prop[0] === '_' &&
           prop[1] !== '_'
        ){
            console.log("autoGetSet -- ");
               //値設定前に実行されるインターセプタ関数名：　beforeSet外部アクセス用変数名
               //値設定後に実行されるインターセプタ関数名：　beforeSet外部アクセス用変数名
               let beforeInterceptorFuncName = "beforeSet" + realprop;
               let afterInterceptorFuncName = "afterSet" + realprop;

           Object.defineProperty(this, realprop, {
               get: function () {
                   return this[prop];
               },
               set: function (val) {let oldval = this[prop];
                   //インターセプタが登録されている場合セッターの前後でコードを実行する
                   this[beforeInterceptorFuncName] && this[beforeInterceptorFuncName](val);
                   this[prop] = val;
                   this[afterInterceptorFuncName] && this[afterInterceptorFuncName](val);
               }
           })
       }
       else{
           console.log("erroraa");
       }
   }, this);
    }
}
