using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class WorkingReport
    {
        public WorkingReport(){}
        public WorkingReport(int? lineNo,int fieldNo, string fieldName, string message)
        {
            LineNo = lineNo;
            FieldNo = fieldNo;
            FieldName = fieldName;
            Message = message;
        }
        public int? LineNo { get; set; }
        public int FieldNo { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
    }
}
