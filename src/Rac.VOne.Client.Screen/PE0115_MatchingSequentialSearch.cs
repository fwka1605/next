using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using GrapeCity.Win.MultiRow;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    public partial class PE0115 : VOneScreenBase
    {
        public GcMultiRow grid { get; set; }

        /// <summary>一括消込 検索</summary>
        public PE0115()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "一括消込 検索";
            FormWidth = 460;
            FormHeight = 200;
            FunctionKeysSetter = buttons => {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF10")
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    else
                        button.Visible = false;
                }
            };
        }

        private void InitializeHandlers()
        {
            btnCustomerCode.Click += (sdr, ev) =>
            {
                this.ButtonClicked(btnCustomerCode, "検索（得意先コード）");
                SearchText(eSearchKind.CustomerCode, txtCustomerCode.Text);
            };
            btnCustomerName.Click += (sdr, ev) =>
            {
                this.ButtonClicked(btnCustomerName, "検索（得意先名）");
                SearchText(eSearchKind.CustomerName, txtCustomerName.Text);
            };
            btnPayerName.Click += (sdr, ev) =>
            {
                this.ButtonClicked(btnPayerName, "検索（振込依頼人名）");
                SearchText(eSearchKind.PayerName, txtPayerName.Text);
            };
            txtPayerName.Validated += (sdr, ev) =>
            {
                txtPayerName.Text = EbDataHelper.ConvertToValidEbKana(txtPayerName.Text.Trim());
            };

            btnSearchCustomerCode.Click += btnCusCode_Click;

        }

        private void btnCusCode_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            this.ButtonClicked(btnSearchCustomerCode);
            ClearStatusMessage();
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer == null) return;
            txtCustomerCode.Text = customer.Code;
            txtCustomerName.Text = customer.Name;
        }



        private void PE0115_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, Task.WhenAll(
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync()), false, SessionKey);

            var expression = new DataExpression(ApplicationControl);
            txtCustomerCode.MaxLength = expression.CustomerCodeLength;
            txtCustomerCode.Format = expression.CustomerCodeFormatString;
            txtCustomerCode.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;


        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Close;
        }

        [OperationLog("戻る")]
        private void Close()
        {
            ParentForm.Close();
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PE0115_Load;
        }

        private enum eSearchKind
        {
            CustomerCode,
            CustomerName,
            PayerName
        }

        private void SearchText(eSearchKind kind, string text)
        {
            if (grid == null || grid.Rows.Count == 0) return;
            if (string.IsNullOrEmpty(text)) return;
            var targetCellIndex = 0;
            if (kind == eSearchKind.CustomerCode) targetCellIndex = grid.Columns["celDispCustomerCode"].Index;
            if (kind == eSearchKind.CustomerName) targetCellIndex = grid.Columns["celDispCustomerName"].Index;
            if (kind == eSearchKind.PayerName   ) targetCellIndex = grid.Columns["celPayerName"].Index;

            var startRowIndex = (grid.CurrentCellPosition.CellIndex != targetCellIndex) ? grid.CurrentCellPosition.RowIndex : grid.CurrentCellPosition.RowIndex + 1;
            var p = new CellPosition(-1, -1);
            if (startRowIndex < grid.Rows.Count)
            {
                p = grid.Search(text, new CellPosition(startRowIndex, targetCellIndex), new CellPosition(grid.Rows.Count - 1, targetCellIndex), false, SearchFlags.None, SearchOrder.NOrder);
            }
            if (p.RowIndex < 0)
            {
                p = grid.Search(text, new CellPosition(0, targetCellIndex), new CellPosition(startRowIndex, targetCellIndex), false, SearchFlags.None, SearchOrder.NOrder);
            }
            if (p.RowIndex < 0)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
            else
            {
                ClearStatusMessage();
                grid.CurrentCellPosition = p;
            }

        }


    }
}
