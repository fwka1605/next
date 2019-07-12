using System;
using System.Collections.Generic;
using System.Text;

namespace Rac.VOne.Web.Models.MFModels
{
    public class PagedBillings : PagedData
    {
        public List<billing> billings { get; set; }
    }
}
