using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Rac.VOne.Web.Models;
using System.Windows.Forms;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.SectionReportModel;
using Rac.VOne.Client.Reports.Settings;
using System.IO;
using GrapeCity.ActiveReports.Document.Section;
using static Rac.VOne.Common.Constants;
using System.Runtime.Serialization.Formatters.Binary;


namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingInvoiceReport の概要の説明です。
    /// </summary>
    public partial class BillingInvoiceReport : GrapeCity.ActiveReports.SectionReport
    {
        #region メンバー & コンストラクター
        public BillingInvoiceReportSetting Setting { get; set; }
        public List<CompanyLogo> CompanyLogos { get; set; }
        public Company Company { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        private bool isFirstPrint { get; set; }
        private bool isCopy { get; set; }
        private const int MaxRowCountFirstPage = 10;
        private const int MaxRowCountOtherPage = 18;

        string[] dueDateComment = new string[100000];
        string[] receiveAccount1 = new string[100000];
        string[] receiveAccount3 = new string[100000];
        string[] receiveAccount2 = new string[100000];
        string[] bankAccountName = new string[100000];
        string[] transferFeeComment = new string[100000];
        int i = 0;
        int j = 0;

        public BillingInvoiceReport(bool isFirstPrint, bool isCopy = false)
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
            this.isFirstPrint = isFirstPrint;
            this.isCopy = isCopy;
        }
        #endregion

        #region 初期化
        public void Initialize(List<ReportSetting> listSetting)
        {
            Setting = new BillingInvoiceReportSetting(listSetting);
            InitializeControls();
            InitializePictures();
            HideControls();
            if (isFirstPrint)
            {
                Name = "請求書" + DateTime.Today.ToString("yyyyMMdd");
            }
            else
            {
                Name = "再発行請求書" + DateTime.Today.ToString("yyyyMMdd");
            }
        }

        #region コントロールの初期化
        private void InitializeControls()
        {
            InitializeGroupHeaderAndFooter();
            InitializeTextBox();
            ReSizeTextBoxNote();
            AllowShrinkToFit();
        }
        private void InitializeTextBox()
        {
            //請求書番号
            lblInvoiceCode.DataField = nameof(BillingInvoiceDetailForPrint.InvoiceCode);
            //右上日付
            txtDate.OutputFormat = "yyyy年M月d日";
            if (Setting.ReportInvoiceDate == ReportInvoiceDate.BilledAt)
            {
                txtDate.DataField = nameof(BillingInvoiceDetailForPrint.BilledAt);
            }
            else if (Setting.ReportInvoiceDate == ReportInvoiceDate.ClosingAt)
            {
                txtDate.DataField = nameof(BillingInvoiceDetailForPrint.ClosingAt);
            }
            else
            {
                var now = DateTime.Now;
                txtDate.Value = now;
                InvoiceDate = now;
            }
            //自社情報の設定
            lblCompanyName.Text = Company.Name;
            lblCompanyPostCode.Text = Company.PostalCode;
            lblCompanyAddress1.Text = Company.Address1;
            lblCompanyAddress2.Text = Company.Address2;
            lblTel.DataField = nameof(BillingInvoiceDetailForPrint.Tel);
            lblFax.DataField = nameof(BillingInvoiceDetailForPrint.Fax);
            lblStaffName.DataField = nameof(BillingInvoiceDetailForPrint.StaffName);
            txtDisplayStaff.DataField = nameof(BillingInvoiceDetailForPrint.DisplayStaff);
            txtDisplayStaff.Visible = false;
            //挨拶文
            txtGreeting.DataField = nameof(BillingInvoiceDetailForPrint.Greeting);
            //宛先
            lblCustomerPostCode.DataField = nameof(BillingInvoiceDetailForPrint.CustomerPostalCode);
            lblCustomerAddress1.DataField = nameof(BillingInvoiceDetailForPrint.CustomerAddress1);
            lblCustomerAddress2.DataField = nameof(BillingInvoiceDetailForPrint.CustomerAddress2);
            lblCustomerName.DataField = nameof(BillingInvoiceDetailForPrint.CustomerName);
            lblCustomerDepartmentName.DataField = nameof(BillingInvoiceDetailForPrint.DestinationDepartmentName);
            lblCustomerAddressee.DataField = nameof(BillingInvoiceDetailForPrint.DestinationAddressee);
            //タイトル
            if (isCopy)
            {
                lblTitle.DataField = "= " + nameof(BillingInvoiceDetailForPrint.Title) + " + \"(控)\"";
            }
            else
            {
                lblTitle.DataField = nameof(BillingInvoiceDetailForPrint.Title);
            }
            //御請求金額
            txtBillingAmountSumHeader.SummaryFunc = SummaryFunc.Sum;
            txtBillingAmountSumHeader.SummaryGroup = ghInvoice.Name;
            txtBillingAmountSumHeader.SummaryRunning = SummaryRunning.None;
            txtBillingAmountSumHeader.SummaryType = SummaryType.SubTotal;
            txtBillingAmountSumHeader.OutputFormat = "#,##0";
            //期日コメント
            txtDueDateComment_gh.DataField = nameof(BillingInvoiceDetailForPrint.DueDateComment);
            //手数料負担コメント
            txtTransferFeeComment_gh.DataField = nameof(BillingInvoiceDetailForPrint.TransferFeeComment);
            //振込銀行
            if (Setting.DisplayVirtualAccount == ReportDoOrNot.Do)
            {
                lblReceiveAccount1_gh.DataField = nameof(BillingInvoiceDetailForPrint.ExclusiveAccount);
            }
            else
            {
                lblReceiveAccount1_gh.DataField = nameof(BillingInvoiceDetailForPrint.ReceiveAccount1);
                lblReceiveAccount2_gh.DataField = nameof(BillingInvoiceDetailForPrint.ReceiveAccount2);
                lblReceiveAccount3_gh.DataField = nameof(BillingInvoiceDetailForPrint.ReceiveAccount3);
                lblBankAccountName_gh.DataField = nameof(BillingInvoiceDetailForPrint.BankAccountName);
            }
            ////明細
            txtSalesAt.DataField = nameof(BillingInvoiceDetailForPrint.SalesAt);
            txtSalesAt.OutputFormat = "yyyy/MM/dd";
            txtNote1.DataField = nameof(BillingInvoiceDetailForPrint.Note1);
            txtNote2.DataField = nameof(BillingInvoiceDetailForPrint.Note2);
            txtAmount.OutputFormat = "#,###";
            txtDetailCount.DataField = nameof(BillingInvoiceDetailForPrint.DetailCount);
            txtDetailCount.Visible = false;
            //金額の設定
            if (Setting.ReportInvoiceAmount == ReportInvoiceAmount.BillingAmount)
            {
                //ヘッダー
                txtBillingAmountSumHeader.DataField = nameof(BillingInvoiceDetailForPrint.BillingAmount);
                //フッター
                txtAmountSum.DataField = nameof(BillingInvoiceDetailForPrint.TaxExcludedPrice);
                txtTaxAmountSum.DataField = nameof(BillingInvoiceDetailForPrint.TaxAmount);
                txtTaxAmountSum.OutputFormat = "#,##0";
                txtTaxAmountSum.SummaryFunc = SummaryFunc.Sum;
                txtTaxAmountSum.SummaryGroup = ghInvoice.Name;
                txtTaxAmountSum.SummaryRunning = SummaryRunning.Group;
                txtTaxAmountSum.SummaryType = SummaryType.SubTotal;
                txtBillingAmountSumFooter.DataField = nameof(BillingInvoiceDetailForPrint.BillingAmount);
                //明細
                txtAmount.DataField = nameof(BillingInvoiceDetailForPrint.TaxExcludedPrice);
                txtQuantity.DataField = nameof(BillingInvoiceDetailForPrint.Quantity);
                txtQuantity.OutputFormat = "#,##0";
                txtUnitSymbol.DataField = nameof(BillingInvoiceDetailForPrint.UnitSymbol);
                txtUnitPrice.DataField = nameof(BillingInvoiceDetailForPrint.UnitPrice);
                txtUnitPrice.OutputFormat = "#,##0";
                txtTaxClassName.DataField = nameof(BillingInvoiceDetailForPrint.TaxClassName);
            }
            else
            {
                //ヘッダー
                txtBillingAmountSumHeader.DataField = nameof(BillingInvoiceDetailForPrint.RemainAmount);
                //フッター
                txtAmountSum.DataField = nameof(BillingInvoiceDetailForPrint.RemainAmount);
                txtTaxAmountSum.Text = "-";
                txtTaxAmountSum.Alignment = TextAlignment.Center;
                txtBillingAmountSumFooter.DataField = nameof(BillingInvoiceDetailForPrint.RemainAmount);
                //明細
                txtAmount.DataField = nameof(BillingInvoiceDetailForPrint.RemainAmount);
                txtQuantity.Text = "-";
                txtQuantity.Alignment = TextAlignment.Center;
                txtUnitSymbol.Text = "-";
                txtUnitSymbol.Alignment = TextAlignment.Center;
                txtUnitPrice.Text = "-";
                txtUnitPrice.Alignment = TextAlignment.Center;
                txtTaxClassName.Text = string.Empty;
            }
            //合計金額
            txtAmountSum.SummaryFunc = SummaryFunc.Sum;
            txtAmountSum.SummaryGroup = ghInvoice.Name;
            txtAmountSum.SummaryRunning = SummaryRunning.Group;
            txtAmountSum.SummaryType = SummaryType.SubTotal;
            txtAmountSum.OutputFormat = "#,##0";
            //請求金額
            txtBillingAmountSumFooter.SummaryFunc = SummaryFunc.Sum;
            txtBillingAmountSumFooter.SummaryGroup = ghInvoice.Name;
            txtBillingAmountSumFooter.SummaryRunning = SummaryRunning.Group;
            txtBillingAmountSumFooter.SummaryType = SummaryType.SubTotal;
            txtBillingAmountSumFooter.OutputFormat = "#,##0";
        }
        private void InitializeGroupHeaderAndFooter()
        {
            ghInvoiceOnPage1.RepeatStyle = RepeatStyle.OnPage;
            ghInvoiceOnPage1.DataField = nameof(BillingInvoiceDetailForPrint.BillingInputId);
            ghInvoiceOnPage1.NewPage = NewPage.Before;
            ghInvoiceOnPage1.CanShrink = true;
            ghInvoiceOnPage1.RepeatStyle = RepeatStyle.OnPageIncludeNoDetail;
            reportInfo2.Visible = false;
            reportInfo3.Visible = false;
            txtDueDateComment_gh.Visible = false;
            lblReceiveAccount1_gh.Visible = false;
            lblReceiveAccount2_gh.Visible = false;
            lblReceiveAccount3_gh.Visible = false;
            lblBankAccountName_gh.Visible = false;
            txtTransferFeeComment_gh.Visible = false;

            ghInvoice.DataField = nameof(BillingInvoiceDetailForPrint.BillingInputId);
            ghInvoice.CanShrink = true;

            ghInvoiceOnPage2.RepeatStyle = RepeatStyle.OnPage;
            ghInvoiceOnPage2.DataField = nameof(BillingInvoiceDetailForPrint.BillingInputId);
            ghInvoiceOnPage2.CanShrink = true;

            detail.CanShrink = true;

            gfInvoice.CanShrink = true;
            gfInvoice.KeepTogether = true;

            gfInvoiceOnPage1.CanShrink = true;
            pageFooter1.CanShrink = true;
        }
        private void ReSizeTextBoxNote()
        {
            //幅の決定
            float width = 0;
            float? pointX = null;

            if (Setting.DisplaySalesAt == ReportDoOrNot.Do
                && Setting.DisplayQuantity == ReportDoOrNot.Do)
            {
                return;
            }
            //数量 非表示
            else if (Setting.DisplaySalesAt == ReportDoOrNot.Do
                && Setting.DisplayQuantity == ReportDoOrNot.NotDo)
            {
                width = line36.X1 - lblNote1.Location.X;
            }
            //取引日 非表示
            else if (Setting.DisplaySalesAt == ReportDoOrNot.NotDo
                && Setting.DisplayQuantity == ReportDoOrNot.Do)
            {
                width = line2.X1 - lblSalesAt.Location.X;
                pointX = lblSalesAt.Location.X;
            }
            //数量 取引日 非表示
            else
            {
                width = line36.X1;
                pointX = lblSalesAt.Location.X;
            }

            lblNote1.Width = width;
            txtNote1.Width = width;
            txtNote2.Width = width;
            if (pointX.HasValue)
            {
                lblNote1.Left = pointX.Value;
                txtNote1.Left = pointX.Value;
                txtNote2.Left = pointX.Value;
            }
        }
        private void AllowShrinkToFit()
        {
            lblReceiveAccount1_pf.ShrinkToFit = true;
            lblReceiveAccount2_pf.ShrinkToFit = true;
            lblReceiveAccount3_pf.ShrinkToFit = true;
            lblBankAccountName_pf.ShrinkToFit = true;

            lblReceiveAccount1_pf.MultiLine = true;
            lblReceiveAccount2_pf.MultiLine = true;
            lblReceiveAccount3_pf.MultiLine = true;
            lblBankAccountName_pf.MultiLine = true;

            lblReceiveAccount1_pf.WrapMode = WrapMode.NoWrap;
            lblReceiveAccount2_pf.WrapMode = WrapMode.NoWrap;
            lblReceiveAccount3_pf.WrapMode = WrapMode.NoWrap;
            lblBankAccountName_pf.WrapMode = WrapMode.NoWrap;
        }
        #endregion

        #region 非表示にする関数
        private void HideControls()
        {
            //承認欄
            if (Setting.DisplayApproval == ReportDoOrNot.NotDo) HideApproval();
            //再発行
            lblRePrint.Visible = (!isFirstPrint && Setting.DisplayRePrint == ReportDoOrNot.Do);
            //数量・単位・単価
            if (Setting.DisplayQuantity == ReportDoOrNot.NotDo) HideQuantity();
            //売上日
            if (Setting.DisplaySalesAt == ReportDoOrNot.NotDo) HideSalesAt();
            //宛先
            if (Setting.DisplayAddress == ReportDoOrNot.NotDo) HideCustomerAddress();
            //振込先
            if (Setting.DisplayVirtualAccount == ReportDoOrNot.Do) HideAccount();
        }
        private void HideApproval()
        {
            line4.Visible = false;
            line13.Visible = false;
            line16.Visible = false;
            line20.Visible = false;
            line23.Visible = false;
            line1.Visible = false;
        }
        private void HideQuantity()
        {
            lblQuantity.Visible = false;
            txtQuantity.Visible = false;
            lblUnitSymbl.Visible = false;
            txtUnitSymbol.Visible = false;
            lblUnitPrice.Visible = false;
            txtUnitPrice.Visible = false;

            line40.Visible = false;
            line27.Visible = false;
            line2.Visible = false;
            line17.Visible = false;
            line18.Visible = false;
            line6.Visible = false;
            line38.Visible = false;
            line19.Visible = false;
            line43.Visible = false;
            line7.Visible = false;
            line10.Visible = false;
            line44.Visible = false;
        }
        private void HideCustomerAddress()
        {
            lblPostalSymbol.Visible = false;
            lblCustomerPostCode.Visible = false;
            lblCustomerAddress1.Visible = false;
            lblCustomerAddress2.Visible = false;
        }
        private void HideSalesAt()
        {
            lblSalesAt.Visible = false;
            txtSalesAt.Visible = false;
            line5.Visible = false;
            line35.Visible = false;
            line12.Visible = false;
            line14.Visible = false;
        }
        private void HideAccount()
        {
            lblReceiveAccount2_pf.Visible = false;
            lblReceiveAccount3_pf.Visible = false;
            lblBankAccountName_pf.Visible = false;
        }
        #endregion

        #region ロゴ・印の初期化
        private void InitializePictures()
        {
            picLogo.Visible = false;
            picOfficialSeal.Visible = false;
            picRoundSeal.Visible = false;

            if (CompanyLogos == null) return;
            foreach (var companyLogo in CompanyLogos)
            {
                if (companyLogo.Logo.Length <= 0) continue;
                switch (companyLogo.LogoType)
                {
                    case (int)CompanyLogoType.Logo:
                        if(Setting.DisplayLogo == ReportDoOrNot.Do)
                            SizingPicture(companyLogo, picLogo);
                        break;
                    case (int)CompanyLogoType.SquareSeal:
                        if (Setting.DisplayOfficialSeal == ReportDoOrNot.Do)
                            SizingPicture(companyLogo, picOfficialSeal);
                        break;
                    case (int)CompanyLogoType.RoundSeal:
                        if (Setting.DisplayRoundSeal == ReportDoOrNot.Do)
                            SizingPicture(companyLogo, picRoundSeal);
                        break;
                    default:
                        continue;
                }
            }
        }
        private void SizingPicture(CompanyLogo logo, Picture picture)
        {
            var m_sngLogoWidth = picture.Width;
            var m_sngLogoHeight = picture.Height;
            //ロゴの設定
            var ms = new MemoryStream(logo.Logo);
            Bitmap bmp = new Bitmap(ms);
            picture.Image = bmp;
            if (picture.Image == null) return;
            picture.Visible = true;

            var intImageHeight = picture.Image.Height;
            var intImageWidth = picture.Image.Width;
            float sngSavWidth = 0;
            float sngSavHeight = 0;

            if (intImageWidth > intImageHeight)
            {
                //横長なら横固定で縦で調整
                picture.Width = m_sngLogoWidth;
                picture.Height = Convert.ToSingle(m_sngLogoWidth * intImageHeight / intImageWidth);

                ////縦がオーバーするなら縦横比率はそのままでイメージの大きさを調整
                if (picture.Height > m_sngLogoHeight)
                {
                    sngSavWidth = Convert.ToSingle(picture.Width * m_sngLogoHeight / picture.Height);
                    sngSavHeight = Convert.ToSingle(picture.Height * m_sngLogoHeight / picture.Height);
                }
            }
            else
            {
                //縦長なら縦固定で横で調整
                picture.Width = Convert.ToSingle(m_sngLogoHeight * intImageWidth / intImageHeight);
                picture.Height = m_sngLogoHeight;

                //横がオーバーするなら縦横比率はそのままでイメージの大きさを調整
                if (picture.Width > m_sngLogoWidth)
                {
                    sngSavWidth = Convert.ToSingle(picture.Width * m_sngLogoWidth / picture.Width);
                    sngSavHeight = Convert.ToSingle(picture.Height * m_sngLogoWidth / picture.Width);
                }
            }
            if (sngSavWidth > 0)
            {
                picture.Width = sngSavWidth;
                picture.Height = sngSavHeight;
            }

        }
        #endregion
       
        #region データソースの加工

        public BindingSource BuildDataSource(List<BillingInvoiceDetailForPrint> invoiceDetails,
            Company company)
        {
            var frikomi = "振込銀行：";
            var receiveAccount1 = frikomi
                + company.BankName1
                + " " + company.BranchName1
                + " " + company.AccountType1
                + " " + company.AccountNumber1;
            var receiveAccount2 = frikomi
                + company.BankName2
                + " " + company.BranchName2
                + " " + company.AccountType2
                + " " + company.AccountNumber2;
            var receiveAccount3 = frikomi
                + company.BankName3
                + " " + company.BranchName3
                + " " + company.AccountType3
                + " " + company.AccountNumber3;
            var accountName = "口座名義人：" + company.BankAccountName;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ja-JP", false);
            ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();

            var listbillingInputId = invoiceDetails.GroupBy(x => x.BillingInputId).Select(r => new { BillingInputId = r.Key, RowCount = 0 });

            foreach (var detail in invoiceDetails)
            {
                if (!detail.DestinationId.HasValue || detail.DestinationId.Value == 0)
                {
                    var isNullDepartment = string.IsNullOrEmpty(detail. CustomerDestinationDepartmentName);
                    var isNullAddressee = string.IsNullOrEmpty(detail.CustomerStaffName);
                    if (isNullDepartment && isNullAddressee)
                    {
                        detail.CustomerName = detail.CustomerName + " " + detail.CustomerHonorific;
                    }
                    else if (!isNullDepartment && isNullAddressee)
                    {
                        detail.DestinationDepartmentName = detail.CustomerDestinationDepartmentName + " " + detail.CustomerHonorific;
                    }
                    else if (isNullDepartment && !isNullAddressee)
                    {
                        detail.DestinationDepartmentName = detail.CustomerStaffName + " " + detail.CustomerHonorific;
                        detail.DestinationAddressee = "";
                    }
                    else
                    {
                        detail.DestinationDepartmentName = detail.CustomerDestinationDepartmentName;
                        detail.DestinationAddressee = detail.CustomerStaffName + " " + detail.CustomerHonorific;
                    }
                }
                else
                {
                    detail.CustomerPostalCode = detail.DestinationPostalCode;
                    detail.CustomerAddress1 = detail.DestinationAddress1;
                    detail.CustomerAddress2 = detail.DestinationAddress2;
                    detail.CustomerName = string.IsNullOrEmpty(detail.DestinationName) ? detail.CustomerName : detail.DestinationName;

                    var isNullDepartment = string.IsNullOrEmpty(detail.DestinationDepartmentName);
                    var isNullAddressee = string.IsNullOrEmpty(detail.DestinationAddressee);
                    if (isNullDepartment && isNullAddressee)
                    {
                        detail.CustomerName = detail.CustomerName + " " + detail.DestinationHonorific;
                    }
                    else if (!isNullDepartment && isNullAddressee)
                    {
                        detail.DestinationDepartmentName = detail.DestinationDepartmentName + " " + detail.DestinationHonorific;
                    }
                    else if (isNullDepartment && !isNullAddressee)
                    {
                        detail.DestinationDepartmentName = detail.DestinationAddressee + " " + detail.DestinationHonorific;
                        detail.DestinationAddressee = "";
                    }
                    else
                    {
                        detail.DestinationAddressee = detail.DestinationAddressee + " " + detail.DestinationHonorific;
                    }
                }

                if (detail.ReceiveAccountId1 == 1)
                {
                    detail.ReceiveAccount1 = receiveAccount1;
                }

                if (detail.ReceiveAccountId2 == 1)
                {
                    if (string.IsNullOrEmpty(detail.ReceiveAccount1))
                    {
                        detail.ReceiveAccount1 = receiveAccount2;
                    }
                    else
                    {
                        detail.ReceiveAccount2 = receiveAccount2;
                    }
                }

                if (detail.ReceiveAccountId3 == 1)
                {
                    if (string.IsNullOrEmpty(detail.ReceiveAccount1))
                    {
                        detail.ReceiveAccount1 = receiveAccount3;
                    }
                    else if (string.IsNullOrEmpty(detail.ReceiveAccount2))
                    {
                        detail.ReceiveAccount2 = receiveAccount3;
                    }
                    else
                    {
                        detail.ReceiveAccount3 = receiveAccount3;
                    }
                }

                if (string.IsNullOrEmpty(detail.ReceiveAccount1))
                {

                }
                else if (string.IsNullOrEmpty(detail.ReceiveAccount2))
                {
                    detail.ReceiveAccount2 = accountName;
                }
                else if (string.IsNullOrEmpty(detail.ReceiveAccount3))
                {
                    detail.ReceiveAccount3 = accountName;
                }
                else
                {
                    detail.BankAccountName = accountName;
                }

                if (!string.IsNullOrEmpty(detail.DueDateComment))
                {
                    string dueAt;
                    switch (detail.DueDateFormat)
                    {
                        case 0:
                            //yyyy年MM月dd日
                            dueAt = detail.DueAt.ToString("yyyy年MM月dd日");
                            break;
                        case 1:
                            //yyyy/MM/dd
                            dueAt = detail.DueAt.ToString("yyyy/MM/dd");
                            break;
                        case 2:
                            //ee年MM月dd日
                            dueAt = detail.DueAt.ToString("gy年MM月dd日", ci);
                            break;
                        case 3:
                            //ee/MM/dd
                            dueAt = detail.DueAt.ToString("y/MM/dd/", ci);
                            break;
                        default:
                            //yyyy年MM月dd日
                            dueAt = detail.DueAt.ToString("yyyy年MM月dd日");
                            break;
                    }
                    detail.DueDateComment = detail.DueDateComment.Replace("[YMD]", dueAt);
                    detail.Tel = company.Tel;
                    detail.Fax = company.Fax;
                }

                //手数料負担コメントは相手先負担の場合のみ表示する
                if (detail.ShareTransferFee > 0)
                {
                    detail.TransferFeeComment = string.Empty;
                }
            }

            // 空の明細行を表示させるために空データを入れる処理
            PadingBlankRows(invoiceDetails);

            return new BindingSource(invoiceDetails, null);
        }

        /// <summary>
        /// 空の明細行を表示させるために空データを入れる処理
        /// </summary>
        /// <param name="invoiceDetails"></param>
        private void PadingBlankRows(List<BillingInvoiceDetailForPrint> invoiceDetails)
        {
            var keys = invoiceDetails.GroupBy(x => x.BillingInputId)
                .Select(y => new { BillingInputId = y.Key, Count = y.Count() });

            foreach (var key in keys)
            {
                var needBlankRow = GetBlankRowNumber(key.Count);
                if (needBlankRow == 0) continue;

                var listIndex = invoiceDetails.Select((i, j) => new { BillingInputId = i.BillingInputId, Index = j })
                    .Where(x => x.BillingInputId == key.BillingInputId)
                    .Select(y => y.Index);

                var last = listIndex.Last();

                var clone = CloneDeepForInvoice(invoiceDetails[last]);

                clone.SalesAt = null;
                clone.Note1 = string.Empty;
                clone.Note2 = string.Empty;
                clone.Quantity = null;
                clone.UnitSymbol = string.Empty;
                clone.UnitPrice = null;
                clone.BillingAmount = 0m;
                clone.RemainAmount = 0m;
                clone.TaxExcludedPrice = 0m;
                clone.TaxAmount = 0m;
                clone.TaxClassName = string.Empty;

                for (var i = 0; i < needBlankRow; ++i)
                {
                    invoiceDetails.Insert(last + 1, clone);
                }
            }
        }

        /// <summary>
        /// 必要なブランク行数を取得する
        /// </summary>
        /// <param name="detailCount"></param>
        /// <returns></returns>
        private int GetBlankRowNumber(int detailCount)
        {
            var rowNumber = 0;
            var countBuffer = 0;

            if (detailCount <= MaxRowCountFirstPage)
            {
                rowNumber = MaxRowCountFirstPage;
                countBuffer = 0;
            }
            else
            {
                rowNumber = MaxRowCountOtherPage;
                countBuffer = MaxRowCountFirstPage;
            }
            var rest = (detailCount - countBuffer) % rowNumber;
            if (rest == 0) return 0;
            return rowNumber - rest;
        }

        /// <summary>
        /// 請求明細Objectのディープコピーメソッド
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static BillingInvoiceDetailForPrint CloneDeepForInvoice(BillingInvoiceDetailForPrint target)
        {
            object clone = null;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, target);
                stream.Position = 0;
                clone = formatter.Deserialize(stream);
            }
            return (BillingInvoiceDetailForPrint)clone;
        }
        #endregion
        #endregion

        #region イベントハンドラー
        private void ghInvoiceOnPage1_BeforePrint(object sender, EventArgs e)
        {
            //2ページ目以降は挨拶文,宛先を非表示にする
            if (!IsFirstPage(reportInfo2.Text))
            {
                txtGreeting.Visible = false;
                lblPostalSymbol.Visible = false;
                lblCustomerPostCode.Visible = false;
                lblCustomerAddress1.Visible = false;
                lblCustomerAddress2.Visible = false;
                lblCustomerName.Visible = false;
                lblCustomerDepartmentName.Visible = false;
                lblCustomerAddressee.Visible = false;
            }
        }
        private bool IsFirstPage(string pageString)
        {
            var array = pageString.Split('/');
            var first = GetNumberFromString(array[0]);
            return 1 == first;
        }
        /// <summary>
        /// 文字列から数値のみを抽出する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private long GetNumberFromString(string value)
        {
            var a = new string(value.Where(x => char.IsDigit(x)).ToArray());
            return Convert.ToInt64(a);
        }
        private int counter { get; set; } = 0;
        private bool isTopRow { get; set; }
        private void detail_Format(object sender, EventArgs e)
        {
            counter++;
            line25.Visible = counter != 1;
            if (isTopRow)
            {
                line25.Visible = false;
                isTopRow = false;
            }
            var detailCount = Convert.ToInt32(txtDetailCount.Text);

            if (IsNewPageTiming(counter, detailCount))
            {
                line48.Visible = true;
                detail.NewPage = NewPage.After;
                isTopRow = true;
            }
            else
            {
                line48.Visible = false;
                detail.NewPage = NewPage.None;
            }
        }
        private bool IsNewPageTiming(int counter, int detailCount)
        {
            var blankRow = GetBlankRowNumber(detailCount);
            detailCount += blankRow;
            if (detailCount <= MaxRowCountFirstPage
                || counter < MaxRowCountFirstPage)
                return false;

            if (counter == MaxRowCountFirstPage) return true;

            if (counter == detailCount) return false;

            if ((counter - MaxRowCountFirstPage) % MaxRowCountOtherPage == 0
                && counter != detailCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void gfInvoice_BeforePrint(object sender, EventArgs e)
        {
            counter = 0;
        }
        private void ghInvoice_Format(object sender, EventArgs e)
        {
            var displayStaff = Convert.ToInt32(txtDisplayStaff.Text);
            lblStaff.Visible = displayStaff != 0;
            lblStaffName.Visible = displayStaff != 0;
        }

        private void pageFooter1_BeforePrint(object sender, EventArgs e)
        {
            //振込先を毎ページ表示したい。（振込先情報は明細上で毎に持っている）
            //グループフッターを利用すると最終ページのみ印字され、毎ページ表示することはActiveReportの仕様上、不可能。
            //しかしグループヘッダーの場合はプロパティを「GroupHeader.RepeatStyle = RepeatStyle.OnPage」
            //に設定することで毎ページ表示することが可能。
            //グループヘッダーで一度持っておき、ページフッター描画直前に渡すことで、振込先を毎ページ表示できる
            //方法：
            //BillingInvoiceReport_PageStartイベントで振込先情報をフィールド上の配列変数に入れて置き、
            //pageFooter1_BeforePrintイベントでページフッターの描写直前にページフッター上TextBoxに振込先情報を渡す。
            //以上のことをすることで振込先情報を毎ページ表示している。

            txtDueDateComment_pf.Text = dueDateComment[j];
            lblReceiveAccount1_pf.Text = receiveAccount1[j];
            lblReceiveAccount2_pf.Text = receiveAccount2[j];
            lblReceiveAccount3_pf.Text = receiveAccount3[j];
            lblBankAccountName_pf.Text = bankAccountName[j];
            txtTransferFeeComment_pf.Text = transferFeeComment[j];
            j += 1;
        }
        private void ghInvoice_BeforePrint(object sender, EventArgs e)
        {
            txtBillingAmountSumHeader.Text = "\\" + txtBillingAmountSumHeader.Text;
        }

        private void BillingInvoiceReport_PageStart(object sender, EventArgs e)
        {
            dueDateComment[i] = txtDueDateComment_gh.Text;
            receiveAccount1[i] = lblReceiveAccount1_gh.Text;
            receiveAccount2[i] = lblReceiveAccount2_gh.Text;
            receiveAccount3[i] = lblReceiveAccount3_gh.Text;
            bankAccountName[i] = lblBankAccountName_gh.Text;
            transferFeeComment[i] = txtTransferFeeComment_gh.Text;
            i += 1;
        }
        #endregion

        #region　印刷設定
        public enum ReportInvoiceDate
        {
            PublicAt = 0
        , ClosingAt
        , BilledAt
        }
        public enum ReportInvoiceAmount
        {
            BillingAmount = 0
        , RemainAmount
        }
        public class BillingInvoiceReportSetting
        {
            public BillingInvoiceReportSetting(List<ReportSetting> listSetting)
            {
                ReportInvoiceDate       = listSetting.GetReportSetting<ReportInvoiceDate>(PC0401.ReportInvoiceDate);
                DisplayLogo             = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayLogo);
                DisplayOfficialSeal     = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayOfficialSeal);
                DisplayRoundSeal        = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayRoundSeal);
                DisplayAddress          = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayAddress);
                DisplayApproval         = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayApproval);
                ReportInvoiceAmount     = listSetting.GetReportSetting<ReportInvoiceAmount>(PC0401.ReportInvoiceAmount);
                DisplayQuantity         = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayQuantity);
                DisplaySalesAt          = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplaySalesAt);
                DisplayVirtualAccount   = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayVirtualAccount);
                OutputCopy              = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.OutputCopy);
                DisplayRePrint          = listSetting.GetReportSetting<ReportDoOrNot>(PC0401.DisplayRePrint);
            }
            public ReportInvoiceDate ReportInvoiceDate { get; set; }
            public ReportDoOrNot DisplayLogo { get; set; }
            public ReportDoOrNot DisplayOfficialSeal { get; set; }
            public ReportDoOrNot DisplayRoundSeal { get; set; }
            public ReportDoOrNot DisplayAddress { get; set; }
            public ReportDoOrNot DisplayApproval { get; set; }
            public ReportInvoiceAmount ReportInvoiceAmount { get; set; }
            public ReportDoOrNot DisplayQuantity { get; set; }
            public ReportDoOrNot DisplaySalesAt { get; set; }
            public ReportDoOrNot DisplayVirtualAccount { get; set; }
            public ReportDoOrNot OutputCopy { get; set; }
            public ReportDoOrNot DisplayRePrint { get; set; }
        }
        #endregion

    }
}
