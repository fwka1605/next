using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    /// <summary>Web API 汎用設定</summary>
    [DataContract]
    public class WebApiSetting
    {
        [DataMember] public int CompanyId { get; set; }
        /// <summary>外部連携 API のタイプ
        /// 1 : 働くDB
        /// 2 : PCA Cloud
        /// 3 : Money Forward
        /// </summary>
        [DataMember] public int ApiTypeId { get; set; }
        [DataMember] public string ApiVersion { get; set; } = string.Empty;
        [DataMember] public string BaseUri { get; set; }
        [DataMember] public string AccessToken { get; set; } = string.Empty;
        [DataMember] public string RefreshToken { get; set; } = string.Empty;
        [DataMember] public string ClientId { get; set; } = string.Empty;
        [DataMember] public string ClientSecret { get; set; } = string.Empty;

        /// <summary>json 形式で保存する 具体的な Web API の設定は model 化し、serialize/deserizlise して対応</summary>
        [DataMember] public string ExtractSetting { get; set; }
        /// <summary>json 形式で保存する 具体的な Web API の設定は model 化し、serialize/deserizlise して対応</summary>
        [DataMember] public string OutputSetting { get; set; }
        [DataMember] public int      CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int      UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class WebApiSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public WebApiSetting WebApiSetting { get; set; }
    }
}
