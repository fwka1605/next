using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>伝票</summary>
    public class BEInputSlip
    {
        /// <summary>
        /// エンティティバージョン
        /// 整数
        /// 4バイト整数
        /// ＤＸシリーズ
        /// Rev1.00～：2
        /// Xシリーズ
        /// Rev4.50～：2
        /// Rev1.00～：1
        /// 常に2を返します
        /// </summary>
        public int BEVersion { get; set; } = 2;
        /// <summary>
        /// 伝票状態区分
        /// 数値/文字列
        /// 0: Modifiable: 修正可能
        /// 1: Locked: 処理中
        /// 2: Deleted: 削除済み
        /// 3: Approved: 承認済み
        /// 4: BeforeJournalLockDate:
        /// 仕訳締切日以前
        /// 5: Vanished: 消込済み
        /// 6: NoAlterRight: 変更・削除権限なし
        /// 7: LockedMasterInput: 入力ロックされたマスターが使用されている
        /// 8: LockedMasterRef: 参照ロックされたマスターが使用されている
        /// 9: AnotherInputProgram: 他処理にて登録された伝票
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int SlipState { get; set; }
        /// <summary>
        /// 伝票ヘッダー
        /// </summary>
        public BEInputSlipHeader InputSlipHeader { get; set; }
        /// <summary>
        /// 伝票明細リスト
        /// 最大999件
        /// </summary>
        public InputSlipDataList InputSlipDataList { get; set; }
        /// <summary>
        /// 消費税自動計算フラグ
        /// 文字列
        /// false： 作成・修正時、指定された消費税額をそのまま登録する
        /// true： 作成・修正時、金額から消費税額を計算して登録する
        /// 作成・修正時は自動設定されます
        /// </summary>
        public string TaxOrgMoneyCalcedByAs { get; set; }
        /// <summary>
        /// 自分仕訳貸借区分
        /// 数値/文字列
        /// 0: None: 指定なし
        /// 作成・修正時は無視されます
        /// </summary>
        public int MainDrCrMode { get; set; }
        /// <summary>
        /// 恒久ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は無視されます
        /// </summary>
        public int PermanentId { get; set; }
    }

    public class BEInputSlipSet
    {
        public BEInputSlip BEInputSlip { get; set; }
    }
    public class ArrayOfBEInputSlip
    {
        public List<BEInputSlip> BEInputSlip { get; set; }
    }
    public class BEInputSlipsSet
    {
        public ArrayOfBEInputSlip ArrayOfBEInputSlip { get; set; }
    }
}
