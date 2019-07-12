using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.InputControlMasterService;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入力設定</summary>
    public partial class PH9903 : VOneScreenBase
    {
        #region 変数宣言
        public string FormName { get; set; }
        public List<InputControl> InputControlList { get; set; }
        private List<ColumnNameSetting> ColumnNameList { get; set; }
        private int InputGridTypeId { get; set; }

        private const string DiscountName = nameof(Billing.UseDiscount);
        private const string DiscountJpName = "歩引計算";
        private const string ContractName = nameof(Billing.ContractNumber);
        private const string ContractJpName = "契約番号";
        #endregion

        #region Initialize

        public PH9903()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            grid.SetupShortcutKeys();
            InputControlList = new List<InputControl>();
            FormWidth = 350;
            FormHeight = 490;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01")
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if (button.Name == "btnF10")
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    else
                        button.Visible = false;
                }
            };

            grid.CellValueChanged += grid_CellValueChanged;
            grid.CellContentClick += grid_CellContentClick;
            grid.CellContentDoubleClick += grid_CellContentDoubleClick;
            grid.CurrentCellDirtyStateChanged += grid_CurrentCellDirtyStateChanged;
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PH9903_Load;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF10ClickHandler = Exit;
        }
        private void PH9903_Load(object sender, EventArgs e)
        {
            try
            {
                InputGridTypeId = FormName == nameof(PD0301)
                    ? InputGridType.Receipt : InputGridType.Billing;

                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

                InitializeGridTemplate();
                SetGridData();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadColumnNameInfoAsync(),
                Task.Run(async () => InputControlList = await LoadListAsync())
            };

            await Task.WhenAll(tasks);
            foreach (var task in tasks.Where(x => x.Exception != null))
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
        }

        #endregion

        #region event handlers

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            Modified = true;
            grid_CellContentClick(sender, e);
        }

        private void grid_CellContentClick(object sender, CellEventArgs e)
        {
            var numberCellIndex = 2; // 入力順

            if (e.RowIndex >= 0 && e.CellIndex == 0)
                OnCheckedChanged(e.RowIndex, e.CellIndex, numberCellIndex);
        }

        private void grid_CellContentDoubleClick(object sender, CellEventArgs e)
        {
            grid_CellContentClick(sender, e);
        }

        private void OnCheckedChanged(int rowIndex, int cellIndex, int numberCellIndex)
        {
            var checkCell = new CheckBoxCell();
            checkCell = (CheckBoxCell)grid.Rows[rowIndex].Cells[cellIndex];

            var numberCell = new GcNumberCell();
            numberCell = (GcNumberCell)grid.Rows[rowIndex].Cells[numberCellIndex];

            bool checkValue = Convert.ToBoolean(checkCell.EditedFormattedValue);

            if (checkValue)
                numberCell.Enabled = true;
            else
                numberCell.Enabled = false;
        }

        private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            grid.CommitEdit(DataErrorContexts.Commit);
        }

        #endregion

        #region 入力設定リスト取得処理

        private async Task<List<InputControl>> LoadListAsync() =>
            await ServiceProxyFactory.DoAsync(async (InputControlMasterClient client) =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId, Login.UserId, InputGridTypeId);
                if (result.ProcessResult.Result)
                    return result.InputControls;
                return new List<InputControl>();
            });

        #endregion

        #region 項目名称情報取得
        private async Task LoadColumnNameInfoAsync()
        {
            await ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    ColumnNameList = result.ColumnNames;
            });
        }
        #endregion

        #region グリッド初期化処理
        private void InitializeGridTemplate()
        {
            var template = new Template();

            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "TabStop"      , caption: "タブ"),
                new CellSetting(height, 180, "ColumnNameJp" , caption: "設定"),
                new CellSetting(height,  59, "TabIndex"     , caption: "入力順")
            });

            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "TabStop"     , readOnly: false, dataField: nameof(InputControl.TabStop)     , cell: builder.GetCheckBoxCell() ),
                new CellSetting(height, 180, "ColumnNameJp", readOnly: true , dataField: nameof(InputControl.ColumnNameJp)),
                new CellSetting(height,  59, "TabIndex"    , readOnly: false, dataField: nameof(InputControl.ColumnOrder) , cell: builder.GetNumberCellFreeInput(1, 99, 2, false) )
            });
            builder.BuildRowOnly(template);
            grid.Template = template;
            grid.AllowUserToResize = false;
        }
        #endregion
 
        #region グリッドデータ設定処理
        private void SetGridData()
        {
            var checkCell = new CheckBoxCell();
            var textCellName = new GcTextBoxCell();
            var numberCellOrder = new GcNumberCell();
            var checkCellIndex = 0;
            var numberCellIndex = 2;
            var useContractNumber = UseLongTermAdvanceReceived && RegisterContractInAdvance;
            var tableName = InputGridTypeId == InputGridType.Billing ? nameof(Billing) : nameof(Receipt);

            var note1 = "備考1";
            var note2 = "備考2";
            var note3 = "備考3";
            var note4 = "備考4";

            foreach (var names in ColumnNameList.Where(x => x.TableName == tableName))
            {
                if (names.ColumnName == "Note1") note1 = names.DisplayColumnName;
                if (names.ColumnName == "Note2") note2 = names.DisplayColumnName;
                if (names.ColumnName == "Note3") note3 = names.DisplayColumnName;
                if (names.ColumnName == "Note4") note4 = names.DisplayColumnName;
            }

            if (InputControlList.Count > 0)
            {
                foreach (var input in InputControlList)
                {
                    if (input.ColumnName == "Note1") input.ColumnNameJp = note1;
                    if (input.ColumnName == "Note2") input.ColumnNameJp = note2;
                    if (input.ColumnName == "Note3") input.ColumnNameJp = note3;
                    if (input.ColumnName == "Note4") input.ColumnNameJp = note4;
                }

                if (!UseDiscount && InputGridTypeId.Equals(InputGridType.Billing))
                {
                    var discountIndex = InputControlList.FindIndex(
                        x => x.ColumnName.Equals(DiscountName)
                        && x.ColumnNameJp.Equals(DiscountJpName));

                    InputControlList.RemoveAt(discountIndex);
                }

                if (!useContractNumber && InputGridTypeId.Equals(InputGridType.Billing))
                {
                    var contractIndex = InputControlList.FindIndex(
                        x => x.ColumnName.Equals(ContractName)
                        && x.ColumnNameJp.Equals(ContractJpName));

                    InputControlList.RemoveAt(contractIndex);
                }

                grid.RowCount = InputControlList.Count;

                for (var i = 0; i < InputControlList.Count; i++)
                {
                    var inputControl = new InputControl();
                    inputControl = InputControlList[i];

                    checkCell = (CheckBoxCell)grid.Rows[i].Cells[0];
                    checkCell.Value = inputControl.TabStop;

                    textCellName = (GcTextBoxCell)grid.Rows[i].Cells[1];
                    textCellName.Value = inputControl.ColumnNameJp;

                    numberCellOrder = (GcNumberCell)grid.Rows[i].Cells[2];
                    numberCellOrder.Value = inputControl.TabIndex;

                    OnCheckedChanged(i, checkCellIndex, numberCellIndex);
                }
            }
            else
            {
                var columnBillingName = new List<string> {
                    nameof(Billing.SalesAt),
                    nameof(Billing.BillingCategoryId),
                    nameof(Billing.TaxClassId),
                    nameof(Billing.DebitAccountTitleId),
                    nameof(Billing.UseDiscount),
                    nameof(Billing.Note1),
                    nameof(Billing.ContractNumber),
                    nameof(Billing.Quantity),
                    nameof(Billing.UnitSymbol),
                    nameof(Billing.UnitPrice),
                    nameof(Billing.Price),
                    nameof(Billing.TaxAmount),
                    nameof(Billing.BillingAmount),
                };

                var columnBillingNameJp = new List<string>
                {
                    "売上日", "請求区分", "税区", "債権科目", "歩引計算",
                    note1, "契約番号", "数量", "単位", "単価",
                    "金額(抜)", "消費税", "請求額"
                };

                var columnReceiptName = new List<string>
                {
                    "ReceiptCategory", "Note1", "DueAt", "ReceiptAmount", "BillNumber",
                    "BillBankCode", "BillBranchCode", "BillDrawAt", "BillDrawer",
                    "Note2", "Note3", "Note4"
                };

                var columnReceiptNameJp = new List<string>
                {
                    "入金区分", note1, "期日", "金額", "手形番号",
                    "銀行コード", "支店コード", "振出日", "振出人",
                    note2, note3, note4
                };

                if (!UseDiscount && InputGridTypeId == InputGridType.Billing)
                {
                    var nameIndex = columnBillingName.FindIndex(x => x.Equals(DiscountName));
                    var jpNameIndex = columnBillingNameJp.FindIndex(x => x.Equals(DiscountJpName));
                    columnBillingName.RemoveAt(nameIndex);
                    columnBillingNameJp.RemoveAt(jpNameIndex);
                }

                if (!useContractNumber && InputGridTypeId == InputGridType.Billing)
                {
                    var nameIndex = columnBillingName.FindIndex(x => x.Equals(ContractName));
                    var jpNameIndex = columnBillingNameJp.FindIndex(x => x.Equals(ContractJpName));
                    columnBillingName.RemoveAt(nameIndex);
                    columnBillingNameJp.RemoveAt(jpNameIndex);
                }

                var columnName = InputGridTypeId == InputGridType.Billing
                    ? columnBillingName : columnReceiptName;
                var columnNameJp = InputGridTypeId == InputGridType.Billing
                    ? columnBillingNameJp : columnReceiptNameJp;

                grid.RowCount = columnName.Count;

                for (var i = 0; i < grid.RowCount; i++)
                {
                    checkCell = (CheckBoxCell)grid.Rows[i].Cells[0];
                    checkCell.Value = true;

                    textCellName = (GcTextBoxCell)grid.Rows[i].Cells[1];
                    textCellName.Value = columnNameJp[i];

                    numberCellOrder = (GcNumberCell)grid.Rows[i].Cells[2];
                    numberCellOrder.Value = i + 1;

                    OnCheckedChanged(i, checkCellIndex, numberCellIndex);

                    var inputControl = new InputControl();
                    inputControl.CompanyId = CompanyId;
                    inputControl.LoginUserId = Login.UserId;
                    inputControl.InputGridTypeId = InputGridTypeId;
                    inputControl.ColumnName = columnName[i];
                    inputControl.ColumnNameJp = columnNameJp[i];
                    inputControl.ColumnOrder = i + 1;
                    inputControl.TabIndex = i + 1;
                    inputControl.TabStop = 1;
                    InputControlList.Add(inputControl);
                }
            }
        }
        #endregion

        #region 入力情報設定処理
        private void SetInputControlInfo()
        {
            for (var i = 0; i < InputControlList.Count; i++)
            {
                var inputControl = InputControlList[i];

                var checkBoxCell = (CheckBoxCell)grid.Rows[i].Cells[0];
                inputControl.TabStop = Convert.ToInt32(checkBoxCell.Value);

                var numberCell = (GcNumberCell)grid.Rows[i].Cells[2];
                inputControl.TabIndex = Convert.ToInt32(numberCell.Value);
            }

            if (InputGridTypeId != InputGridType.Billing)
                return;

            var discountIndex = InputControlList.FindIndex(
                x => x.ColumnName.Equals(DiscountName)
                && x.ColumnNameJp.Equals(DiscountJpName));

            if (discountIndex < 0
                && InputGridTypeId == InputGridType.Billing)
            {
                var index = InputControlList.FindIndex(
                    x => x.ColumnName.Equals("DebitAccountTitle"));

                InputControlList.Insert(index + 1, new InputControl()
                {
                    CompanyId = CompanyId,
                    LoginUserId = Login.UserId,
                    InputGridTypeId = InputGridType.Billing,
                    ColumnName = DiscountName,
                    ColumnNameJp = DiscountJpName,
                    ColumnOrder = 0,
                    TabStop = 0,
                    TabIndex = 1
                });
            }

            var contractIndex = InputControlList.FindIndex(
                x => x.ColumnName.Equals(ContractName)
                && x.ColumnNameJp.Equals(ContractJpName));

            if (contractIndex < 0
                && InputGridTypeId == InputGridType.Billing)
            {
                var index = InputControlList.FindIndex(
                    x => x.ColumnName.Equals("Note1"));

                InputControlList.Insert(index + 1, new InputControl()
                {
                    CompanyId = Login.CompanyId,
                    LoginUserId = Login.UserId,
                    InputGridTypeId = InputGridType.Billing,
                    ColumnName = ContractName,
                    ColumnNameJp = ContractJpName,
                    ColumnOrder = 0,
                    TabStop = 0,
                    TabIndex = 1
                });
            }

            var order = 1;
            foreach (var inputControl in InputControlList)
            {
                inputControl.ColumnOrder = order;
                order++;
            }
        }
        #endregion

        #region 登録
        [OperationLog("登録")]
        private void Save()
        {
            try
            {

                var inputGridType = InputGridTypeId == InputGridType.Billing
                    ? "請求" : "入金";
                if (!ShowConfirmDialog(MsgQstConfirmClearEditedXXXData, inputGridType))
                    return;

                SetInputControlInfo();

                InputControlsResult result = null;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    result = await ServiceProxyFactory.DoAsync(async (InputControlMasterClient client)
                        => await client.SaveAsync(SessionKey, CompanyId, Login.UserId, InputGridTypeId, InputControlList.ToArray()));
                }), false, SessionKey);

                if (result.ProcessResult.Result)
                {
                    Modified = false;
                    ParentForm.DialogResult = DialogResult.OK;
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 戻る
        [OperationLog("戻る")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ParentForm.DialogResult = DialogResult.Cancel;
        }
        #endregion

    }
}
