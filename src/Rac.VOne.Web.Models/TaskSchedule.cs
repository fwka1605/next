using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class TaskSchedule
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        /// <summary>
        /// インポート種別
        /// 0 : 得意先マスター
        /// 1 : 債権代表者マスター
        /// 2 : 請求フリーインポーター
        /// 3 : EBデータ取込
        /// 4 : 入金フリーインポーター
        /// 5 : 入金予定フリーインポーター
        /// </summary>
        [DataMember] public int ImportType { get; set; }
        /// <summary>
        /// インポート種別詳細パターン
        /// (マスター登録方法
        /// /EBファイルフォーマット
        /// /フリーインポーターパターンID)
        /// </summary>
        [DataMember] public int ImportSubType { get; set; }
        [DataMember] public int Duration { get; set; }
        [DataMember] public DateTime StartDate { get; set; }
        [DataMember] public int Interval { get; set; }
        [DataMember] public byte[] WeekDay { get; set; }
        /// <summary> 取込ファイル指定フォルダ </summary>
        [DataMember] public string ImportDirectory { get; set; }
        /// <summary> 取込成功フォルダ </summary>
        [DataMember] public string SuccessDirectory { get; set; }
        /// <summary> 取込失敗フォルダ
        /// エラーログ出力先 候補</summary>
        [DataMember] public string FailedDirectory { get; set; }
        /// <summary>
        /// エラーログ出力場所
        /// 0 : 失敗時移動フォルダと同一
        /// 1 : ユーザーフォルダー
        /// </summary>
        [DataMember] public int LogDestination { get; set; }
        /// <summary>
        /// 入金予定用 0 : 未消込, 1 : 一部消込も対象
        /// </summary>
        [DataMember] public int TargetBillingAssignment { get; set; }
        /// <summary>
        /// 入金予定用 金額更新方法 0 : 更新, 1 : 加算
        /// </summary>
        [DataMember] public int BillingAmount { get; set; }
        /// <summary>
        /// 入金予定用  0 : なにもしない, 1 : すべて更新
        /// </summary>
        [DataMember] public int UpdateSameCustomer { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public string CreateUserName { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public string UpdateUserName { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        /// <summary>
        /// 得意先マスター用 取込方法  0 : 上書, 1 : 追加, 2 : 更新
        /// </summary>
        [DataMember] public int ImportMode { get; set; }
    }

    [DataContract]
    public class TaskScheduleResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public TaskSchedule TaskSchedule { get; set; }
    }

    [DataContract]
    public class TaskSchedulesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<TaskSchedule> TaskSchedules { get; set; }
    }

}
