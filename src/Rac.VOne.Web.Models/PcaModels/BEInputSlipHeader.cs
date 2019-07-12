using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>
    /// 伝票ヘッダー
    /// </summary>
    public class BEInputSlipHeader
    {
        /// <summary>
        /// 伝票ID
        /// 数値
        /// 4バイト整数
        /// 作成時以外は必須
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ロック区分
        /// 数値/文字列
        /// 0: None     : ロックなし
        /// 1: Input    : マスター・仕訳入力ロック
        /// 2: Reference: 参照ロック
        /// 作成・修正時は無視されます
        /// </summary>
        public int LockType { get; set; }
        /// <summary>
        /// 伝票日付
        /// 数値8桁
        /// 西暦(YYYYMMDD)
        /// </summary>
        public IntDate Date { get; set; }
        /// <summary>
        /// 伝票番号
        /// 数値5桁
        /// 1～99999
        /// 作成時は自動設定されます
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 仕訳区分
        /// 数値/文字列
        /// 11: Begin   : 月初
        /// 21: Normal  : 月次(初期値)
        /// 31: Close1  : 決算１
        /// 32: Close2  : 決算２
        /// 33: Close3  : 決算３
        /// ※ おそらく 11 が月次残高, 21 が通常仕訳, 31..3 は 決算修正用
        /// </summary>
        public int JournalClass { get; set; } = 21;
        /// <summary>
        /// 管理会計仕訳区分
        /// 数値/文字列
        ///  0: Financial   : 財務(初期値)
        ///  1: Manage1     : 管理仕訳１
        ///  2: Manage2     : 管理仕訳２
        ///  3: Manage3     : 管理仕訳３
        ///  4: Manage4     : 管理仕訳４
        ///  5: Manage5     : 管理仕訳５
        ///  6: Manage6     : 管理仕訳６
        ///  7: Manage7     : 管理仕訳７
        ///  8: Manage8     : 管理仕訳８
        ///  9: Manage9     : 管理仕訳９
        /// 10: Manage10    : 管理仕訳１０
        /// </summary>
        public int ManageClass { get; set; }
        /// <summary>
        /// 状態区分
        /// 数値/文字列
        /// 0: Temporary: 仮登録
        /// 1: Registered: 登録
        /// 9: Deleted: 削除
        /// 10: TemporaryDeleted: 仮登録削除
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 消込み状態
        /// 文字列
        /// false: 未消込(初期値)
        /// true: 消込済
        /// 作成時は自動設定されます
        /// </summary>
        public string VanishState { get; set; } = "false";
        /// <summary>
        /// 入力モジュール名
        /// 文字列
        /// 全角32文字/半角64文字
        /// 作成・修正時はメソッドのパラメータが自動設定されます
        /// </summary>
        public string InputModuleName { get; set; }
        /// <summary>
        /// 入力ユーザーID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int InputAreaUserId { get; set; }
        /// <summary>
        /// 承認ユーザーID①
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int ApprovalAreaUserId1 { get; set; }
        /// <summary>
        /// 承認ユーザーID②
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int ApprovalAreaUserId2 { get; set; }
        /// <summary>
        /// 承認ユーザーID③
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int ApprovalAreaUserId3 { get; set; }
        /// <summary>
        /// 承認ユーザーID④
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int ApprovalAreaUserId4 { get; set; }
        /// <summary>
        /// 修正前伝票ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int OldId { get; set; }
        /// <summary>
        /// 初代伝票ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int OrgId { get; set; }
        /// <summary>
        /// 本社支社情報ID
        /// 数値
        /// 4バイト整数
        /// </summary>
        public int HsId { get; set; }
        /// <summary>
        /// 予備1
        /// 数値
        /// 4バイト整数
        /// 作成時は自動設定されます
        /// </summary>
        public int Reserve1 { get; set; }
        /// <summary>
        /// 予備2
        /// </summary>
        public int Reserve2 { get; set; }
        /// <summary>
        /// 予備3
        /// </summary>
        public int Reserve3 { get; set; }
        /// <summary>
        /// 予備金額1
        /// 数値
        /// -922,337,203,685,477.5808 ～ 922,337,203,685,477.5807
        /// (小数4桁)
        /// </summary>
        public decimal ReserveMoney1 { get; set; }
        /// <summary>
        /// 予備金額2
        /// </summary>
        public decimal ReserveMoney2 { get; set; }
        /// <summary>
        /// 予備金額3
        /// </summary>
        public decimal ReserveMoney3 { get; set; }
        /// <summary>
        /// 予備文字列1
        /// 文字列
        /// 全角32文字/半角64文字
        /// </summary>
        public string ReserveString1 { get; set; }
        /// <summary>
        /// 予備文字列2
        /// 文字列
        /// 全角64文字/半角128文字
        /// </summary>
        public string ReserveString2 { get; set; }
        /// <summary>
        /// 予備文字列3
        /// 文字列
        /// 全角128文字/半角256文字
        /// </summary>
        public string ReserveString3 { get; set; }
        /// <summary>
        /// 更新日付時間
        /// 日付時間
        /// 作成時は自動設定されます
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
