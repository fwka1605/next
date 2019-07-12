using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.MFModels
{
    public class department
    {
        public string id { get; set; }
        public string name { get; set; }
        public string zip { get; set; }
        public string tel { get; set; }
        public string prefecture { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string person_title { get; set; }
        public string person_name { get; set; }
        public string email { get; set; }
        public string cc_emails { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
