using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// MastereImportSetting マスター取込設定
    /// フリーインポーターの設定とは異なる
    /// マスター取込の取込方法(上書, 追加, 更新)
    /// エラーログ出力 有無 出力先の設定
    /// 取込時に確認ダイアログを表示するか否か
    /// </summary>
    [DataContract]
    public class ImportSetting
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int ImportFileType { get; set; }
        [DataMember] public string ImportFileName { get; set; }
        /// <summary>
        ///     取込方法
        ///     0 : 上書, 1 : 追加, 2 : 更新
        /// </summary>
        [DataMember] public int ImportMode { get; set; }
        [DataMember] public int ExportErrorLog { get; set; }
        /// <summary>
        /// 0 : ユーザーフォルダー
        /// 1 : 取込ファイルと同一フォルダー
        /// </summary>
        [DataMember] public int ErrorLogDestination { get; set; }
        [DataMember] public int Confirm { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class ImportSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ImportSetting ImportSetting { get; set; }
    }


    [DataContract]
    public class ImportSettingResults : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ImportSetting> ImportSettings { get; set; }
    }
}


