using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.PcaModels
{
    public class BusinessEntityResultOfBEInputSlip
    {
        /// <summary>
        /// エラーコード
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// エラーレベル None 固定
        /// </summary>
        public string ErrorLevel { get; set; }
    }
}
