using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// マスターインポート用 Generic class
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    [DataContract]
    public class MasterImportData<TModel>
        where TModel : class, new()
    {
        [DataMember] public List<TModel> InsertItems { get; set; }
        [DataMember] public List<TModel> UpdateItems { get; set; }
        [DataMember] public List<TModel> DeleteItems { get; set; }

        public MasterImportData()
        {
        }
    }
}
