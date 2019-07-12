﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ImportResultSectionWithLoginUser : ImportResult
    {
        [DataMember] public List<SectionWithLoginUser> SectionWithLoginUser { get; set; } = new List<SectionWithLoginUser>();
    }
}
