using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    public class WebApiHatarakuDBOutputSetting
    {
        /// <summary>データベースID</summary>
        public string dbSchemaId { get; set; }
        /// <summary>インポート設定ID</summary>
        public string importId { get; set; }
    }
}
