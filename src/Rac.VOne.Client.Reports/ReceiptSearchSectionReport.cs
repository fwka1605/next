using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using GrapeCity.ActiveReports.SectionReportModel;
using Rac.VOne.Web.Models;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// PaymentDataSectionReport の概要の説明です。
    /// </summary>
    public partial class ReceiptSearchSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private bool UseReceiptSection { get; set; } = false;
        //   true:削除済みデータ false:未削除データ ,
        private bool IsDeleted { get; set; } = false;

        public ReceiptSearchSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblcompanycode.Text = companycode + " " + companyname;
        }

        public void SetData(List<Web.Models.Receipt> receiptData, int precision, bool useSection, bool isDeleted, List<GridSetting> gridSettingInfo)
        {
            if (gridSettingInfo != null)
            {
                foreach (var gridSetting in gridSettingInfo)
                {
                    switch (gridSetting.ColumnName)
                    {
                        case "Note1":
                            lblNote.Text = gridSetting.ColumnNameJp;
                            break;
                    }
                }
            }

            IsDeleted = isDeleted;
            DataSource = new BindingSource(receiptData, null);
            UseReceiptSection = useSection;

            string displayFormat = "#,##0";
            if (precision > 0)
            {
                displayFormat += "." + new string('0', precision);
            }

            SetSectionValue();

            txtReceiptId.DataField = "Id";
            if (IsDeleted)
            {
                txtAssignmentFlag.DataField = "DeleteAt";
                lblAssignmentFlg.Text = "削除日";
            }
            else
            {
                txtAssignmentFlag.DataField = " AssignmentFlagName";
                txtAssignmentFlag.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center;
                lblAssignmentFlg.Text = "消込区分";
            }

            txtRecordedAt.DataField = "RecordedAt";
            txtDueAt.DataField = "DueAt";
            txtPaymentCategory.DataField = "CategoryCodeName";
            txtInputType.DataField = "InputTypeCodeName";
            txtPayerName.DataField = "PayerName";
            txtNote1.DataField = "Note1";
            txtSectionCode.DataField = "SectionCode";
            txtSectionName.DataField = "SectionName";
            txtReceiptAmount.DataField = "ReceiptAmount";
            txtReceiptAmount.OutputFormat = displayFormat;
            txtRemainAmount.DataField = "RemainAmount";
            txtRemainAmount.OutputFormat = displayFormat;
            txtExcludeCategory.DataField = "ExcludeCategoryCodeAndName";
            txtExcludeAmount.DataField = "ExcludeAmount";
            txtExcludeAmount.OutputFormat = displayFormat;
            txtBankCode.DataField = "BankCode";
            txtBankName.DataField = "BankName";
            txtBranchCode.DataField = "BranchCode";
            txtBranchName.DataField = "BranchName";
            txtAccountNumber.DataField = "AccountNumber";
            txtSourceBankName.DataField = "SourceBankName";
            txtSourceBranchName.DataField = "SourceBranchName";
            txtReceiptAmtTotal.OutputFormat = displayFormat;
            txtRemainAmtTotal.OutputFormat = displayFormat;
            txtExcludeAmtTotal.OutputFormat = displayFormat;

        }

        private void SetSectionValue()
        {
            if (!UseReceiptSection)
            {
                lblSection.Visible = false;
                txtSectionCode.Visible = false;
                txtSectionName.Visible = false;
                lblPayerName.Width += lblSection.Width;

                lblNote.Width += lblSection.Width;
                lineHeaderVerPayerName.Visible = false;
                lineHeaderHorReceiptId.Width += lblSection.Width;
                lineDetailVerPayerName.Visible = false;
            }
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }
    }
}
