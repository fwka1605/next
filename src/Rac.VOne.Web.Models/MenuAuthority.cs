using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>メニュー権限マスター
    /// メニュー追加時の注意点は remarks に記載 F12で内容を確認しておくこと
    /// </summary>
    /// <remarks>
    /// <see cref="ApplicationControl"/>に オプション項目を追加した場合、
    /// 当Modelの private property で制御を行う
    /// 標準 で表示するメニュー → 何も変更を行わない
    /// 標準には存在せず、<see cref="ApplicationControl"/>のオプション有効時のみ許可
    ///  → private な bool 型 property を用意 <see cref="IsStandard"/>
    ///     および<see cref="IsAvailable(ApplicationControl)"/>メソッドの記述を修正
    /// 外貨利用時のみ 非表示 → <see cref="IsForeignCurrencyExcluded"/>とその参照を確認してください
    /// </remarks>
    [DataContract] public class MenuAuthority : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string MenuId { get; set; }
        [DataMember] public int AuthorityLevel { get; set; }
        [DataMember] public int Available { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        #region menu table items

        [DataMember] public string MenuName { get; set; }
        [DataMember] public string MenuCategory { get; set; }
        [DataMember] public int Sequence { get; set; }

        #endregion



        #region options / ApplicationControl と連動
        /// <summary>標準メニュー</summary>
        /// <remarks>オプションで制限する項目を追加した場合は、
        /// public readonly のプロパティを追加するとともに、標準メニュー外であることを
        /// 下記の記述を修正して定義する必要あり
        /// 標準メニュー追加の場合は、モデルの修正を行う必要はない
        /// </remarks>
        private bool IsStandard
            => !(IsReceiptSection
              || IsScheduledPaymentOnly
              || IsAuthorizationOnly
              || IsForeignCurrencyOnly
              || IsDistribution
              || IsPublishInvoice
              || IsHatarakuDBWebApi
              || IsPcaDXWebApi
              || IsReminder
              || IsAccountTransfer
              || IsMFWebApi
              || IsClosing
            );

        /// <summary>入金部門管理利用時のみ利用可能</summary>
        private bool IsReceiptSection
            => MenuId == "PB1101" /* 入金部門マスター */
            || MenuId == "PB1201" /* 入金・請求部門対応マスター */
            || MenuId == "PB1301" /* 入金部門・担当者対応マスター */
            || MenuId == "PD0801" /* 入金部門振替処理 */
            ;

        /// <summary>外貨利用時に、利用不可</summary>
        private bool IsForeignCurrencyExcluded
            => MenuId == "PC0401" /* 請求書発行 */
            || MenuId == "PC1401" /* 請求書再発行 */
            || MenuId == "PC1501" /* 請求書設定 */
            || MenuId == "PC1601" /* 定期請求パターンマスター */
            || MenuId == "PC1701" /* 定期請求データ登録 */
            || MenuId == "PF0201" /* 債権総額管理表 */
            || MenuId == "PF0601" /* 回収予定表 */
            ;

        /// <summary>入金予定利用時のみ利用可能</summary>
        private bool IsScheduledPaymentOnly
            => MenuId == "PC0901" /* 入金予定入力 */
            || MenuId == "PC1001" /* 入金予定フリーインポーター */
            ;

        /// <summary>承認機能利用時のみ利用可能</summary>
        private bool IsAuthorizationOnly
            => MenuId == "PE0701" /* 消込データ承認 */
            ;

        /// <summary>外貨利用時のみ利用可能</summary>
        private bool IsForeignCurrencyOnly
            => MenuId == "PB2101" /* 通貨マスター */
            ;

        /// <summary>配信機能利用時のみ、利用可能</summary>
        private bool IsDistribution
            => MenuId == "PG0101" /* 回収通知メール配信 */
            || MenuId == "PG0201" /* 回収遅延通知メール配信 */
            || MenuId == "PG0301" /* メール設定 */
            || MenuId == "PG0401" /* WebViewer公開処理 */
            ;

        /// <summary>請求書発行機能利用時のみ、利用可能</summary>
        private bool IsPublishInvoice
            => MenuId == "PC0401" /* 請求書発行 */
            || MenuId == "PC1401" /* 請求書再発行 */
            || MenuId == "PC1501" /* 請求書設定 */
            ;

        /// <summary>働くDB Web API 連携利用時のみ、利用可能</summary>
        private bool IsHatarakuDBWebApi
            => MenuId == "PC1301" /* 働くDB 請求データ抽出 */
            || MenuId == "PE0801" /* 働くDB 消込結果連携 */
            || MenuId == "PH1101" /* 働くDB WebAPI 連携設定 */
            ;

        /// <summary>PCA会計DX Web API 連携利用時のみ、利用可能</summary>
        private bool IsPcaDXWebApi
            => MenuId == "PE0901" /* PCA会計DX  消込結果連携 */
            || MenuId == "PH1201" /* PCA会計DX WebAPI 連携設定 */
            ;

        /// <summary>PCA会計DX Web API 連携利用時のみ、利用可能</summary>
        private bool IsMFWebApi
            => MenuId == "PH1401" /* MFクラウド請求書 Web API 連携設定 */
            || MenuId == "PC1801" /* MFクラウド請求書 請求データ抽出 */
            || MenuId == "PE1001" /* MFクラウド会計 消込結果連携 */
            ;

        /// <summary>督促管理利用時のみ、利用可能</summary>
        private bool IsReminder
            => MenuId == "PI0101" /*督促データ確定*/
            || MenuId == "PI0201" /*督促データ管理*/
            || MenuId == "PI0301" /*督促管理帳票*/
            || MenuId == "PI0401" /*督促設定*/
            ;

        /// <summary>口座振替利用時のみ、利用可能</summary>
        private bool IsAccountTransfer
            => MenuId == "PB1901" /*決済代行会社マスタ*/
            || MenuId == "PC0701" /*口座振替依頼データ作成*/
            || MenuId == "PC0801" /*口座振替結果データ取込*/
            ;

        /// <summary>締め処理利用時のみ、利用可能</summary>
        private bool IsClosing
            => MenuId == "PH1501" /*締め処理*/
            ;

        /// <summary><see cref="ApplicationControl"/>によって、メニューが利用可能か判別するメソッド</summary>
        public bool IsAvailable(ApplicationControl app)
            => (app.UseForeignCurrency      == 0
             || app.UseForeignCurrency      == 1 && !IsForeignCurrencyExcluded)
            && (IsStandard                  
             || app.UseReceiptSection       == 1 && IsReceiptSection
             || app.UseForeignCurrency      == 1 && IsForeignCurrencyOnly
             || app.UseScheduledPayment     == 1 && IsScheduledPaymentOnly
             || app.UseAuthorization        == 1 && IsAuthorizationOnly
             || app.UseDistribution         == 1 && IsDistribution
             || app.UsePublishInvoice       == 1 && IsPublishInvoice
             || app.UseHatarakuDBWebApi     == 1 && IsHatarakuDBWebApi
             || app.UsePCADXWebApi          == 1 && IsPcaDXWebApi
             || app.UseReminder             == 1 && IsReminder
             || app.UseAccountTransfer      == 1 && IsAccountTransfer
             || app.UseMFWebApi             == 1 && IsMFWebApi
             || app.UseClosing              == 1 && IsClosing
            );

        #endregion
    }

    [DataContract]
    public class MenuAuthoritiesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MenuAuthority> MenuAuthorities { get; set; }
    }
}
