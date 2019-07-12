using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>個別消込・歩引入力</summary>
    public partial class PE0105 : VOneScreenBase
    {
        public Billing[] billingInfo { get; set; }
        private Color row0BackColor;
        private Color row1BackColor;
        private int Precision { get; set; } = 0;
        private List<decimal[]> Discountlist { get; set; }

        public PE0105()
        {
            InitializeComponent();
            grdDiscoutChousei.SetupShortcutKeys();
            Text = "歩引額調整";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
        }

        private void InitializeNumberControlSetting()
        {
            if (billingInfo != null && billingInfo.Count() > 0) Precision = billingInfo[0].CurrencyPrecision;
            nmbBillingAmt.DisplayFields.Clear();
            nmbRemainAmt.DisplayFields.Clear();
            nmbTargetAmt.DisplayFields.Clear();
            nmbDiscountTotal.DisplayFields.Clear();
            nmbDiscountAmount1.DisplayFields.Clear();
            nmbDiscountAmount2.DisplayFields.Clear();
            nmbDiscountAmount3.DisplayFields.Clear();
            nmbDiscountAmount4.DisplayFields.Clear();
            nmbDiscountAmount5.DisplayFields.Clear();
            nmbBillingAmt.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbRemainAmt.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbTargetAmt.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbDiscountTotal.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbDiscountAmount1.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbDiscountAmount2.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbDiscountAmount3.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbDiscountAmount4.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbDiscountAmount5.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
        }
        private void PE0105_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var tasks = new List<Task> {
                    LoadApplicationControlAsync(),
                    LoadCompanyAsync(),
                    LoadControlColorAsync(),
                };
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                InitializeNumberControlSetting();
                InitializeGridSetting();
                calculateTotal();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeGridSetting()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var rowHeight = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight, 40, "Header"         , cell: builder.GetRowHeaderCell(), sortable: true ),
                new CellSetting(rowHeight, 86, "CustomerCode"   , dataField: "CustomerCode"   , caption: "得意先コード", cell: builder.GetTextBoxCell()        ),
                new CellSetting(rowHeight, 86, "CustomerName"   , dataField: "CustomerName"   , caption: "得意先名"    , cell: builder.GetTextBoxCell()        ),
                new CellSetting(rowHeight, 86, "BilledAt"       , dataField: "BilledAt"       , caption: "請求日"      , cell: builder.GetDateCell_yyyyMMdd()  ),
                new CellSetting(rowHeight, 86, "DueAt"          , dataField: "DueAt"          , caption: "予定日"      , cell: builder.GetDateCell_yyyyMMdd()  ),
                new CellSetting(rowHeight, 86, "BillingAmount"  , dataField: "BillingAmount"  , caption: "請求額"      , cell: builder.GetNumberCellCurrency(Precision, Precision, 0)             ),
                new CellSetting(rowHeight, 86, "RemainAmount"   , dataField: "RemainAmount"   , caption: "請求残"      , cell: builder.GetNumberCellCurrency(Precision, Precision, 0)             ),
                new CellSetting(rowHeight, 75, "DiscountAmount1", dataField: "DiscountAmount1", caption: "歩引額1"     , cell: builder.GetNumberCellCurrencyInput(Precision, Precision, 0), readOnly: false),
                new CellSetting(rowHeight, 75, "DiscountAmount2", dataField: "DiscountAmount2", caption: "歩引額2"     , cell: builder.GetNumberCellCurrencyInput(Precision, Precision, 0), readOnly: false),
                new CellSetting(rowHeight, 75, "DiscountAmount3", dataField: "DiscountAmount3", caption: "歩引額3"     , cell: builder.GetNumberCellCurrencyInput(Precision, Precision, 0), readOnly: false),
                new CellSetting(rowHeight, 75, "DiscountAmount4", dataField: "DiscountAmount4", caption: "歩引額4"     , cell: builder.GetNumberCellCurrencyInput(Precision, Precision, 0), readOnly: false),
                new CellSetting(rowHeight, 75, "DiscountAmount5", dataField: "DiscountAmount5", caption: "歩引額5"     , cell: builder.GetNumberCellCurrencyInput(Precision, Precision, 0), readOnly: false),
                new CellSetting(rowHeight, 60, "InvoiceCode"    , dataField: "InvoiceCode"    , caption: "請求書番号"  ,  cell: builder.GetTextBoxCell()),
                new CellSetting(rowHeight, 80, "DepartmentName" , dataField: "DepartmentName" , caption: "請求部門名"  ,  cell: builder.GetTextBoxCell()),
                new CellSetting(rowHeight,  0, "Id"             , dataField:"Id" ),
            });

            grdDiscoutChousei.Template = builder.Build();

            if (Discountlist == null) Discountlist = new List<decimal[]>();

            List<Billing> BillingList = new List<Billing>();
            BillingList = billingInfo.ToList();
            for (int i = 0; i < billingInfo.Count(); i++)
            {
                decimal discountAmt1 = billingInfo[i].DiscountAmount1;
                decimal discountAmt2 = billingInfo[i].DiscountAmount2;
                decimal discountAmt3 = billingInfo[i].DiscountAmount3;
                decimal discountAmt4 = billingInfo[i].DiscountAmount4;
                decimal discountAmt5 = billingInfo[i].DiscountAmount5;
                Discountlist.Add(new[] { discountAmt1, discountAmt2, discountAmt3, discountAmt4, discountAmt5 });
            }

            if (billingInfo != null)
            {
                Invoke(new System.Action(() =>
                {
                    grdDiscoutChousei.DataSource = new BindingSource(BillingList, null);
                    grdDiscoutChousei.CurrentCellBorderLine = new Line(LineStyle.None, Color.Empty);
                    grdDiscoutChousei.CurrentRowBorderLine = new Line(LineStyle.None, Color.Empty);
                }));
            }
            if (grdDiscoutChousei.RowCount > 1)
            {
                row1BackColor = grdDiscoutChousei.Rows[0].InheritedStyle.BackColor;
                row0BackColor = grdDiscoutChousei.Rows[1].InheritedStyle.BackColor;
            }
        }

        private string GetNumberFormat()
        {
            string displayFormatString = "0";
            var displayFieldString = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < Precision; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }
            return displayFieldString;
        }
        private void calculateTotal()
        {
            decimal billingAmount = 0M;
            decimal remainAmount = 0M;
            decimal discountAmount1 = 0M;
            decimal discountAmount2 = 0M;
            decimal discountAmount3 = 0M;
            decimal discountAmount4 = 0M;
            decimal discountAmount5 = 0M;
            decimal discountTotal = 0M;
            foreach (Row row in grdDiscoutChousei.Rows)
            {
                billingAmount += Convert.ToDecimal(row.Cells["celBillingAmount"].Value);
                remainAmount += Convert.ToDecimal(row.Cells["celRemainAmount"].Value);
                discountAmount1 += Convert.ToDecimal(row.Cells["celDiscountAmount1"].Value);
                discountAmount2 += Convert.ToDecimal(row.Cells["celDiscountAmount2"].Value);
                discountAmount3 += Convert.ToDecimal(row.Cells["celDiscountAmount3"].Value);
                discountAmount4 += Convert.ToDecimal(row.Cells["celDiscountAmount4"].Value);
                discountAmount5 += Convert.ToDecimal(row.Cells["celDiscountAmount5"].Value);
                discountTotal += discountAmount1 + discountAmount2 + discountAmount3 + discountAmount4 + discountAmount5;
            }
            nmbBillingAmt.Value = billingAmount;
            nmbRemainAmt.Value = remainAmount;
            nmbDiscountAmount1.Value = discountAmount1;
            nmbDiscountAmount2.Value = discountAmount2;
            nmbDiscountAmount3.Value = discountAmount3;
            nmbDiscountAmount4.Value = discountAmount4;
            nmbDiscountAmount5.Value = discountAmount5;
            nmbDiscountTotal.Value = discountTotal;
            nmbTargetAmt.Value = remainAmount - discountTotal;
        }

        private void grdDiscoutChousei_CellValidated(object sender, CellEventArgs e)
        {
           if (e.CellName.ToString() == "celDiscountAmount1"
                || e.CellName.ToString() == "celDiscountAmount2"
                || e.CellName.ToString() == "celDiscountAmount3"
                || e.CellName.ToString() == "celDiscountAmount4"
                || e.CellName.ToString() == "celDiscountAmount5")
            {
                CellStyle style = new CellStyle();
                decimal newDiscount = 0M;
                decimal oldDiscount = 0M;
                int columnsCount = this.grdDiscoutChousei.Columns.Count;
                newDiscount = Convert.ToDecimal(grdDiscoutChousei.Rows[e.RowIndex].Cells[e.CellIndex].Value);
                string cellName = grdDiscoutChousei.Rows[e.RowIndex].Cells[e.CellIndex].Name.ToString();

                int discountType = 0;
                if (cellName == "celDiscountAmount1")
                {
                    discountType = 0;
                }
                else if (cellName == "celDiscountAmount2")
                {
                    discountType = 1;
                }
                else if (cellName == "celDiscountAmount3")
                {
                    discountType = 2;
                }
                else if (cellName == "celDiscountAmount4")
                {
                    discountType = 3;
                }
                else
                {
                    discountType = 4;
                }
                oldDiscount = Discountlist[e.RowIndex][discountType];

                if (newDiscount != oldDiscount)
                {
                    style.BackColor = System.Drawing.Color.LightCyan;
                    grdDiscoutChousei.Rows[e.RowIndex].Cells[e.CellIndex].Style.ApplyStyle(style);
                }
                else
                {
                    ResetGridBackColor(e.RowIndex,e.CellIndex);
                }
                calculateTotal();
                ClearStatusMessage();
            }
        }

        private void ResetGridBackColor(int row,int cel)
        {
            CellStyle style = new CellStyle();
            int columns_count = this.grdDiscoutChousei.Columns.Count;
            int mod = row % 2;
            if (mod != 0)
            {
                style.BackColor = row0BackColor;
            }
            else
            {
                style.BackColor = row1BackColor;
            }

            grdDiscoutChousei.Rows[row][cel].Style.ApplyStyle(style);

        }

        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!checkUpdate())
                {
                    ShowWarningDialog(MsgWngNoDataForProcess, "登録");
                    return;
                }
                foreach (Row row in grdDiscoutChousei.Rows)
                {
                    int check_zero = 0;
                    int billing_id = Convert.ToInt32(row.Cells["celId"].Value);

                    if (Convert.ToDecimal(row.Cells["celDiscountAmount1"].Value) != 0M
                        && row.Cells["celDiscountAmount1"].Value.ToString() != "")
                    {
                        check_zero = 1;
                    }
                    else if (Convert.ToDecimal(row.Cells["celDiscountAmount2"].Value) != 0M
                        && row.Cells["celDiscountAmount2"].Value.ToString() != "")
                    {
                        check_zero = 1;
                    }
                    else if (Convert.ToDecimal(row.Cells["celDiscountAmount3"].Value) != 0M
                      && row.Cells["celDiscountAmount3"].Value.ToString() != "")
                    {
                        check_zero = 1;
                    }
                    else if (Convert.ToDecimal(row.Cells["celDiscountAmount4"].Value) != 0M
                       && row.Cells["celDiscountAmount4"].Value.ToString() != "")
                    {
                        check_zero = 1;
                    }
                    else if (Convert.ToDecimal(row.Cells["celDiscountAmount5"].Value) != 0M
                          && row.Cells["celDiscountAmount5"].Value.ToString() != "")
                    {
                        check_zero = 1;
                    }

                    if (check_zero == 0)
                    {
                        DeleteDiscounts(billing_id);
                    }
                    else
                    {
                        BillingDiscount bd = new BillingDiscount();
                        bd.AssignmentFlag = 0;
                        bd.BillingId = billing_id;
                        bd.DiscountAmount1 = Convert.ToDecimal(row.Cells["celDiscountAmount1"].Value);
                        bd.DiscountAmount2 = Convert.ToDecimal(row.Cells["celDiscountAmount2"].Value);
                        bd.DiscountAmount3 = Convert.ToDecimal(row.Cells["celDiscountAmount3"].Value);
                        bd.DiscountAmount4 = Convert.ToDecimal(row.Cells["celDiscountAmount4"].Value);
                        bd.DiscountAmount5 = Convert.ToDecimal(row.Cells["celDiscountAmount5"].Value);
                        SaveDiscount(bd);
                    }

                    ParentForm.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();

            if (checkUpdate() && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            ReloadDiscount();
        }

        private void DeleteDiscounts(long billing_id)
        {
           var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var billingService = factory.Create<BillingServiceClient>();
                CountResult deleteResult = await billingService.DeleteDiscountAsync(SessionKey, billing_id);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }

        private void SaveDiscount(BillingDiscount bDiscount)
        {
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var billingService = factory.Create<BillingServiceClient>();
                CountResult saveResult = await billingService.SaveDiscountAsync(SessionKey, bDiscount);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }
        private bool checkUpdate()
        {
            foreach (Row row in grdDiscoutChousei.Rows)
            {
                if ((Convert.ToDecimal(row.Cells["celDiscountAmount1"].Value) != Discountlist[row.Index][0])) return true;
                if ((Convert.ToDecimal(row.Cells["celDiscountAmount2"].Value) != Discountlist[row.Index][1])) return true;
                if ((Convert.ToDecimal(row.Cells["celDiscountAmount3"].Value) != Discountlist[row.Index][2])) return true;
                if ((Convert.ToDecimal(row.Cells["celDiscountAmount4"].Value) != Discountlist[row.Index][3])) return true;
                if ((Convert.ToDecimal(row.Cells["celDiscountAmount5"].Value) != Discountlist[row.Index][4])) return true;
            }
            return false;
        }

        private void ReloadDiscount() {

            DataTable dt = createDiscountTable();
            DataTable dtempty = dt.Clone();

            foreach (Row row in grdDiscoutChousei.Rows)
            {
                DataRow dr = CreateDataRowTemplate(dt, row);
                dt.Rows.Add(dr);
            }
            grdDiscoutChousei.DataSource = null;
            grdDiscoutChousei.DataSource = dt;
            calculateTotal();
        }

        private DataTable createDiscountTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerCode");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("BilledAt");
            dt.Columns.Add("DueAt");
            dt.Columns.Add("BillingAmount");
            dt.Columns.Add("RemainAmount");
            dt.Columns.Add("DiscountAmount1");
            dt.Columns.Add("DiscountAmount2");
            dt.Columns.Add("DiscountAmount3");
            dt.Columns.Add("DiscountAmount4");
            dt.Columns.Add("DiscountAmount5");
            dt.Columns.Add("InvoiceCode");
            // dt.Columns.Add("InvoiceCode"); category
            dt.Columns.Add("DepartmentName");
            dt.Columns.Add("Id");
            return dt;
        }

        private DataRow CreateDataRowTemplate(DataTable dt, Row row)
        {
            DataRow dr = dt.NewRow();
            dr["CustomerCode"] = row.Cells["celCustomerCode"].Value;
            dr["CustomerName"] = row.Cells["celCustomerName"].Value;
            dr["BilledAt"] = row.Cells["celBilledAt"].Value;
            dr["DueAt"] = row.Cells["celDueAt"].Value;
            dr["BillingAmount"] = row.Cells["celBillingAmount"].Value;
            dr["RemainAmount"] = row.Cells["celRemainAmount"].Value;
            dr["DiscountAmount1"] = Discountlist[row.Index][0];
            dr["DiscountAmount2"] = Discountlist[row.Index][1];
            dr["DiscountAmount3"] = Discountlist[row.Index][2];
            dr["DiscountAmount4"] = Discountlist[row.Index][3];
            dr["DiscountAmount5"] = Discountlist[row.Index][4];
            dr["InvoiceCode"] = row.Cells["celInvoiceCode"].Value;
            dr["DepartmentName"] = row.Cells["celDepartmentName"].Value;
            // dr["DepartmentName"] = row.Cells["DepartmentName"].Value; category
            dr["Id"] = row.Cells["celId"].Value;
            return dr;
        }
    }
}
