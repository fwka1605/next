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
using System.Linq;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerGroupSectionReport の概要の説明です。
    /// </summary>
    public partial class CustomerGroupSectionReport : GrapeCity.ActiveReports.SectionReport, IMasterSectionReport<CustomerGroup>
    {

        public CustomerGroupSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
            InitializeUserComponent();
        }
        private void InitializeUserComponent()
        {
            txtParentCustomerCode.DataField = nameof(CustomerGroup.ParentCustomerCode);
            txtParentCustomerName.DataField = nameof(CustomerGroup.ParentCustomerName);
            txtParentCustomerKana.DataField = nameof(CustomerGroup.ParentCustomerKana);
            txtChildCustomerCode.DataField = nameof(CustomerGroup.ChildCustomerCode);
            txtChildCustomerName.DataField = nameof(CustomerGroup.ChildCustomerName);
            ParentCustomerCodeBuffer = string.Empty;
            FetchData += rpt_FetchData;
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyName.Text = companycode + " " + companyname;
        }



        private string ParentCustomerCodeBuffer { get; set; }
        private void rpt_FetchData(object sender, FetchEventArgs e)
        {
            var changed = !ParentCustomerCodeBuffer.Equals(Fields[nameof(CustomerGroup.ParentCustomerCode)].Value);

            txtParentCustomerCode.Visible = changed;
            txtParentCustomerName.Visible = changed;
            txtParentCustomerKana.Visible = changed;

            ParentCustomerCodeBuffer = Fields[nameof(CustomerGroup.ParentCustomerCode)].Value.ToString();
        }

        public void SetData(IEnumerable<CustomerGroup> items)
        {
            this.DataSource = new BindingSource(items, null);
        }
    }
}
