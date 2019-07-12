using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    public class WebApiHatarakuDBExtractSetting
    {
        /// <summary>DBスキーマID</summary>
        public string dbSchemaId { get; set; }
        /// <summary>絞込ID 任意</summary>
        public string searchId { get; set; }
        /// <summary>レコード一覧画面設定ID 任意</summary>
        public string listId { get; set; }
        /// <summary>取得件数 任意 max 200 linq Take に相当</summary>
        public string limit { get; set; }
        /// <summary>取得開始 オフセット 任意 未指定時 0 link Skip に相当</summary>
        public string offset { get; set; }
        /// <summary>請求フリーインポーター パターンコード
        /// Web API問合せ時に json に出力しないように 必ず null にすること</summary>
        public string PatternCode { get; set; }
    }
}
