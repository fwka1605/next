
    export class FixedField
    {
        private length:number;
        private value:Object;
        private paddingChar:string = ' ';
        private rightPadding: boolean;

        /// <summary>
        /// <see cref="FixedFiled"/>のコンストラクタ
        /// 通常文字列の場合は、rightPadding : true
        /// 数字型の場合は、 padding : '0', rightPadding : false
        /// ※ ２バイト文字の取り扱いがある場合は破綻するので、Left / Right の関数を変更して対応すること
        /// </summary>
        /// <param name="length">文字列長</param>
        /// <param name="value">元となる値</param>
        /// <param name="padding">paddingChar</param>
        /// <param name="rightPadding">右側を padding するか</param>
        constructor(length:number, value:object, padding:string = ' ', rightPadding:boolean = true)
        {
            this.length = length;
            this.value = value;
            this.paddingChar = padding;
            this.rightPadding = rightPadding;
        }

        public ToString():string
        {
            return this.rightPadding
                ? this.paddingRight()
                : this.paddingLeft();
        }

        private paddingLeft():string{
            return (String(this.paddingChar).repeat(this.length) + this.value.toString()).substr((this.length * -1), this.length) ;
        }

        private paddingRight():string{
            return (this.value + String(this.paddingChar).repeat(this.length)).substring(0, this.length);
        }

    }