using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// フリーインポーター 設定の詳細情報
    /// </summary>
    [DataContract]
    public class ImporterSettingDetail
    {
        [DataMember] public int ImporterSettingId { get; set; }
        [DataMember] public int Sequence { get; set; }
        [DataMember] public int ImportDivision { get; set; }
        [DataMember] public int? FieldIndex { get; set; }
        [DataMember] public string Caption { get; set; }
        [DataMember] public int? AttributeDivision { get; set; }
        [DataMember] public int ItemPriority { get; set; }
        [DataMember] public int DoOverwrite { get; set; }
        [DataMember] public string FixedValue { get; set; }
        [DataMember] public int UpdateKey { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int IsUnique { get; set; }

        //for join ImporterSettingBase
        [DataMember] public DateTime ImporterSettingUpdateAt { get; set; }
        [DataMember] public string FieldName { get; set; }
        [DataMember] public int BaseImportDivision { get; set; }
        [DataMember] public int BaseAttributeDivision { get; set; }
        [DataMember] public string TargetColumn { get; set; }


        /// <summary>入金部門コード 入金フリーインポーター用</summary>
        private bool IsSectionFieldForReceipt
            => (int)Common.Importer.Receipt.Fields.SectionCode == Sequence;
        /// <summary>通貨コード 入金フリーインポーター用</summary>
        private bool IsCurrencyFieldForReceipt
            => (int)Common.Importer.Receipt.Fields.CurrencyCode == Sequence;

        /// <summary>入金フリーインポーターで利用可能な field かどうか</summary>
        /// <param name="app"></param>
        public bool IsUsableForReceipt(ApplicationControl app)
            => app.UseReceiptSection == 1 && IsSectionFieldForReceipt
            || app.UseForeignCurrency == 1 && IsCurrencyFieldForReceipt
            || !(IsSectionFieldForReceipt || IsCurrencyFieldForReceipt);
    }

    [DataContract]
    public class ImporterSettingDetailResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ImporterSettingDetail ImporterSettingDetail { get; set; }
    }

    [DataContract]
    public class ImporterSettingDetailsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ImporterSettingDetail> ImporterSettingDetails { get; set; }
    }
}