using Rac.VOne.Common.Extensions;

namespace Rac.VOne.AccountTransfer.Export
{
    public class FixedField
    {
        public int Length { get; set; }
        public object Value { get; set; }
        public char PaddingChar { get; set; } = ' ';
        public bool RightPadding { get; set; }

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
        public FixedField(int length, object value, char padding = ' ', bool rightPadding = true)
        {
            Length          = length;
            Value           = value;
            PaddingChar     = padding;
            RightPadding    = rightPadding;
        }
        public override string ToString()
        {
            return RightPadding
                ? Value.ToString().Left (Length, PaddingChar, true)
                : Value.ToString().Right(Length, PaddingChar, true);
        }

    }
}
