using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ApplicationControl : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int DepartmentCodeLength { get; set; }
        [DataMember] public int DepartmentCodeType { get; set; }
        [DataMember] public int SectionCodeLength { get; set; }
        [DataMember] public int SectionCodeType { get; set; }
        [DataMember] public int AccountTitleCodeLength { get; set; }
        [DataMember] public int AccountTitleCodeType { get; set; }
        [DataMember] public int CustomerCodeLength { get; set; }
        [DataMember] public int CustomerCodeType { get; set; }
        [DataMember] public int LoginUserCodeLength { get; set; }
        [DataMember] public int LoginUserCodeType { get; set; }
        [DataMember] public int StaffCodeLength { get; set; }
        [DataMember] public int StaffCodeType { get; set; }
        /// <summary>請求部門利用</summary>
        [Obsolete][DataMember] public int UseDepartment { get; set; } = 1;
        /// <summary>入金予定入力利用 </summary>
        [DataMember] public int UseScheduledPayment { get; set; }
        /// <summary>入金部門利用</summary>
        [DataMember] public int UseReceiptSection { get; set; }
        /// <summary>承認機能利用</summary>
        [DataMember] public int UseAuthorization { get; set; }
        /// <summary>長期前受管理 利用</summary>
        [DataMember] public int UseLongTermAdvanceReceived { get; set; }
        /// <summary>長期前受管理 契約番号の事前登録を行う</summary>
        [DataMember] public int RegisterContractInAdvance { get; set; }
        /// <summary>期日現金利用 </summary>
        [DataMember] public int UseCashOnDueDates { get; set; }
        /// <summary>ファクタリング対応 </summary>
        [DataMember] public int UseFactoring { get; set; }
        /// <summary>入金予定入力 - 予定額を消込対象額に使用</summary>
        [DataMember] public int UseDeclaredAmount { get; set; }
        /// <summary>歩引利用</summary>
        [DataMember] public int UseDiscount { get; set; }
        /// <summary>外貨利用</summary>
        [DataMember] public int UseForeignCurrency { get; set; }
        /// <summary>請求絞込利用</summary>
        [DataMember] public int UseBillingFilter { get; set; }
        /// <summary>配信機能利用</summary>
        [DataMember] public int UseDistribution { get; set; }
        /// <summary>操作ログ機能利用</summary>
        [DataMember] public int UseOperationLogging { get; set; }
        /// <summary>スタンダード/エントリー モデル</summary>
        [DataMember] public int ApplicationEdition { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public string GeneralSettingValue { get; set; }
        /// <summary>フォルダ選択の制限</summary>
        [DataMember] public int LimitAccessFolder { get; set; }
        /// <summary>ルートフォルダのパス</summary>
        [DataMember] public string RootPath { get; set; }
        /// <summary>請求書発行機能利用</summary>
        [DataMember] public int UsePublishInvoice { get; set; }
        /// <summary>働くDB WebAPI 利用</summary>
        [DataMember] public int UseHatarakuDBWebApi { get; set; }
        /// <summary>PCA会計DX WebAPI利用</summary>
        [DataMember] public int UsePCADXWebApi { get; set; }
        /// <summary>督促管理利用</summary>
        [DataMember] public int UseReminder { get; set; }
        /// <summary>口座振替利用</summary>
        [DataMember] public int UseAccountTransfer { get; set; }
        /// <summary>MFクラウド請求書 WebAPI利用</summary>
        [DataMember] public int UseMFWebApi { get; set; }
        /// <summary>締め処理</summary>
        [DataMember] public int UseClosing { get; set; }
        /// <summary>MF明細連携 利用</summary>
        [DataMember] public int UseMfAggregation { get; set; }
    }

    [DataContract]
    public class ApplicationControlResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ApplicationControl ApplicationControl { get; set; }
    }
}
