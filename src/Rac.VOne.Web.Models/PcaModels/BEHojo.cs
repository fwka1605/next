using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>補助科目(得意先)</summary>
    public class BEHojo
    {
        ///<summary>1.エンティティバージョン</summary>
        public int BEVersion { get; set; }
        ///<summary>2.補助ID</summary>
        public int Id { get; set; }
        ///<summary>3.補助科目コード</summary>
        public string Code { get; set; }
        ///<summary>4.科目ID</summary>
        public int KmkId { get; set; }
        ///<summary>5.補助結合ID</summary>
        public int HojoUnionId { get; set; }
        ///<summary>6.ｶﾅ索引</summary>
        public string Kana { get; set; }
        ///<summary>7.補助科目名</summary>
        public string Name { get; set; }
        ///<summary>8.補助科目正式名ﾌﾘｶﾞﾅ</summary>
        public string FormalKana { get; set; }
        ///<summary>9.補助科目正式名</summary>
        public string FormalName { get; set; }
        ///<summary>10.借方税区分ID</summary>
        public int DrTaxClassId { get; set; }
        ///<summary>11.貸方税区分ID</summary>
        public int CrTaxClassId { get; set; }
        ///<summary>12.消費税自動計算</summary>
        public string TaxCalcMode { get; set; }
        ///<summary>13.消費税端数処理</summary>
        public string TaxRoundMode { get; set; }
        ///<summary>14.振込先使用フラグ</summary>
        public string BankTransferMode { get; set; }
        ///<summary>15.郵便番号</summary>
        public string ZipCode { get; set; }
        ///<summary>16.住所上段</summary>
        public string Address1 { get; set; }
        ///<summary>17.住所下段</summary>
        public string Address2 { get; set; }
        ///<summary>18.TEL</summary>
        public string Tel { get; set; }
        ///<summary>19.FAX</summary>
        public string Fax { get; set; }
        ///<summary>20.締日</summary>
        public int CloseDay { get; set; }
        ///<summary>21.支払日</summary>
        public int PayDay { get; set; }
        ///<summary>22.法人番号</summary>
        public string CorporateMyNumber { get; set; }
        ///<summary>23.予備1</summary>
        public int Reserve1 { get; set; }
        ///<summary>24.予備2</summary>
        public int Reserve2 { get; set; }
        ///<summary>25.予備3</summary>
        public int Reserve3 { get; set; }
        ///<summary>26.更新日付時間</summary>
        public DateTime UpdateTime { get; set; }
        ///<summary>27.科目権限ロック</summary>
        public string KmkLockType { get; set; }

    }

    /// <summary>補助科目 Web API データ取得用</summary>
    public class ArrayOfBEHojo
    {
        public List<BEHojo> BEHojo { get; set; }
    }

    /// <summary>補助科目 Web API データ取得用</summary>
    public class BEHojoSet
    {
        public ArrayOfBEHojo ArrayOfBEHojo { get; set; }
    }
}
