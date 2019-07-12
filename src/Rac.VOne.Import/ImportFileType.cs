using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Import
{
    /// <summary>インポートファイルタイプ </summary>
    public static class ImportFileType
    {
        /// <summary> 1:  請求部門マスター </summary>
        public const int Department = 1;
        /// <summary> 2:  営業担当者マスター </summary>
        public const int Staff = 2;
        /// <summary> 3:  ログインユーザーマスター </summary>
        public const int LoginUser = 3;
        /// <summary> 4:  得意先マスター </summary>
        public const int Customer = 4;
        /// <summary> 5:  得意先マスター登録手数料 </summary>
        public const int CustomerFee = 5;
        /// <summary> 6:  得意先マスター歩引設定 </summary>
        public const int CustomerDiscount = 6;
        /// <summary> 7:  銀行口座マスター </summary>
        public const int BankAccount = 7;
        /// <summary> 8:  科目マスター </summary>
        public const int AccountTitle = 8;
        /// <summary> 9:  債権代表者マスター </summary>
        public const int CustomerGroup = 9;
        /// <summary> 10: 学習履歴マスター </summary>
        public const int KanaHistory = 10;
        /// <summary> 11: 入金・請求部門対応マスター </summary>
        public const int SectionWithDepartment = 11;
        /// <summary> 12: 入金部門・担当者対応マスター </summary>
        public const int SectionWithLoginUser = 12;
        /// <summary> 13: 入金部門マスター </summary>
        public const int Section = 13;
        /// <summary> 14: 長期前受契約マスター </summary>
        public const int BillingDivisionContract = 14;
        /// <summary> 15: 除外カナマスター </summary>
        public const int IgnoreKana = 15;
        /// <summary> 16: カレンダーマスター </summary>
        public const int HolidayCalendar = 16;
        /// <summary> 17: 通貨マスター </summary>
        public const int Currency = 17;
        /// <summary> 18: 法人格マスター </summary>
        public const int JuridicalPersonality = 18;
        /// <summary> 19: 銀行・支店マスター </summary>
        public const int BankBranch = 19;
    }
}
