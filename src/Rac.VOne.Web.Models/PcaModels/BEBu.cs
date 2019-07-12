using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    /// <summary>部門マスター</summary>
    public class BEBu
    {
        ///<summary>1.部門ID</summary>
        public int Id { get; set; }
        ///<summary>2.部門コード</summary>
        public string Code { get; set; }
        ///<summary>3.部門名</summary>
        public string Name { get; set; }
        ///<summary>4.ｶﾅ索引</summary>
        public string Kana { get; set; }
        ///<summary>5.削除可能フラグ</summary>
        public string Deletable { get; set; }
        ///<summary>6.予備1</summary>
        public int Reserve1 { get; set; }
        ///<summary>7.予備2</summary>
        public int Reserve2 { get; set; }
        ///<summary>8.予備3</summary>
        public int Reserve3 { get; set; }
        ///<summary>9.更新日付時間</summary>
        public DateTime UpdateTime { get; set; }
        ///<summary>10.部門権限ロック</summary>
        public string BuLockType { get; set; }
    }

    /// <summary>部門マスター Web API データ取得用</summary>
    public class ArrayOfBEBu
    {
        public List<BEBu> BEBu { get; set; }
    }

    /// <summary>部門マスター Web API データ取得用</summary>
    public class BEBuSet
    {
        public ArrayOfBEBu ArrayOfBEBu { get; set; }
    }
}
