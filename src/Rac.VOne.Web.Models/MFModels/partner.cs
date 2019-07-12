using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.MFModels
{
    public class partner
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string name_kana { get; set; }
        public string name_suffix { get; set; }
        public string memo { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<department> departments { get; set; }
    }
}
