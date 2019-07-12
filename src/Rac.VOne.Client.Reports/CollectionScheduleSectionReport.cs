using GrapeCity.ActiveReports.SectionReportModel;
using Rac.VOne.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Rac.VOne.Client.Reports
{
    /// <summary>回収予定表 帳票</summary>
    public partial class CollectionScheduleSectionReport : GrapeCity.ActiveReports.SectionReport
    {

        private const float lineWeight = 0.005F;
        private const int MaxRowsCount = 25;
        private enum eTextBox
        {
            Kubun,
            KinZ,
            Kin0,
            Kin1,
            Kin2,
            Kin3,
            KinK,
        }

        public bool NewPagePerStaff { get; set; }
        public bool NewPagePerDepartment { get; set; }
        public bool GroupByDepartment { get; set; }

        private List<CollectionSchedule> OriginalSource { get; set; }
        private List<CollectionSchedule> ReportSource { get; set; }

        private int CollectCategoryCount { get; set; }

        private string DepartmentCode { get; set; }
        private string StaffCode { get; set; }

        private int RowIndex { get; set; }
        private int RowNumber { get; set; }
        public CollectionScheduleSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();

            txtCustomerCodeandName.DataField = nameof(CollectionSchedule.CustomerInfo);
            txtClosingDay.DataField = nameof(CollectionSchedule.Closing);
            txtStaffName.DataField = nameof(CollectionSchedule.StaffName);
            lblHeaderDepartmentName.DataField = nameof(CollectionSchedule.DepartmentName);
            txtCategoryName.DataField = nameof(CollectionSchedule.CollectCategoryName);
            txtUncollectedAmountLast.DataField = nameof(CollectionSchedule.UncollectedAmountLast);
            txtUncollectedAmount0.DataField = nameof(CollectionSchedule.UncollectedAmount0);
            txtUncollectedAmount1.DataField = nameof(CollectionSchedule.UncollectedAmount1);
            txtUncollectedAmount2.DataField = nameof(CollectionSchedule.UncollectedAmount2);
            txtUncollectedAmount3.DataField = nameof(CollectionSchedule.UncollectedAmount3);
        }

        public void SetBasicPageSetting(string CompanyCode, string CompanyName)
        {
            lblcompanycode.Text = CompanyCode + " " + CompanyName;
        }

        public void SetData(List<CollectionSchedule> list)
        {
            var offset = 0.0F;
            if (!NewPagePerStaff)
            {
                DisableStaffTotal();
                offset += lblHeaderStaff.Height;
            }

            if (!NewPagePerDepartment)
            {
                DisableDepartmentTotal();
                offset += lblHeaderDepartment.Height;
            }

            if (!GroupByDepartment)
            {
                DisableDepartmentColumn();
            }

            if (offset > 0.0F)
            {
                OffsetHeaderControls(offset);
            }

            OriginalSource = list;
            ReportSource = list.Where(x => x.RecordType == 0).ToList();

            CollectCategoryCount = list.Count(x => x.RecordType == 3);
            if (NewPagePerStaff)
                InitializeCollectCategories(grfStaff);

            if (NewPagePerDepartment)
                InitializeCollectCategories(grfDepartment);

            InitializeCollectCategories(grfTotal);

            DataSource = new System.Windows.Forms.BindingSource(ReportSource, null);

        }

        private void DisableStaffTotal()
        {
            grhStaff.GroupKeepTogether = GroupKeepTogether.None;
            grfStaff.KeepTogether = false;
            grfStaff.NewPage = NewPage.None;
            grhStaff.Visible = false;
            grfStaff.Visible = false;
            lblHeaderStaff.Visible = false;
            lblHeaderStaffCode.Visible = false;
            lblHeaderStaffName.Visible = false;
        }

        private void DisableDepartmentTotal()
        {
            grhDepartment.GroupKeepTogether = GroupKeepTogether.None;
            grfDepartment.KeepTogether = false;
            grfDepartment.NewPage = NewPage.None;
            grfDepartment.Visible = false;
            grhDepartment.Visible = false;
            lblHeaderDepartment.Visible = false;
            lblHeaderDepartmentName.Visible = false;
            lblHeaderDepartmentCode.Visible = false;
        }

        private void DisableDepartmentColumn()
        {
            var width = lblBumon.Width;
            lblBumon.Visible = false;
            lineHeaderV04.Visible = false;
            lineHeaderV01.Left += width;
            lineHeaderV02.Left += width;
            lineHeaderV03.Left += width;
            lblClosingDay.Left += width;
            lblTanto.Left += width;
            lblCustomer.Width += width;
            lblCustomerCollectInfo.Width += width;
            txtDepartment.Visible = false;
            lineDV04.Visible = false;
            lineDV01.Left += width;
            lineDV02.Left += width;
            lineDV03.Left += width;
            txtClosingDay.Left += width;
            txtStaffName.Left += width;
            txtCustomerCodeandName.Width += width;
        }

        private void OffsetHeaderControls(float offset)
        {
            foreach (var item in pageHeader.Controls.Cast<ARControl>().Where(x => IsMovableHeaderControl(x)))
            {
                item.Top -= offset;
            }
            pageHeader.Height -= offset;
        }

        private bool IsMovableHeaderControl(ARControl control)
        {
            if (control.Name.StartsWith("lineHead")) return true;
            if (control == lblCustomer) return true;
            if (control == lblCustomerCollectInfo) return true;
            if (control == lblClosingDay) return true;
            if (control == lblTanto) return true;
            if (control == lblBumon) return true;
            if (control == lblKubun) return true;
            if (control == lblUncollectedAmountLast) return true;
            if (control == lblUncollectAmount0) return true;
            if (control == lblUncollectAmount1) return true;
            if (control == lblUncollectAmount2) return true;
            if (control == lblUncollectAmount3) return true;
            if (control == lblHeaderStaff) return true;
            if (control == lblHeaderStaffCode) return true;
            if (control == lblHeaderStaffName) return true;
            if (control == lblKingakuK) return true;
            return false;
        }

        private void InitializeCollectCategories(GroupFooter footer)
        {
            var count = CollectCategoryCount;
            footer.Height = footer.Height * count + lineWeight;
            foreach (var item in footer.Controls.Cast<ARControl>())
            {
                if (item.Name.StartsWith("lblCaption")
                    || item.Name.StartsWith("lineV"))
                    item.Height = item.Height * count;
                if (item.Name.StartsWith("lineBottom"))
                    item.Top = item.Top * count;
            }
            for (var i = 0; i < count; i++)
            {
                SetSubtoalCategoryItems(footer, i);
            }
            footer.Format += grf_Format;
        }

        private void SetSubtoalCategoryItems(GroupFooter footer, int recordIndex)
        {
            var fields = Enum.GetValues(typeof(eTextBox));
            var items = new ARControl[fields.Length];
            foreach (eTextBox field in fields)
            {
                var index = (int)field;
                var item = new TextBox();
                var source = GetSourceTextBox(field);
                item.Location = new PointF(source.Left, source.Height * recordIndex);
                item.Size = source.Size;
                item.Style = source.Style;
                item.MultiLine = source.MultiLine;
                item.OutputFormat = source.OutputFormat;
                item.Padding = source.Padding;
                item.Name = $"{footer.Name}{source.Name}{recordIndex}";
                items[index] = item;
            }
            footer.Controls.AddRange(items);
            if (recordIndex == 0) return;
            var line = new Line { LineStyle = LineStyle.Dot };
            line.Location = new PointF(txtCategoryName.Left, txtCategoryName.Height * recordIndex);
            line.Width = txtUncollectedAmountTotal.Width + txtUncollectedAmountTotal.Left - txtCategoryName.Left;
            footer.Controls.Add(line);
        }

        private TextBox GetSourceTextBox(eTextBox field)
        {
            switch (field)
            {
                case eTextBox.Kubun: return txtCategoryName;
                case eTextBox.KinZ:  return txtUncollectedAmountLast;
                case eTextBox.Kin0:  return txtUncollectedAmount0;
                case eTextBox.Kin1:  return txtUncollectedAmount1;
                case eTextBox.Kin2:  return txtUncollectedAmount2;
                case eTextBox.Kin3:  return txtUncollectedAmount3;
                case eTextBox.KinK:  return txtUncollectedAmountTotal;
            }
            return null;
        }

        private decimal GetFieldValue(CollectionSchedule item, eTextBox field)
        {
            switch (field)
            {
                case eTextBox.KinZ: return item?.UncollectedAmountLast ?? 0M;
                case eTextBox.Kin0: return item?.UncollectedAmount0 ?? 0M;
                case eTextBox.Kin1: return item?.UncollectedAmount1 ?? 0M;
                case eTextBox.Kin2: return item?.UncollectedAmount2 ?? 0M;
                case eTextBox.Kin3: return item?.UncollectedAmount3 ?? 0M;
                case eTextBox.KinK: return item?.UncollectedAmountTotal ?? 0M;
            }
            return 0M;
        }

        private void grf_Format(object sender, EventArgs e)
        {
            var index = 0;
            var fields = Enum.GetValues(typeof(eTextBox));
            var footer = sender as GroupFooter;
            foreach (var item in OriginalSource.Where(x => x.RecordType == 3))
            {
                foreach (eTextBox field in fields)
                {
                    var source = GetSourceTextBox(field);
                    if (field == eTextBox.Kubun)
                        ((TextBox)footer.Controls[$"{footer.Name}{source.Name}{index}"]).Text = item.CollectCategoryName;
                    else
                    {
                        var subItem = GetSubItem(footer, item);
                        ((TextBox)footer.Controls[$"{footer.Name}{source.Name}{index}"]).Value = GetFieldValue(subItem, field);
                    }
                }
                index++;
            }
            RowNumber = 0;
        }

        private CollectionSchedule GetSubItem(GroupFooter footer, CollectionSchedule item)
        {
            if (footer == grfTotal) return item;
            Func<CollectionSchedule, bool> selector = null;
            if (footer == grfStaff)
                selector = x =>
                    x.RecordType == 1 &&
                    x.CollectCategoryCode == item.CollectCategoryCode &&
                    x.StaffCode == StaffCode &&
                    (!NewPagePerDepartment || x.DepartmentCode == DepartmentCode);
            else if (footer == grfDepartment)
                selector = x =>
                    x.RecordType == 2 &&
                    x.CollectCategoryCode == item.CollectCategoryCode &&
                    x.DepartmentCode == DepartmentCode &&
                    (!NewPagePerStaff || x.StaffCode == StaffCode);
            return OriginalSource.FirstOrDefault(selector);
        }

        private void detail_Format(object sender, EventArgs e)
        {
            DepartmentCode = Convert.ToString(Fields["DepartmentCode"].Value);
            StaffCode = Convert.ToString(Fields["StaffCode"].Value);

            var current = ReportSource[RowIndex];
            var next = RowIndex + 1 < ReportSource.Count ? ReportSource[RowIndex + 1] : null;

            var difference = (next?.RowId.HasValue ?? true);
            lineBottomDetail.LineStyle = difference ? LineStyle.Solid : LineStyle.Dot;
            lineBottomDetail.Width = lineDetailTop.Width - (difference ? 0.0F : txtCategoryName.Left);
            lineBottomDetail.Left = difference ? lineDetailTop.Left : txtCategoryName.Left;

            // current customer
            if (current.RecordType == 0
                && current.RowId.HasValue)
            {
                var sameCustomreCount = ReportSource
                    .Skip(RowIndex).TakeWhile(x => x.CustomerCode == current.CustomerCode).Count();
                if (MaxRowsCount < RowNumber + sameCustomreCount)
                {
                    detail.NewPage = NewPage.Before;
                    RowNumber = 0;
                }
            }
            else
            {
                detail.NewPage = NewPage.None;
            }
            RowIndex++;
            RowNumber++;
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }

    }

}
