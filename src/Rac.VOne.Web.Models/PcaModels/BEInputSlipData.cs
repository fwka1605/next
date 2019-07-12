using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>伝票明細</summary>
    public class BEInputSlipData
    {
        /// <summary>
        /// 伝票ヘッダーID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int JournalHeaderId { get; set; }
        /// <summary>
        /// 行番号
        /// 数値3桁
        /// 1～999
        /// 作成・修正時は、コレクションの並び順で自動設定されます
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// 借方仕訳ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int DrId { get; set; }
        /// <summary>
        /// 借方税計算モード
        /// 数値/文字列
        /// 0: NotCalc  : 税計算しない(初期値)
        /// 1: Inside   : 内税自動計算
        /// 2: Outside  : 外税自動計算
        /// </summary>
        public int DrTaxCalcMode { get; set; }
        /// <summary>
        /// 借方部門ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時について
        /// 部門を指定する場合、会社基本情報の部門管理設定に従い、適切に部門を指定してください
        /// ロック時は、検索時に取得した値をそのまま設定してください
        /// <管理しない>
        /// 必ず共通部門(削除不可部門)を指定してください
        /// <損益計算書科目>
        /// 借方科目がPL科目以外ならば共通部門(削除不可部門)を指定してください
        /// </summary>
        public int DrBuId { get; set; }
        /// <summary>
        /// 借方科目ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は、借方科目に補助が登録されている場合は必須
        /// </summary>
        public int DrKmkId { get; set; }
        /// <summary>
        /// 借方補助ID
        /// 数値
        /// 4バイト整数
        /// </summary>
        public int DrHojoId { get; set; }
        /// <summary>
        /// 借方税区分ID
        /// 数値
        /// 4バイト整数
        /// </summary>
        public int DrTaxClassId { get; set; }
        /// <summary>
        /// 借方税区分対応消費税科目ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は指定された借方税区分ID情報から自動設定されます
        /// </summary>
        public int DrTaxKmkId { get; set; }
        /// <summary>
        /// 借方金額
        /// 数値
        /// 金額11桁(マイナスは10桁)
        /// </summary>
        public decimal DrMoney { get; set; }
        /// <summary>
        /// 借方消費税額
        /// 数値
        /// 金額10桁(マイナスは9桁)
        /// </summary>
        public decimal DrTaxMoeny { get; set; }
        /// <summary>
        /// 借方スタンプ
        /// 数値/文字列
        ///  0: None    : スタンプなし
        ///  1: Stamp1  : ①
        ///  2: Stamp2  : ②
        ///  3: Stamp3  : ③
        ///  4: Stamp4  : ④
        ///  5: Stamp5  : ⑤
        ///  6: Stamp6  : ⑥
        ///  7: Stamp7  : ⑦
        ///  8: Stamp8  : ⑧
        ///  9: Stamp9  : ⑨
        /// 10: Stamp10 : ⑩
        /// 11: Stamp11 : ⑪
        /// 12: Stamp12 : ⑫
        /// 13: Finish  : 済
        /// 14: Off     : 消
        /// 15: In      : 入
        /// 16: Out     : 出
        /// 17: Decision: 決
        /// 18: Temp    : 仮
        /// 19: Approval: 承
        /// 20: Wait    : 待
        /// 21: Unproven: 未
        /// </summary>
        public int DrStamp { get; set; }
        /// <summary>
        /// 借方銀行振込転送区分
        /// 数値/文字列
        /// 0: Calc     : 集計する
        /// 1: NotCalc  : 集計しない
        /// 2: Finish   : 転送済み(集計しない)
        /// </summary>
        public int DrBankTransferState { get; set; }
        /// <summary>
        /// 借方予備1
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は自動設定されます
        /// </summary>
        public int DrReserve1 { get; set; }
        /// <summary>
        /// 借方予備2
        /// </summary>
        public int DrReserve2 { get; set; }
        /// <summary>
        /// 借方予備3
        /// </summary>
        public int DrReserve3 { get; set; }
        /// <summary>
        /// 貸方仕訳ID
        /// </summary>
        public int CrId { get; set; }
        /// <summary>
        /// 貸方税計算モード
        /// </summary>
        public int CrTaxCalcMode { get; set; }
        /// <summary>
        /// 貸方部門ID
        /// </summary>
        public int CrBuId { get; set; }
        /// <summary>
        /// 貸方科目ID
        /// </summary>
        public int CrKmkId { get; set; }
        /// <summary>
        /// 貸方補助ID
        /// </summary>
        public int CrHojoId { get; set; }
        /// <summary>
        /// 貸方税区分ID
        /// </summary>
        public int CrTaxClassId { get; set; }
        /// <summary>
        /// 貸方税区分対応消費税科目ID
        /// </summary>
        public int CrTaxKmkId { get; set; }
        /// <summary>
        /// 貸方金額
        /// </summary>
        public decimal CrMoney { get; set; }
        /// <summary>
        /// 貸方消費税額
        /// </summary>
        public decimal CrTaxMoeny { get; set; }
        /// <summary>
        /// 貸方スタンプ
        /// </summary>
        public int CrStamp { get; set; }
        /// <summary>
        /// 貸方銀行振込転送区分
        /// </summary>
        public int CrBankTransferState { get; set; }
        /// <summary>
        /// 貸方予備1
        /// </summary>
        public int CrReserve1 { get; set; }
        /// <summary>
        /// 貸方予備2
        /// </summary>
        public int CrReserve2 { get; set; }
        /// <summary>
        /// 貸方予備3
        /// </summary>
        public int CrReserve3 { get; set; }
        /// <summary>
        /// 仕訳摘要ID
        /// 数値
        /// 4バイト整数
        /// 作成・修正時は無視されます
        /// </summary>
        public int RemId { get; set; }
        /// <summary>
        /// 数字１
        /// 文字列
        /// 半角英数カナ 6文字
        /// </summary>
        public int Number1 { get; set; }
        /// <summary>
        /// 数字２
        /// 文字列
        /// 半角英数カナ 23文字
        /// </summary>
        public int Number2 { get; set; }
        /// <summary>
        /// フセンID
        /// 数値
        /// 4バイト整数
        /// </summary>
        public int LabelId { get; set; }
        /// <summary>
        /// フセン文字列
        /// 文字列
        /// 全角15文字/半角30文字
        /// </summary>
        public string LabelString { get; set; }
        /// <summary>
        /// 仕訳摘要文字列
        /// 文字列
        /// 全角128文字/半角256文字
        /// </summary>
        public string Summary { get; set; }
    }

    public class InputSlipDataList
    {
        public List<BEInputSlipData> BEInputSlipData { get; set; }
    }
}
