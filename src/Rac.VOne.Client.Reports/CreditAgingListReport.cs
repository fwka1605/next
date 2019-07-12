using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Rac.VOne.Web.Models;
using System.Windows.Forms;
using System.Linq;
using GrapeCity.ActiveReports.SectionReportModel;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CreditAgingList の概要の説明です。
    /// </summary>
    public partial class CreditAgingListReport : GrapeCity.ActiveReports.SectionReport
    {
        private List<CreditAgingList> CreditAgingPrintList { get; set; }

        public bool RequireStaffTotal { private get; set; }
        public bool RequireDepartmentTotal { private get; set; }
        public bool DisplayCustomerCode { private get; set; }
        public bool ConsiderCustomerGroup { private get; set; }
        public bool UseMasterStaff { private get; set; }


        public CreditAgingListReport()
        {
            InitializeComponent();
        }

        public void SetBasicPageSetting(string CompanyCode, string CompanyName)
        {
            lblCompanyCodeName.Text = CompanyCode + " " + CompanyName;
        }

        public void SetPageDataSetting(List<CreditAgingList> GeneralResult)
        {
            CreditAgingPrintList = GeneralResult;
            DataSource = new BindingSource(GeneralResult, null);
            lblDepartmentCode.DataField = nameof(CreditAgingList.DepartmentCode);
            lblDepartmentCodeAndName.DataField = nameof(CreditAgingList.DepartmentName);
            lblStaffCode.DataField = nameof(CreditAgingList.StaffCode);
            lblStaffCodeAndName.DataField = nameof(CreditAgingList.StaffName);
            txtSubTotal.DataField = "SubTotal";
            txtCollectCategory.DataField = nameof(CreditAgingList.TotalText);
            txtCreditAmount.DataField = nameof(CreditAgingList.CreditAmount);
            txtUnsettledRemain.DataField = nameof(CreditAgingList.UnsettledRemain);
            txtBillingRemain.DataField = nameof(CreditAgingList.BillingRemain);
            txtCreditLimit.DataField = nameof(CreditAgingList.CreditLimit);
            txtCreditRemain.DataField = nameof(CreditAgingList.CreditBalance);
            txtCreditRemainStar.DataField =nameof(CreditAgingList.CreditBalanceMark);
            txtArrivalDueDate1.DataField = nameof(CreditAgingList.ArrivalDueDate1);
            txtArrivalDueDate2.DataField = nameof(CreditAgingList.ArrivalDueDate2);
            txtArrivalDueDate3.DataField = nameof(CreditAgingList.ArrivalDueDate3);
            txtArrivalDueDate4.DataField = nameof(CreditAgingList.ArrivalDueDate4);

            var displayFormat = "#,##0";
            txtCreditAmount.OutputFormat = displayFormat;
            txtUnsettledRemain.OutputFormat = displayFormat;
            txtBillingRemain.OutputFormat = displayFormat;
            txtCreditLimit.OutputFormat = displayFormat;
            txtCreditRemain.OutputFormat = displayFormat;
            txtArrivalDueDate1.OutputFormat = displayFormat;
            txtArrivalDueDate2.OutputFormat = displayFormat;
            txtArrivalDueDate3.OutputFormat = displayFormat;
            txtArrivalDueDate4.OutputFormat = displayFormat;

            lblDepartment.Visible = RequireDepartmentTotal;
            lblDepartmentCode.Visible = RequireDepartmentTotal;
            lblDepartmentCodeAndName.Visible = RequireDepartmentTotal;

            lblStaff.Visible = RequireStaffTotal;
            lblStaffCode.Visible = RequireStaffTotal;
            lblStaffCodeAndName.Visible = RequireStaffTotal;
            if (!UseMasterStaff) HideCreditLimitRelatedControls();

        }
        private void HideCreditLimitRelatedControls()
        {
            #region visible = false
            lblCreditRemain.Visible = false;
            lblCreditLimit.Visible = false;
            txtCreditRemainStar.Visible = false;
            txtCreditRemain.Visible = false;
            txtCreditLimit.Visible = false;
            lineHeaderVerCreditLimit1.Visible = false;
            lineHeaderVerBillingRemain.Visible = false;
            lineDetailVerCreditLimit1.Visible = false;
            lineDetailVerBillingRemain.Visible = false;
            #endregion
            var width = lblCreditRemain.Width + lblCreditLimit.Width;
            #region move to right
            lblBillingRemain.Left += width;
            txtBillingRemain.Left += width;
            lineHeaderVerUnsettleRemain2.Left += width;
            lineHeaderVerUnsettleRemain1.Left += width;
            lineDetailVerUnsettledRemain2.Left += width;
            lineDetailVerUnsettledRemain1.Left += width;
            lblUnsettleRemain.Left += width;
            txtUnsettledRemain.Left += width;
            lineHeaderVerCreditAmount2.Left += width;
            lineHeaderVerCreditAmount1.Left += width;
            lineDetailVerCreditAmount2.Left += width;
            lineDetailVerCreditAmount1.Left += width;

            lblCreditAmount.Left += width;
            txtCreditAmount.Left += width;
            lineHeaderVerCollectCategory.Left += width;
            lineDetailVerCollectCategory.Left += width;

            lblCollectCategory.Left += width;
            txtCollectCategory.Left += width;
            lineHeaderVerCustomerCode.Left += width;
            lineDetailVerSubTotal.Left += width;
            #endregion
            #region expand control
            lblCustomerCode.Width += width;
            txtCustomerCode.Width += width;
            #endregion
        }


        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }

        private int RowIndex { get; set; }
        private void detail_Format(object sender, EventArgs e)
        {
            var current = CreditAgingPrintList[RowIndex];
            RowIndex++;
            var next = RowIndex < CreditAgingPrintList.Count ? CreditAgingPrintList[RowIndex] : null;

            switch (current.RecordType)
            {
                case 0:
                    if (ConsiderCustomerGroup && current.ParentCustomerFlag != 1 && current.ParentCustomerId.HasValue)
                    {
                        txtCustomerCode.Text = DisplayCustomerCode
                            ? $"  {current.CustomerCode} {current.CustomerName}"
                            : $"  {current.CustomerName}";
                    }
                    else
                    {
                        txtCustomerCode.Text = DisplayCustomerCode
                            ? $"{current.CustomerCode} {current.CustomerName}"
                            : current.CustomerName;
                    }
                    detail.BackColor = Color.Transparent;
                    break;
                case 1:
                    lblStaffCodeAndName.Text = $"{current.StaffCode}";
                    lineDetailVerSubTotal.Height = 0;
                    txtCustomerCode.Text = $"{current.StaffCode} {current.StaffName}";
                    detail.BackColor = Color.WhiteSmoke;
                    break;
                case 2:
                    lblDepartmentCodeAndName.Text = $"{current.DepartmentCode}";
                    lineDetailVerSubTotal.Height = 0;
                    txtCustomerCode.Text = $"{current.DepartmentCode} {current.DepartmentName}";
                    detail.BackColor = Color.WhiteSmoke;
                    break;
                case 3:
                    lineDetailVerSubTotal.Height = 0;
                    txtCustomerCode.Text = $"{current.TotalText}";
                    txtCollectCategory.Text = null;
                    txtSubTotal.Text = null;
                    detail.BackColor = Color.WhiteSmoke;
                    break;
            }

            if (current.RecordType != 0 && next?.RecordType == 0)
            {
                this.detail.NewPage = NewPage.After;
            }
            else
            {
                this.detail.NewPage = NewPage.None;
            }
        }
    }
}
