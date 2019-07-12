using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>勘定科目</summary>
    public class BEKmk
    {
        ///<summary>1.科目ID</summary>
        public int Id { get; set; }
        ///<summary>2.科目属性連結番号
        /// 現預金範囲  :11110000 ～ 11119999
        /// B/S科目範囲 :10000000 ～ 19999999
        /// P/L科目範囲 :20000000 ～ 39999999
        /// 製造原価範囲:23000000 ～ 26999999
        /// </summary>
        public int KmkAttributeLinkNumber { get; set; }
        ///<summary>3.勘定科目コード</summary>
        public string Code { get; set; }
        ///<summary>4.削除可能フラグ</summary>
        public string Deletable { get; set; }
        ///<summary>5.勘定科目名</summary>
        public string Name { get; set; }
        ///<summary>6.勘定科目正式名</summary>
        public string FormalName { get; set; }
        ///<summary>7.ｶﾅ索引</summary>
        public string Kana { get; set; }
        ///<summary>8.貸借区分</summary>
        public string DrCrMode { get; set; }
        ///<summary>9.借方税区分ID</summary>
        public int DrTaxClassId { get; set; }
        ///<summary>10.貸方税区分ID</summary>
        public int CrTaxClassId { get; set; }
        ///<summary>11.関連科目ID</summary>
        public int ConnectionKmkId { get; set; }
        ///<summary>12.補助件数</summary>
        public int HojoCount { get; set; }
        ///<summary>13.表示区分</summary>
        public string DisplayMode { get; set; }
        ///<summary>14.消費税自動計算</summary>
        public string TaxCalcMode { get; set; }
        ///<summary>15.消費税端数処理</summary>
        public string TaxRoundMode { get; set; }
        ///<summary>16.予備1</summary>
        public int Reserve1 { get; set; }
        ///<summary>17.予備2</summary>
        public int Reserve2 { get; set; }
        ///<summary>18.予備3</summary>
        public int Reserve3 { get; set; }
        ///<summary>19.更新日付時間</summary>
        public DateTime UpdateTime { get; set; }
        ///<summary>20.科目権限ロック</summary>
        public string KmkLockType { get; set; }

        /// <summary>補助科目 要否</summary>
        public bool RequireHojo => HojoCount > 0;
        /// <summary>損益計算(PL)科目かどうか</summary>
        public bool IsPLAccount =>
            20000000 <= KmkAttributeLinkNumber
                     && KmkAttributeLinkNumber <= 39999999;
    }

    /// <summary>勘定科目 Web API データ取得用</summary>
    public class ArrayOfBEKmk
    {
        public List<BEKmk> BEKmk { get; set; }
    }

    /// <summary>勘定科目 Web API データ取得用</summary>
    public class BEKmkSet
    {
        public ArrayOfBEKmk ArrayOfBEKmk { get; set; }
    }
}
