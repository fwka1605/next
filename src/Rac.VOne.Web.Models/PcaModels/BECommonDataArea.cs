using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>データ領域</summary>
    public class BECommonDataArea
    {
        ///<summary>1.データ領域名</summary>
        public string Name { get; set; }
        ///<summary>2.データ領域バージョン</summary>
        public string DataVersion { get; set; }
        ///<summary>3.会社-コード</summary>
        public string CompanyCode { get; set; }
        ///<summary>4.会社-名称</summary>
        public string CompanyName { get; set; }
        ///<summary>5.会社-フリガナ</summary>
        public string CompanyKane { get; set; }
        ///<summary>6.会社-処理時間</summary>
        public int CompanyTerm { get; set; }
        ///<summary>7.会社-処理時間開始日</summary>
        public int CompanyTermStart { get; set; }
        ///<summary>8.会社-処理時間終了日</summary>
        public int CompanyTermEnd { get; set; }
        ///<summary>9.会社-任意の種類</summary>
        public int CompanyType { get; set; }
        ///<summary>10.会社-使用する暦</summary>
        public string CompanyKoyomi { get; set; }
        ///<summary>11.会社-任意の数値1</summary>
        public int CompanyInt1 { get; set; }
        ///<summary>12.会社-任意の数値2</summary>
        public int CompanyInt2 { get; set; }
        ///<summary>13.会社-任意の数値3</summary>
        public int CompanyInt3 { get; set; }
        ///<summary>14.会社-任意の数値4</summary>
        public int CompanyInt4 { get; set; }
        ///<summary>15.会社-任意の数値5</summary>
        public int CompanyInt5 { get; set; }
        ///<summary>16.会社-任意の文字列1</summary>
        public string CompanyString1 { get; set; }
        ///<summary>17.会社-任意の文字列2</summary>
        public string CompanyString2 { get; set; }
        ///<summary>18.会社-任意の文字列3</summary>
        public string CompanyString3 { get; set; }
        ///<summary>19.会社-任意の文字列4</summary>
        public string CompanyString4 { get; set; }
        ///<summary>20.会社-任意の文字列5</summary>
        public string CompanyString5 { get; set; }
        ///<summary>21.会社-説明</summary>
        public string CompanyDescription { get; set; }
        ///<summary>22.会社-識別ID</summary>
        public string CompanyChainId { get; set; }
        ///<summary>23.領域状態の表示文字列</summary>
        public string StatusDescription { get; set; }
        ///<summary>24.更新日付時間</summary>
        public DateTime UpdateTime { get; set; }


        public override string ToString() => $"[{Name}] {CompanyCode}:{CompanyName}";

    }

    /// <summary>データ領域 Web API データ取得用</summary>
    public class ArrayOfBECommonDataArea
    {
        public List<BECommonDataArea> BECommonDataArea { get; set; }
    }

    /// <summary>データ領域 Web API データ取得用</summary>
    public class BECommonDataAreaSet
    {
        public ArrayOfBECommonDataArea ArrayOfBECommonDataArea { get; set; }
    }

}
