using System;
using System.Collections.Generic;
using System.Text;

namespace Rac.VOne.Web.Models.MFModels
{
    public class MetaData
    {
        public int total_count { get; set; }
        public int total_pages { get; set; }
        public int current_page { get; set; }
        public int per_page { get; set; }
    }
}
