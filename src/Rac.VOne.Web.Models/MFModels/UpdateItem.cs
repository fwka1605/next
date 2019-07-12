using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.MFModels
{
    /// <summary>入金ステータス変更用Model</summary>
    public class UpdateItem
    {
        public billing_status billing_status { get; set; }
        public UpdateItem(bool isMatched)
        {
            billing_status = new billing_status {
                payment = isMatched ? "2" : "0",
            };
        }
    }

}
