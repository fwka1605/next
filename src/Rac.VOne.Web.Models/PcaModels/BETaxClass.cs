using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>税区分マスター</summary>
    public class BETaxClass
    {
        ///<summary>1.エンティティバージョン</summary>
        public int BEVersion { get; set; }
        ///<summary>2.税区分ID</summary>
        public int Id { get; set; }
        ///<summary>3.税区分コード</summary>
        public string Code { get; set; }
        ///<summary>4.税区分名</summary>
        public string Name { get; set; }
        ///<summary>5.説明文</summary>
        public string Caption { get; set; }
        ///<summary>6.税率</summary>
        public decimal Rate { get; set; }
        ///<summary>7.使用開始日</summary>
        public IntDate StartDate { get; set; }
        ///<summary>8.使用終了日</summary>
        public IntDate EndDate { get; set; }
        ///<summary>9.改正後税区分ID</summary>
        public int RevisedId { get; set; }
        ///<summary>10.表示区分</summary>
        public string DisplayMode { get; set; }
        ///<summary>11.参照ウィンドウ表示グループ</summary>
        public string RefGroupType { get; set; }
        ///<summary>12.消費税管理別表示区分</summary>
        public string ItemizedDisplayMode { get; set; }
        ///<summary>13.改正前税区分ID</summary>
        public int PriorId { get; set; }
    }

    /// <summary>税区分マスター Web API データ取得用</summary>
    public class ArrayOfBETaxClass
    {
        public List<BETaxClass> BETaxClass { get; set; }
    }

    /// <summary>税区分マスター Web API データ取得用</summary>
    public class BETaxClassSet
    {
        public ArrayOfBETaxClass ArrayOfBETaxClass { get; set; }
    }
}
