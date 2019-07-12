using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.MFModels
{
    public class Office
    {
        ///<summary>名前</summary>
        public string Name { get; set; }
        ///<summary>郵便番号</summary>
        public string Zip { get; set; }
        ///<summary>都道府県</summary>
        public string Prefecture { get; set; }
        ///<summary>住所1</summary>
        public string Address1 { get; set; }
        ///<summary>住所2</summary>
        public string Address2 { get; set; }
        ///<summary>電話番号</summary>
        public string Tel { get; set; }
        ///<summary>FAX番号</summary>
        public string Fax { get; set; }

        #region Web API 設定 事務所情報表示用
        public string GetOfficeInfo() => string.Join("\r\n", new[] {
            $"    事務所名：{Name}",
            $"    郵便番号：{Zip}",
            $"    都道府県：{Prefecture}",
            $"    住所1：{Address1}",
            $"    住所2：{Address2}",
            $"    電話番号：{Tel}",
            $"    FAX番号：{Fax}",
            });
        #endregion
    }
}
