using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CompanyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    public partial class PH0101 : VOneScreenBase
    {
        /// <summary>編集中の会社(既存編集時のみセット，新規登録時はnullになる)</summary>
        private Company CurrentCompany { get; set; }

        /// <summary>編集中の会社ロゴ・社判・丸印(既存編集時のみセット，新規登録時はnullになる)</summary>
        private List<CompanyLogo> CurrentCompanyLogos { get; set; }

        /// <summary>編集中の基本設定(既存編集時のみセット，新規登録時はnullになる)</summary>
        private ApplicationControl CurrentApplicationControl { get; set; }

        /// <summary>編集中のパスワード設定(既存編集時のみセット，新規登録時はnullになる)</summary>
        private PasswordPolicy CurrentPasswordPolicy { get; set; }

        private List<LoginUserLicense> CurrentLoginUserLicenses { get; set; }
        #region 初期化
        public PH0101()
        {
            InitializeComponent();
            Text = "各種設定＆セキュリティ";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = RegisterSettings;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearScreen;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = DeleteCompany;

            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = CloseWindow;
        }

        private void PH0101_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();
                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());

                ProgressDialog.Start(BaseForm, Task.WhenAll(loadTask), false, SessionKey);

                if (DataExpression.CompanyCodeTypeGlobal == 0)
                {
                    txtCompanyCode.Format = "9";
                    txtCompanyCode.PaddingChar = '0';
                }
                else
                {
                    txtCompanyCode.Format = "9A";
                }

                InitializeScreen();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeScreen()
        {
            InitializeHandlers();
            InitializeComboBox();
            InitializeGrid();

            ResetScreen();
        }

        private void InitializeHandlers()
        {
            System.Action limitAccessFolderChanged = delegate ()
            {
                txtRootPath.Enabled = rdoLimitAccessFolder1.Checked;
                if (rdoLimitAccessFolder0.Checked)
                    txtRootPath.Clear();
                else
                    txtRootPath.Focus();
            };
            rdoLimitAccessFolder1.CheckedChanged += (sender, e) => limitAccessFolderChanged();

            btnLogoSelect.Click += (sender, e) => SetPicture(sender, e);
            btnSquareSelect.Click += (sender, e) => SetPicture(sender, e);
            btnRoundSelect.Click += (sender, e) => SetPicture(sender, e);

            btnLogoClear.Click += (sender, e) => ClearPicture(sender, e);
            btnSquareClear.Click += (sender, e) => ClearPicture(sender, e);
            btnRoundClear.Click += (sender, e) => ClearPicture(sender, e);
        }

        private void InitializeComboBox()
        {
            // 会社設定
            new[] { cmbBankAccountType1, cmbBankAccountType2, cmbBankAccountType3 }.ForEach(cmb =>
            {
                cmb.Items.Add(new ListItem("", -1));
                cmb.Items.Add(new ListItem("普通預金", 1));
                cmb.Items.Add(new ListItem("当座預金", 2));
                cmb.Items.Add(new ListItem("貯蓄預金", 3));
                cmb.Items.Add(new ListItem("通知預金", 4));
                cmb.TextSubItemIndex = 0;
                cmb.ValueSubItemIndex = 1;
            });

            // 機能設定
            new[] { cmbDepartmentCodeType, cmbAccountTitleCodeType, cmbCustomerCodeType, cmbLoginUserCodeType, cmbSectionCodeType, cmbStaffCodeType }.ForEach(cmb =>
            {
                cmb.Items.Add(new ListItem("数字", 0));
                cmb.Items.Add(new ListItem("英数", 1));
                cmb.TextSubItemIndex = 0;
                cmb.ValueSubItemIndex = 1;
            });
            cmbCustomerCodeType.Items.Add(new ListItem("英数カナ", 2));

            // 権限設定
            cmbMenuAuthorityLevel.Items.Add(new ListItem("2", 2));
            cmbMenuAuthorityLevel.Items.Add(new ListItem("3", 3));
            cmbMenuAuthorityLevel.Items.Add(new ListItem("4", 4));
            cmbMenuAuthorityLevel.TextSubItemIndex = 0;
            cmbMenuAuthorityLevel.ValueSubItemIndex = 1;

            // パスワード設定
        }

        private void InitializeGrid()
        {
            grdMenuAuthority.Template = MakeMenuAuthorityGridTemplate();
            grdFunctionAuthority.Template = MakeFunctionAuthorityGridTemplate();
        }

        private Template MakeMenuAuthorityGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header", cell: builder.GetRowHeaderCell()),
                new CellSetting(height,   0, "MenuId", visible: false, dataField: nameof(MenuAuthorityGridRow.MenuId)),
                new CellSetting(height, 245, "MenuName"      , caption: "画面表題", cell: builder.GetTextBoxCell() , dataField: nameof(MenuAuthorityGridRow.MenuName)),
                new CellSetting(height,  35, "IsLv1Available", caption: "1"       , cell: builder.GetCheckBoxCell(), enabled: false , dataField: nameof(MenuAuthorityGridRow.IsLv1Available)),
                new CellSetting(height,  35, "IsLv2Available", caption: "2"       , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(MenuAuthorityGridRow.IsLv2Available)),
                new CellSetting(height,  35, "IsLv3Available", caption: "3"       , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(MenuAuthorityGridRow.IsLv3Available)),
                new CellSetting(height,  35, "IsLv4Available", caption: "4"       , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(MenuAuthorityGridRow.IsLv4Available)),
            });

            return builder.Build();
        }

        private Template MakeFunctionAuthorityGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"        , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,   0, "FunctionType"  , visible: false, dataField: nameof(FunctionAuthorityGridRow.FunctionType)),
                new CellSetting(height, 175, "FunctionName"  , caption: "セキュリティ権限", cell: builder.GetTextBoxCell()                  , dataField: nameof(FunctionAuthorityGridRow.FunctionName)),
                new CellSetting(height,  35, "IsLv1Available", caption: "1"               , cell: builder.GetCheckBoxCell(), enabled: false , dataField: nameof(FunctionAuthorityGridRow.IsLv1Available)),
                new CellSetting(height,  35, "IsLv2Available", caption: "2"               , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(FunctionAuthorityGridRow.IsLv2Available)),
                new CellSetting(height,  35, "IsLv3Available", caption: "3"               , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(FunctionAuthorityGridRow.IsLv3Available)),
                new CellSetting(height,  35, "IsLv4Available", caption: "4"               , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(FunctionAuthorityGridRow.IsLv4Available)),
                new CellSetting(height,  35, "IsLv5Available", caption: "5"               , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(FunctionAuthorityGridRow.IsLv5Available)),
                new CellSetting(height,  35, "IsLv6Available", caption: "6"               , cell: builder.GetCheckBoxCell(), readOnly: false, dataField: nameof(FunctionAuthorityGridRow.IsLv6Available)),
            });

            builder.Items.Select(cs => cs.CellInstance).OfType<CheckBoxCell>().ForEach(cbx =>
            {
                cbx.TrueValue = true;
                cbx.FalseValue = false;
            });

            return builder.Build();
        }

        #endregion 初期化

        #region 定義

        /// <summary>
        /// 「権限設定」タブ 「メニュー権限」グリッド行データ
        /// </summary>
        private class MenuAuthorityGridRow
        {
            public string MenuId { get; private set; }
            public string MenuName { get; private set; }
            public string MenuCategory { get; private set; }
            public int Sequence { get; private set; }
            public int IsLv1Available { get; } = 1; // 1以外の値を取らない(DBから0を取得しても1に書き換える)
            public int IsLv2Available { get; set; }
            public int IsLv3Available { get; set; }
            public int IsLv4Available { get; set; }

            public MenuAuthorityGridRow(string menuId, string menuName, string menuCategory, int sequence)
            {
                MenuId = menuId;
                MenuName = menuName;
                MenuCategory = menuCategory;
                Sequence = sequence;
            }

            /// <param name="menuAuthorityList">
            /// MenuId, MenuName, MenuCategory, Sequence が同じオブジェクトのリストであること。
            /// </param>
            public MenuAuthorityGridRow(List<MenuAuthority> menuAuthorityList)
            {
                if (menuAuthorityList == null)
                {
                    throw new ArgumentNullException(nameof(menuAuthorityList));
                }
                if (menuAuthorityList.Count == 0)
                {
                    throw new ArgumentException("menuAuthorityList.Count == 0");
                }

                MenuId = menuAuthorityList[0].MenuId;
                MenuName = menuAuthorityList[0].MenuName;
                MenuCategory = menuAuthorityList[0].MenuCategory;
                Sequence = menuAuthorityList[0].Sequence;

                foreach (var auth in menuAuthorityList)
                {
                    if (auth.MenuId != MenuId || auth.MenuName != MenuName || auth.MenuCategory != MenuCategory || auth.Sequence != Sequence)
                    {
                        var ex = new InvalidOperationException($"Invalid MenuAuthority");
                        ex.Data["menuAuthorityList[0].MenuId"] = MenuId;
                        ex.Data["menuAuthorityList[0].MenuName"] = MenuName;
                        ex.Data["menuAuthorityList[0].MenuCategory"] = MenuCategory;
                        ex.Data["menuAuthorityList[0].Sequence"] = Sequence;
                        ex.Data["auth.CompanyId"] = auth.CompanyId;
                        ex.Data["auth.MenuId"] = auth.MenuId;
                        ex.Data["auth.AuthorityLevel"] = auth.AuthorityLevel;
                        ex.Data["auth.MenuName"] = auth.MenuName;
                        ex.Data["auth.MenuCategory"] = auth.MenuCategory;
                        ex.Data["auth.Sequence"] = auth.Sequence;

                        throw ex;
                    }

                    switch (auth.AuthorityLevel)
                    {
                        case 1: break; // 1固定なのでDB取得値を取り込まない
                        case 2: IsLv2Available = auth.Available; break;
                        case 3: IsLv3Available = auth.Available; break;
                        case 4: IsLv4Available = auth.Available; break;

                        default:
                            throw new NotSupportedException("MenuAuthority.AuthorityLevel = {auth.AuthorityLevel}");
                    }
                }
            }

            /// <summary>
            /// 現在のオブジェクトからモデルリストを作成する。
            /// CreateAt, UpdateAt はセットしない。
            /// </summary>
            /// <param name="companyId"></param>
            /// <param name="createBy"></param>
            /// <returns></returns>
            public List<MenuAuthority> MakeModelList(int companyId, int createBy)
            {
                var list = new List<MenuAuthority>
                {
                    new MenuAuthority { AuthorityLevel = 1, Available = IsLv1Available },
                    new MenuAuthority { AuthorityLevel = 2, Available = IsLv2Available },
                    new MenuAuthority { AuthorityLevel = 3, Available = IsLv3Available },
                    new MenuAuthority { AuthorityLevel = 4, Available = IsLv4Available },
                };

                list.ForEach(auth =>
                {
                    auth.CompanyId = companyId;
                    auth.MenuId = MenuId;
                    auth.MenuName = MenuName;
                    auth.MenuCategory = MenuCategory;
                    auth.Sequence = Sequence;
                    auth.CreateBy = createBy;
                    auth.UpdateBy = createBy;
                });

                return list;
            }
        }

        /// <summary>
        /// 「権限設定」タブ 「セキュリティ権限」グリッド行データ
        /// </summary>
        public class FunctionAuthorityGridRow
        {
            public FunctionType FunctionType { get; private set; }
            public bool IsLv1Available { get; } = true; // true以外の値を取らない(DBからfalseを取得してもtrueに書き換える)
            public bool IsLv2Available { get; set; }
            public bool IsLv3Available { get; set; }
            public bool IsLv4Available { get; set; }
            public bool IsLv5Available { get; set; }
            public bool IsLv6Available { get; set; }
            public string FunctionName
            {
                get
                {
                    switch (FunctionType)
                    {
                        case FunctionType.MasterImport:
                            return "マスターインポート";
                        case FunctionType.MasterExport:
                            return "マスターエクスポート";
                        case FunctionType.ModifyBilling:
                            return "請求データ修正・削除";
                        case FunctionType.RecoverBilling:
                            return "請求データ復活";
                        case FunctionType.ModifyReceipt:
                            return "入金データ修正・削除";
                        case FunctionType.RecoverReceipt:
                            return "入金データ復活";
                        case FunctionType.CancelMatching:
                            return "消込解除";
                        default:
                            throw new NotSupportedException($"FunctionAuthority.FunctionType = {FunctionType.ToString()}");
                    }
                }
            }

            public FunctionAuthorityGridRow(FunctionType functionType)
            {
                FunctionType = functionType;
            }

            /// <param name="menuAuthorityList">
            /// FunctionType が同じオブジェクトのリストであること。
            /// </param>
            public FunctionAuthorityGridRow(List<FunctionAuthority> functionAuthorityList)
            {
                if (functionAuthorityList == null)
                {
                    throw new ArgumentNullException(nameof(functionAuthorityList));
                }
                if (functionAuthorityList.Count == 0)
                {
                    throw new ArgumentException("functionAuthorityList.Count == 0");
                }

                FunctionType = functionAuthorityList[0].FunctionType;

                foreach (var auth in functionAuthorityList)
                {
                    if (auth.FunctionType != FunctionType)
                    {
                        var ex = new InvalidOperationException($"Invalid MenuAuthority");
                        ex.Data["functionAuthorityList[0].FunctionType"] = (int)FunctionType;
                        ex.Data["functionAuthorityList.CompanyId"] = auth.CompanyId;
                        ex.Data["functionAuthorityList.MenuId"] = (int)auth.FunctionType;
                        ex.Data["functionAuthorityList.AuthorityLevel"] = auth.AuthorityLevel;

                        throw ex;
                    }

                    switch (auth.AuthorityLevel)
                    {
                        case 1: break; // true固定なのでDB取得値を取り込まない
                        case 2: IsLv2Available = auth.Available; break;
                        case 3: IsLv3Available = auth.Available; break;
                        case 4: IsLv4Available = auth.Available; break;
                        case 5: IsLv5Available = auth.Available; break;
                        case 6: IsLv6Available = auth.Available; break;

                        default:
                            throw new NotSupportedException("FunctionAuthority.AuthorityLevel = {auth.AuthorityLevel}");
                    }
                }
            }

            /// <summary>
            /// 現在のオブジェクトからモデルリストを作成する。
            /// CreateAt, UpdateAt はセットしない。
            /// </summary>
            /// <param name="companyId"></param>
            /// <param name="createBy"></param>
            /// <returns></returns>
            public List<FunctionAuthority> MakeModelList(int companyId, int createBy)
            {
                var list = new List<FunctionAuthority>
                {
                    new FunctionAuthority { AuthorityLevel = 1, Available = IsLv1Available },
                    new FunctionAuthority { AuthorityLevel = 2, Available = IsLv2Available },
                    new FunctionAuthority { AuthorityLevel = 3, Available = IsLv3Available },
                    new FunctionAuthority { AuthorityLevel = 4, Available = IsLv4Available },
                    new FunctionAuthority { AuthorityLevel = 5, Available = IsLv5Available },
                    new FunctionAuthority { AuthorityLevel = 6, Available = IsLv6Available },
                };

                list.ForEach(auth =>
                {
                    auth.CompanyId = companyId;
                    auth.FunctionType = FunctionType;
                    auth.CreateBy = createBy;
                    auth.UpdateBy = createBy;
                });

                return list;
            }
        }

        /// <summary>
        /// 入力値検証エラー情報
        /// </summary>
        private class ValidationError
        {
            public TabPage FocusTargetTabPage { get; private set; }
            public Control FocusTargetControl { get; private set; }
            public string MessageId { get; private set; }
            public string[] MessageArgs { get; private set; }

            public ValidationError(TabPage focusTargetTabPage, Control focusTargetControl, string messageId, params string[] messageArgs)
            {
                FocusTargetTabPage = focusTargetTabPage;
                FocusTargetControl = focusTargetControl;
                MessageId = messageId;
                MessageArgs = messageArgs;
            }
        }

        #endregion 定義

        #region 画面操作関連イベントハンドラ

        #region 共通

        /// <summary>
        /// 入力データを登録する。
        /// </summary>
        [OperationLog("登録")]
        private void RegisterSettings()
        {
            ClearStatusMessage();

            try
            {
                var error = ValidateInput();
                if (error != null)
                {
                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    if (error.FocusTargetTabPage != null)
                        tbcMain.SelectedTab = error.FocusTargetTabPage;
                    if (error.FocusTargetControl != null)
                        error.FocusTargetControl.Focus();
                    return;
                }

                if (!ValidateRootPath())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                    return;

                CompanySource companySource;
                MakeRegisterData(out companySource);

                Debug.Assert( // applicationControlだけ登録時nullが有り得る
                    companySource.Company != null
                    && !string.IsNullOrEmpty(companySource.Company.Code)
                    && companySource.SaveCompanyLogos != null
                    && companySource.DeleteCompanyLogos != null
                    && companySource.MenuAuthorities != null
                    && companySource.MenuAuthorities.Count() != 0
                    && companySource.FunctionAuthorities != null
                    && companySource.FunctionAuthorities.Count() != 0
                    && companySource.PasswordPolicy != null);

                Company registeredCompany = null;

                var task = Task.Run(async () =>
                {
                    registeredCompany = await RegisterCompanySettingsAsync(companySource);
                });
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (registeredCompany == null)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                ResetScreen();
                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSaveError);
                ex.Data["CompanyCode"] = txtCompanyCode.Text;
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// 画面をクリアする。
        /// </summary>
        [OperationLog("クリア")]
        private void ClearScreen()
        {
            ClearStatusMessage();

            try
            {
                ResetScreen();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// タスクを削除する。
        /// </summary>
        [OperationLog("削除")]
        private void DeleteCompany()
        {
            ClearStatusMessage();

            try
            {
                var error = ValidateForDeletion();
                if (error != null)
                {
                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    if (error.FocusTargetTabPage != null)
                        tbcMain.SelectedTab = error.FocusTargetTabPage;
                    if (error.FocusTargetControl != null)
                        error.FocusTargetControl.Focus();
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                    return;

                int? count = null;
                var task = Task.Run(async () =>
                {
                    count = await DeleteCompanySettingsAsync(Login.SessionKey, CurrentCompany.Id);
                });
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (!count.HasValue || count == 0)
                {
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }

                ResetScreen();
                ShowWarningDialog(MsgInfDeleteSuccess);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrDeleteError);
                ex.Data["CompanyCode"] = txtCompanyCode.Text;
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// 画面を閉じる。
        /// </summary>
        [OperationLog("終了")]
        private void CloseWindow()
        {
            BaseForm.Close();
        }

        /// <summary>
        /// Enabled == false に設定されたタブページを選択できないようにする。
        /// </summary>
        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!e.TabPage.Enabled)
                e.Cancel = true;
        }

        #endregion 共通

        #region 会社設定

        private async void txtCompanyCode_Validated(object sender, EventArgs e)
        {
            //  IsNullOrEmpty(companyCode)
            //      会社コードが入力されていない
            //      会社コード欄のEnabled制御しない(新規入力・後で会社コード入力)
            //      且つ、他タブ選択を許可しない(会社コードを入力するまでは会社設定タブのみ編集可能)
            // !IsNullOrEmpty(companyCode) &&  IsNull(CurrentCompany)
            //      入力されている会社コードがDB登録されていない
            //      新規登録モード
            // !IsNullOrEmpty(companyCode) && !IsNull(CurrentCompany)
            //      入力されている会社コードがDB登録されている
            //      既存更新モード

            CurrentCompany = null;
            CurrentCompanyLogos = null;
            CurrentApplicationControl = null;
            CurrentPasswordPolicy = null;
            CurrentLoginUserLicenses = null;
            // MenuAuthorityとFunctionAuthorityはLoadGridData内でデータ取得し上記とは別に管理する

            var companyCode = txtCompanyCode.Text;

            if (string.IsNullOrEmpty(companyCode))
                return;

            // 何らかの会社コードが入力されたら入力値を固定し、他のタブを選択可能にする
            txtCompanyCode.Enabled = false;

            // 入力された会社コードの会社情報を保持する(新規登録時はnullになる)
            CurrentCompany = await GetCompanyAsync(Login.SessionKey, companyCode);

            if (CurrentCompany != null) // 既存データの更新時
            {
                CurrentCompanyLogos = await GetCompanyLogosAsync(Login.SessionKey, CurrentCompany.Id);
                CurrentApplicationControl = await GetApplicationControlAsync(Login.SessionKey, CurrentCompany.Id);
                CurrentPasswordPolicy = await GetPasswordPolicyAsync(Login.SessionKey, CurrentCompany.Id);
                CurrentLoginUserLicenses = Util.GetLoginUserLicenses(Login, CurrentCompany.Id);
                BaseContext.SetFunction03Enabled(true);
                btnAddLicense.Enabled = true;

                ClearStatusMessage(); // 初期表示状態で登録ボタンを押した場合に出るエラーメッセージをクリア
                UpdateScreen();
            }
            else // 新規登録時
            {
                dlgPassWordAnswer frm = new dlgPassWordAnswer();
                frm.ShowDialog();

                if (frm.DialogResult != DialogResult.OK)
                {
                    txtCompanyCode.ResetText();
                    txtCompanyCode.Enabled = true;
                    this.ActiveControl = this.txtCompanyCode;

                    return;
                }
                btnAddLicense.Enabled = false;
                DispStatusMessage(MsgInfSaveNewData, "会社");
            }

            tbpAppControl.Enabled = true;
            tbpAuthority.Enabled = true;
            tbpPasswordPolicy.Enabled = true;

            // ｢機能設定｣タブ内のコンテンツ編集可否制御
            // 本来なら pageAppControl.Enabled を変更することで配下のコントロールが全て制御されるが、
            // この画面はタブを Enabled = false に設定した場合に選択できなくなるよう組んでいるので
            // タブ内の全コントロールを個別に制御する。
            var pageAppControlEnabled = (CurrentCompany == null);
            tbpAppControl.GetAll<Label>().ForEach(c => c.Enabled = pageAppControlEnabled);
            tbpAppControl.GetAll<RadioButton>().ForEach(c => c.Enabled = pageAppControlEnabled);
            tbpAppControl.GetAll<VOneNumberControl>().ForEach(c => c.Enabled = pageAppControlEnabled);
            tbpAppControl.GetAll<VOneComboControl>().ForEach(c => c.Enabled = pageAppControlEnabled);
            nmbLicenseNum.Enabled = false;
            lblLicense.Enabled = (CurrentCompany != null);
            InitializeOption();
            if (CurrentCompany == null) InitializeOptionForNewCompany();
        }
        private void InitializeOption()
        {
            gbxUseLongTermAdvanceReceived.Visible = false;
            gbxRegisterContractInAdvance.Visible = false;
            gbxUseDiscount.Visible = false;
            gbxUseDistribution.Visible = false;
        }
        private void InitializeOptionForNewCompany()
        {
            rdoUseReceiptSection0.Checked = true;
            rdoUseAuthorization0.Checked = true;
            rdoUseScheduledPayment0.Checked = true;
            rdoUseDeclaredAmount0.Checked = true;
            rdoUseLongTermAdvanceReceived0.Checked = true;
            rdoRegisterContractInAdvance0.Checked = true;
            rdoUseCashOnDueDates0.Checked = true;
            rdoUseDiscount0.Checked = true;
            rdoUseForeignCurrency0.Checked = true;
            rdoUseBillingFilter0.Checked = true;
            rdoUseDistribution0.Checked = true;
            rdoUsePublishInvoice0.Checked = true;
            rdoUseReminder0.Checked = true;
            rdoUseHatarakuDBWebApi0.Checked = true;
            rdoUsePCADXWebApi0.Checked = true;
            rdoUseAccountTransfer0.Checked = true;
            rdoUseMFWebApi0.Checked = true;
            rdoUseClosing0.Checked = true;
            rdoUseFactoring0.Checked = true;

            gbxUseDeclareAmount.Enabled = false;
            gbxRegisterContractInAdvance.Enabled = false;
        }

        /// <summary>
        /// 「締日」入力値調整
        /// </summary>
        private void nmbClosingDay_Validated(object sender, EventArgs e)
        {
            var value = nmbClosingDay.Value;
            if (!value.HasValue)
                return;

            if (28 <= value)
                nmbClosingDay.Value = nmbClosingDay.MaxValue;
        }

        /// <summary>
        /// 共有「ロゴ選択」「社判選択」「丸印選択」ボタン押下
        /// </summary>
        private void SetPicture(object sender, EventArgs e)
        {
            var pictureBox = new PictureBox();
            var sizeMode = new PictureBoxSizeMode();

            if (btnLogoSelect.Equals(sender))
            {
                this.ButtonClicked(btnLogoSelect);
                pictureBox = picLogo;
                sizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (btnSquareSelect.Equals(sender))
            {
                this.ButtonClicked(btnSquareSelect);
                pictureBox = picSquareSeal;
                sizeMode = PictureBoxSizeMode.Zoom;
            }
            else if (btnRoundSelect.Equals(sender))
            {
                this.ButtonClicked(btnRoundSelect);
                pictureBox = picRoundSeal;
                sizeMode = PictureBoxSizeMode.Zoom;
            }

            try
            {
                var serverPath = "";
                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    serverPath = Util.GetGeneralSettingServerPathAsync(Login).Result;
                    serverPath = Util.GetDirectoryName(serverPath);
                }), false, SessionKey);

                var fileNames = new List<string>();
                if (!LimitAccessFolder ?
                    !ShowOpenFileDialog(serverPath, out fileNames, filter: "画像ファイル) | *.BMP; *.JPG; *.GIF; *.PNG") :
                    !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectFile)) return;

                pictureBox.Load(fileNames?.FirstOrDefault());
                pictureBox.SizeMode = sizeMode;
            }
            catch (Exception)
            {
                ResetPictureBoxImage(pictureBox);
            }
        }

        /// <summary>
        /// 「ロゴクリア」「社判クリア」「丸印クリア」ボタン押下
        /// </summary>
        private void ClearPicture(object sender, EventArgs e)
        {
            var pictureBox = new PictureBox();
            if (btnLogoClear.Equals(sender))
            {
                this.ButtonClicked(btnLogoClear);
                pictureBox = picLogo;
            }
            else if (btnSquareClear.Equals(sender))
            {
                this.ButtonClicked(btnSquareClear);
                pictureBox = picSquareSeal;
            }
            else if (btnRoundClear.Equals(sender))
            {
                this.ButtonClicked(btnRoundClear);
                pictureBox = picRoundSeal;
            }

            ResetPictureBoxImage(pictureBox);
        }

        #endregion 会社設定

        #region 機能設定
        private void btnAddLicense_Click(object sender, EventArgs e)
        {
            using (var dialog = CreateDialog<dlgLoginUserLicense>())
            {
                dialog.CurrentCompanyId = CurrentCompany.Id;
                dialog.RegisteredLoginUserLicense = CurrentLoginUserLicenses;
                dialog.ProductKey = CurrentCompany.ProductKey;
                dialog.ShowDialog();
                if (dialog.IsUpdated)
                {
                    txtCompanyCode_Validated(txtCompanyCode, new EventArgs());
                }
            }
        }
        private void rdoUseScheduledPayment1_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentCompany != null) return;
            if (rdoUseScheduledPayment1.Checked)
            {
                gbxUseDeclareAmount.Enabled = true;
            }
            else
            {
                gbxUseDeclareAmount.Enabled = false;
                rdoUseDeclaredAmount0.Checked = true;
            }
        }
        private void rdoUseLongTermAdvanceReceived1_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentCompany != null) return;
            if (rdoUseLongTermAdvanceReceived1.Checked)
            {
                gbxRegisterContractInAdvance.Enabled = true;
            }
            else
            {
                gbxRegisterContractInAdvance.Enabled = false;
                rdoRegisterContractInAdvance0.Checked = true;
            }
        }
        private void rdoUseForeignCurrency1_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentCompany != null) return;
            if (rdoUseForeignCurrency1.Checked)
            {
                gbxUseLongTermAdvanceReceived.Enabled = false;
                rdoUseLongTermAdvanceReceived0.Checked = true;

                gbxRegisterContractInAdvance.Enabled = false;
                rdoRegisterContractInAdvance0.Checked = true;

                gbxUseDiscount.Enabled = false;
                rdoUseDiscount0.Checked = true;

                gbxUseAccountTransfer.Enabled = false;
                rdoUseAccountTransfer0.Checked = true;

                gbxUsePublishInvoice.Enabled = false;
                rdoUsePublishInvoice0.Checked = true;

                gbxUseReminder.Enabled = false;
                rdoUseReminder0.Checked = true;

                gbxUseMFWebApi.Enabled = false;
                rdoUseMFWebApi0.Checked = true;

                gbxUseFactoring.Enabled = false;
                rdoUseFactoring0.Checked = true;
            }
            else
            {
                gbxUseLongTermAdvanceReceived.Enabled = true;
                gbxRegisterContractInAdvance.Enabled = true;
                gbxUseDiscount.Enabled = true;
                gbxUseAccountTransfer.Enabled = true;
                gbxUsePublishInvoice.Enabled = true;
                gbxUseReminder.Enabled = true;
                gbxUseMFWebApi.Enabled = true;
                gbxUseFactoring.Enabled = true;
            }
        }

        private void rdoUseMFWebApi1_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentCompany != null) return;
            if (rdoUseMFWebApi1.Checked)
            {
                cmbCustomerCodeType.SelectedValue = 1;
                cmbCustomerCodeType.Enabled = false;
            }
            else
            {
                cmbCustomerCodeType.Enabled = true;
            }
        }
        #endregion

        #region 権限設定

        private void btnCheckMenuAuthorityLevel_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnCheckMenuAuthorityLevel);

            if (cmbMenuAuthorityLevel.SelectedIndex < 0)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "権限レベル");
                return;
            }

            var level = (int)cmbMenuAuthorityLevel.SelectedValue;
            ChangeMenuAuthorityCheckStatus(level, true);
        }

        private void btnUncheckMenuAuthorityLevel_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnUncheckMenuAuthorityLevel);

            if (cmbMenuAuthorityLevel.SelectedIndex < 0)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "権限レベル");
                return;
            }

            var level = (int)cmbMenuAuthorityLevel.SelectedValue;
            ChangeMenuAuthorityCheckStatus(level, false);
        }

        #endregion 権限設定

        #region パスワード設定

        /// <summary>
        /// チェックボックスにチェックされていれば数値入力欄を有効化し、
        /// そうでなければ数値入力欄の内容をクリアして無効化する。
        /// </summary>
        /// <param name="nmb"></param>
        /// <param name="cbx"></param>
        private void ControlNumberBoxByCheckBoxState(VOneNumberControl nmb, CheckBox cbx) // メソッド名
        {
            if (cbx.Checked)
            {
                nmb.Enabled = true;
            }
            else
            {
                nmb.Enabled = false;
                nmb.Value = null;
            }
        }

        /// <summary>
        /// 「アルファベット」チェックボックス
        /// </summary>
        private void cbxPasswordUseAlphabet_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAvailability(cbxPasswordUseAlphabet.Checked, nmbPasswordMinAlphabetUseCount);
            cbxPasswordCaseSensitive.Enabled = CurrentCompany == null;
        }

        /// <summary>
        /// 「数字」チェックボックス
        /// </summary>
        private void cbxPasswordUseNumber_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAvailability(cbxPasswordUseNumber.Checked, nmbPasswordMinNumberUseCount);
        }

        /// <summary>
        /// 「記号」チェックボックス
        /// </summary>
        private void cbxPasswordUseSymbol_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAvailability(cbxPasswordUseSymbol.Checked,
                nmbPasswordMinSymbolUseCount,   // 文字数指定
                cbxPasswordCheckAllSymbols,     // ｢全てにチェックを入れる｣チェックボックス
                cbxPasswordExclamationMark,     // !
                cbxPasswordNumericalSign,       // #
                cbxPasswordPercent,             // %
                cbxPasswordPlusSign,            // +
                cbxPasswordMinusSign,           // -
                cbxPasswordAsterisk,            // *
                cbxPasswordSlash,               // /
                cbxPasswordDollarsSign,         // $
                cbxPasswordUnderscore,          // _
                cbxPasswordTilde,               // ~
                cbxPasswordYenSign,             // \
                cbxPasswordSemicolon,           // ;
                cbxPasswordColon,               // :
                cbxPasswordAtSign,              // @
                cbxPasswordAmpersand,           // &
                cbxPasswordQuestionMark,        // ?
                cbxPasswordCaret                // ^
            );
        }

        /// <summary>
        /// 「全てにチェックを入れる」チェックボックス
        /// </summary>
        private void cbxPasswordCheckAllSymbols_CheckedChanged(object sender, EventArgs e)
        {
            new CheckBox[]
            {
                cbxPasswordExclamationMark, // !
                cbxPasswordNumericalSign,   // #
                cbxPasswordPercent,         // %
                cbxPasswordPlusSign,        // +
                cbxPasswordMinusSign,       // -
                cbxPasswordAsterisk,        // *
                cbxPasswordSlash,           // /
                cbxPasswordDollarsSign,     // $
                cbxPasswordUnderscore,      // _
                cbxPasswordTilde,           // ~
                cbxPasswordYenSign,         // \
                cbxPasswordSemicolon,       // ;
                cbxPasswordColon,           // :
                cbxPasswordAtSign,          // @
                cbxPasswordAmpersand,       // &
                cbxPasswordQuestionMark,    // ?
                cbxPasswordCaret,           // ^
            }.ForEach(cbx => cbx.Checked = cbxPasswordCheckAllSymbols.Checked);
        }

        /// <summary>
        /// 「同じ文字を連続して使用しない」チェックボックス
        /// </summary>
        private void cbxMinSameCharactorRepet_CheckedChanged(object sender, EventArgs e)
        {
            ControlNumberBoxByCheckBoxState(nmbPasswordMinSameCharactorRepeat, cbxPasswordMinSameCharactorRepeat);
        }

        /// <summary>
        /// 「パスワード有効期間」チェックボックス
        /// </summary>
        private void cbxExpirationDays_CheckedChanged(object sender, EventArgs e)
        {
            ControlNumberBoxByCheckBoxState(nmbPasswordExpirationDays, cbxPasswordExpirationDays);
        }

        #endregion パスワード設定

        #endregion 画面操作関連イベントハンドラ

        #region Functions

        /// <summary>
        /// 画面を初期表示状態にリセットする。
        /// </summary>
        private void ResetScreen()
        {
            SuspendLayout();

            try
            {
                // 入力内容クリア
                CurrentCompany = null;
                CurrentCompanyLogos = null;
                CurrentApplicationControl = null;
                CurrentPasswordPolicy = null;
                CurrentLoginUserLicenses = null;
                ResetContents();

                // 選択／入力可否
                txtCompanyCode.Enabled = true;
                tbpAppControl.Enabled = false;
                tbpAuthority.Enabled = false;
                tbpPasswordPolicy.Enabled = false;

                // 初期フォーカス
                tbcMain.SelectedIndex = 0; // 会社情報タブ
                txtCompanyCode.Select();

                // ステータスバー
                ClearStatusMessage();

                // ファンクションキー
                BaseContext.SetFunction03Enabled(false);
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// 全ての入力内容をクリアし、コントロールの有効／無効を初期化する。
        /// 処理対象は入力エリアのみ、タブやファンクションキーは対象外。
        /// </summary>
        private void ResetContents()
        {
            // 先に機械的に全クリア／全有効化
            foreach (var txt in this.GetAll<VOneTextControl>())
            {
                txt.Text = null;
                txt.Enabled = true;
            }
            foreach (var nmb in this.GetAll<VOneNumberControl>())
            {
                var setValue = nmb == nmbDepartmentCodeLength || nmb == nmbAccountTitleCodeLength || nmb == nmbCustomerCodeLength ||
                               nmb == nmbLoginUserCodeLength || nmb == nmbSectionCodeLength || nmb == nmbStaffCodeLength;
                nmb.Value = setValue ? 10 : (int?)null;
                nmb.Enabled = true;
            }
            foreach (var msk in this.GetAll<VOneMaskControl>())
            {
                msk.Text = null;
                msk.Enabled = true;
            }
            foreach (var cmb in this.GetAll<VOneComboControl>())
            {
                var setIndex = cmb == cmbDepartmentCodeType || cmb == cmbAccountTitleCodeType || cmb == cmbCustomerCodeType ||
                               cmb == cmbLoginUserCodeType || cmb == cmbSectionCodeType || cmb == cmbStaffCodeType;
                cmb.SelectedIndex = setIndex ? 0 : -1;
                cmb.Enabled = true;
            }
            foreach (var chk in this.GetAll<CheckBox>())
            {
                chk.Checked = false;
                chk.Enabled = true;
            }
            foreach (var rdo in this.GetAll<RadioButton>().Where(rdo => rdo.Name.EndsWith("0")))
            {
                rdo.Checked = true;
                rdo.Enabled = true;
            }
            foreach (var pic in this.GetAll<PictureBox>())
            {
                ResetPictureBoxImage(pic);
            }

            // 権限設定(リセットではなくリロード)
            LoadGridData();

            // パスワード設定
            cbxPasswordUseAlphabet_CheckedChanged(null, null);
            cbxPasswordUseNumber_CheckedChanged(null, null);
            cbxPasswordUseSymbol_CheckedChanged(null, null);
            if (!cbxPasswordMinSameCharactorRepeat.Checked)
            {
                nmbPasswordMinSameCharactorRepeat.Enabled = false;
            }
            if (!cbxPasswordExpirationDays.Checked)
            {
                nmbPasswordExpirationDays.Enabled = false;
            }
            txtRootPath.Enabled = rdoLimitAccessFolder1.Checked;
        }

        private void ResetPictureBoxImage(PictureBox pic)
        {
            if (pic.Image != null)
            {
                pic.Image.Dispose();
                pic.Image = null;
                pic.ImageLocation = null; // 新たにファイルから読み込んだ事を示すフラグ／画像フォーマット判定に利用しているのでLocationもクリアする。
            }
        }

        /// <summary>
        /// 指定された会社の情報を元にコンテンツをセットする。
        /// ロゴ画像・機能設定・権限設定・パスワード設定など必要なものはサーバより取得する。
        /// </summary>
        private void UpdateScreen()
        {
            SuspendLayout();

            try
            {
                // 会社設定
                var cc = CurrentCompany;
                if (cc == null)
                {
                    ResetContents();
                    return;
                }

                txtProductKey.Enabled = false;

                txtCompanyCode.Text = cc.Code;
                txtProductKey.Text = cc.ProductKey;
                txtCompanyName.Text = cc.Name;
                txtCompanyKana.Text = cc.Kana;
                mskCompanyPostalCode.Text = cc.PostalCode;
                txtCompanyTel.Text = cc.Tel;
                txtCompanyFax.Text = cc.Fax;
                txtCompanyAddress1.Text = cc.Address1;
                txtCompanyAddress2.Text = cc.Address2;

                foreach (var companyLogo in CurrentCompanyLogos)
                {
                    if (companyLogo.Logo.Length <= 0)
                        continue;

                    switch (companyLogo.LogoType)
                    {
                        case (int)CompanyLogoType.Logo:
                            picLogo.Image = ConvertToImage(companyLogo.Logo);
                            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                            picLogo.Refresh();
                            picLogo.ImageLocation = null;
                            break;
                        case (int)CompanyLogoType.SquareSeal:
                            picSquareSeal.Image = ConvertToImage(companyLogo.Logo);
                            picSquareSeal.SizeMode = PictureBoxSizeMode.Zoom;
                            picSquareSeal.Refresh();
                            picSquareSeal.ImageLocation = null;
                            break;
                        case (int)CompanyLogoType.RoundSeal:
                            picRoundSeal.Image = ConvertToImage(companyLogo.Logo);
                            picRoundSeal.SizeMode = PictureBoxSizeMode.Zoom;
                            picRoundSeal.Refresh();
                            picRoundSeal.ImageLocation = null;
                            break;
                    }
                }

                txtBankAccountName.Text = cc.BankAccountName;
                txtBankAccountKana.Text = cc.BankAccountKana;
                txtBankName1.Text = cc.BankName1;
                txtBankName2.Text = cc.BankName2;
                txtBankName3.Text = cc.BankName3;
                txtBankBranchName1.Text = cc.BranchName1;
                txtBankBranchName2.Text = cc.BranchName2;
                txtBankBranchName3.Text = cc.BranchName3;
                var dictionary = cmbBankAccountType1.Items.ToArray() // 預金種別 ComboBox Text => Value
                    .Select(item => new { Text = (string)item.SubItems[0].Value, Value = (int)item.SubItems[1].Value })
                    .ToDictionary(item => item.Text, item => item.Value);
                cmbBankAccountType1.SelectedValue = dictionary.GetValueOrDefault(cc.AccountType1, -1);
                cmbBankAccountType2.SelectedValue = dictionary.GetValueOrDefault(cc.AccountType2, -1);
                cmbBankAccountType3.SelectedValue = dictionary.GetValueOrDefault(cc.AccountType3, -1);
                txtBankAccountNumber1.Text = cc.AccountNumber1;
                txtBankAccountNumber2.Text = cc.AccountNumber2;
                txtBankAccountNumber3.Text = cc.AccountNumber3;

                SetVOneNumberControlValue(nmbClosingDay, cc.ClosingDay);
                nmbClosingDay_Validated(null, null); // 28日以降 => 99日
                var information = UtilClosing.GetClosingInformation(Login.SessionKey, cc.Id);
                nmbClosingDay.Enabled = !(information.UseClosing && information.Closing != null);
                cbxPresetCodeSearchDialog.Checked = cc.PresetCodeSearchDialog == 1;
                cbxShowConfirmDialog.Checked = cc.ShowConfirmDialog == 1;
                cbxShowWarningDialog.Checked = cc.ShowWarningDialog == 1;
                cbxTransferAggregate.Checked = cc.TransferAggregate == 1;
                cbxAutoCloseProgressDialog.Checked = cc.AutoCloseProgressDialog == 1;

                // 機能設定
                var ac = CurrentApplicationControl;
                if (ac != null)
                {
                    rdoUseReceiptSection1.Checked = ac.UseReceiptSection == 1;
                    rdoUseAuthorization1.Checked = ac.UseAuthorization == 1;
                    rdoUseScheduledPayment1.Checked = ac.UseScheduledPayment == 1;
                    rdoUseLongTermAdvanceReceived1.Checked = ac.UseLongTermAdvanceReceived == 1;
                    rdoRegisterContractInAdvance1.Checked = ac.RegisterContractInAdvance == 1;
                    rdoUseCashOnDueDates1.Checked = ac.UseCashOnDueDates == 1;
                    rdoUseDeclaredAmount1.Checked = ac.UseDeclaredAmount == 1;
                    rdoUseDiscount1.Checked = ac.UseDiscount == 1;
                    rdoUseForeignCurrency1.Checked = ac.UseForeignCurrency == 1;
                    rdoUseBillingFilter1.Checked = ac.UseBillingFilter == 1;
                    rdoUseDistribution1.Checked = ac.UseDistribution == 1;
                    rdoUsePublishInvoice1.Checked = ac.UsePublishInvoice == 1;
                    rdoUseReminder1.Checked = ac.UseReminder == 1;
                    rdoUseHatarakuDBWebApi1.Checked = ac.UseHatarakuDBWebApi == 1;
                    rdoUsePCADXWebApi1.Checked = ac.UsePCADXWebApi == 1;
                    rdoUseAccountTransfer1.Checked = ac.UseAccountTransfer == 1;
                    rdoUseMFWebApi1.Checked = ac.UseMFWebApi == 1;
                    rdoUseClosing1.Checked = ac.UseClosing == 1;
                    rdoUseFactoring1.Checked = ac.UseFactoring == 1;

                    rdoLimitAccessFolder1.Checked = ac.LimitAccessFolder == 1;
                    txtRootPath.Text = ac.RootPath;
                    txtRootPath.Enabled = false;
                    btnAddLicense.Enabled = true;
                    lblLicense.Enabled = true;
                    SetVOneNumberControlValue(nmbLicenseNum, CurrentLoginUserLicenses.Count());

                    SetVOneNumberControlValue(nmbDepartmentCodeLength, ac.DepartmentCodeLength);
                    SetVOneNumberControlValue(nmbAccountTitleCodeLength, ac.AccountTitleCodeLength);
                    SetVOneNumberControlValue(nmbCustomerCodeLength, ac.CustomerCodeLength);
                    SetVOneNumberControlValue(nmbLoginUserCodeLength, ac.LoginUserCodeLength);
                    SetVOneNumberControlValue(nmbSectionCodeLength, ac.SectionCodeLength);
                    SetVOneNumberControlValue(nmbStaffCodeLength, ac.StaffCodeLength);

                    cmbDepartmentCodeType.SelectedValue = ac.DepartmentCodeType;
                    cmbAccountTitleCodeType.SelectedValue = ac.AccountTitleCodeType;
                    cmbCustomerCodeType.SelectedValue = ac.CustomerCodeType;
                    cmbLoginUserCodeType.SelectedValue = ac.LoginUserCodeType;
                    cmbSectionCodeType.SelectedValue = ac.SectionCodeType;
                    cmbStaffCodeType.SelectedValue = ac.StaffCodeType;
                }

                // 権限設定
                LoadGridData();

                // パスワード設定
                var pw = CurrentPasswordPolicy;
                if (pw != null)
                {
                    // 統一的な制御はできそうになく逆に複雑になりそうだったので、下記の考えで実装した。
                    // (1) 一度全コントロールの値をDB取得値のままセットする(Min/Max範囲外のものはnull値に変換)。
                    // (2) その後、チェックされていないチェックボックスに関連するコントロールを無効化し入力値をクリアする。

                    var cbxPasswordSymbols = new CheckBox[]
                    {
                        cbxPasswordExclamationMark, // !
                        cbxPasswordNumericalSign,   // #
                        cbxPasswordPercent,         // %
                        cbxPasswordPlusSign,        // +
                        cbxPasswordMinusSign,       // -
                        cbxPasswordAsterisk,        // *
                        cbxPasswordSlash,           // /
                        cbxPasswordDollarsSign,     // $
                        cbxPasswordUnderscore,      // _
                        cbxPasswordTilde,           // ~
                        cbxPasswordYenSign,         // \
                        cbxPasswordSemicolon,       // ;
                        cbxPasswordColon,           // :
                        cbxPasswordAtSign,          // @
                        cbxPasswordAmpersand,       // &
                        cbxPasswordQuestionMark,    // ?
                        cbxPasswordCaret,           // ^
                    };

                    // (1)
                    SetVOneNumberControlValue(nmbPasswordMinLength, pw.MinLength);
                    SetVOneNumberControlValue(nmbPasswordMaxLength, pw.MaxLength);

                    cbxPasswordUseAlphabet.Checked = pw.UseAlphabet == 1;
                    cbxPasswordUseNumber.Checked = pw.UseNumber == 1;
                    cbxPasswordUseSymbol.Checked = pw.UseSymbol == 1;

                    SetVOneNumberControlValue(nmbPasswordMinAlphabetUseCount, pw.MinAlphabetUseCount);
                    SetVOneNumberControlValue(nmbPasswordMinNumberUseCount, pw.MinNumberUseCount);
                    SetVOneNumberControlValue(nmbPasswordMinSymbolUseCount, pw.MinSymbolUseCount);

                    cbxPasswordCaseSensitive.Checked = pw.CaseSensitive == 1;
                    cbxPasswordCaseSensitive.Enabled = false;

                    cbxPasswordExclamationMark.Checked = pw.SymbolType.Contains('!');
                    cbxPasswordNumericalSign.Checked = pw.SymbolType.Contains('#');
                    cbxPasswordPercent.Checked = pw.SymbolType.Contains('%');
                    cbxPasswordPlusSign.Checked = pw.SymbolType.Contains('+');
                    cbxPasswordMinusSign.Checked = pw.SymbolType.Contains('-');
                    cbxPasswordAsterisk.Checked = pw.SymbolType.Contains('*');
                    cbxPasswordSlash.Checked = pw.SymbolType.Contains('/');
                    cbxPasswordDollarsSign.Checked = pw.SymbolType.Contains('$');
                    cbxPasswordUnderscore.Checked = pw.SymbolType.Contains('_');
                    cbxPasswordTilde.Checked = pw.SymbolType.Contains('~');
                    cbxPasswordYenSign.Checked = pw.SymbolType.Contains('\\');
                    cbxPasswordSemicolon.Checked = pw.SymbolType.Contains(';');
                    cbxPasswordColon.Checked = pw.SymbolType.Contains(':');
                    cbxPasswordAtSign.Checked = pw.SymbolType.Contains('@');
                    cbxPasswordAmpersand.Checked = pw.SymbolType.Contains('&');
                    cbxPasswordQuestionMark.Checked = pw.SymbolType.Contains('?');
                    cbxPasswordCaret.Checked = pw.SymbolType.Contains('^');

                    cbxPasswordCheckAllSymbols.Checked = cbxPasswordSymbols.All(cbx => cbx.Checked); // CheckedChangedイベントの発火による影響は気にしなくてよい

                    SetVOneNumberControlValue(nmbPasswordMinSameCharactorRepeat, pw.MinSameCharacterRepeat);
                    SetVOneNumberControlValue(nmbPasswordExpirationDays, pw.ExpirationDays);
                    SetVOneNumberControlValue(nmbPasswordHistoryCount, pw.HistoryCount);

                    cbxPasswordMinSameCharactorRepeat.Checked = nmbPasswordMinSameCharactorRepeat.Value.HasValue;
                    cbxPasswordExpirationDays.Checked = nmbPasswordExpirationDays.Value.HasValue;

                    // (2)
                    ChangeAvailability(cbxPasswordUseAlphabet.Checked, nmbPasswordMinAlphabetUseCount);
                    ChangeAvailability(cbxPasswordUseNumber.Checked, nmbPasswordMinNumberUseCount);
                    ChangeAvailability(cbxPasswordUseSymbol.Checked, cbxPasswordSymbols.Concat(new Control[]
                    {
                        nmbPasswordMinSymbolUseCount,
                        cbxPasswordCheckAllSymbols,
                    }).ToArray());

                    ChangeAvailability(cbxPasswordMinSameCharactorRepeat.Checked, nmbPasswordMinSameCharactorRepeat);
                    ChangeAvailability(cbxPasswordExpirationDays.Checked, nmbPasswordExpirationDays);
                }
            }
            finally
            {
                ResumeLayout();
            }
        }

        private async void LoadGridData()
        {
            if (CurrentCompany == null) // 新規登録時は初期値をセット
            {
                List<MenuAuthority> initialMenuAuthorityList = null;
                var task = Task.Run(async () =>
                {
                    initialMenuAuthorityList = await GetMenuAuthorityListAsync(Login.SessionKey, null, null);
                });
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                grdMenuAuthority.DataSource = initialMenuAuthorityList
                .GroupBy(ma => ma.MenuId)
                .Select(group => new MenuAuthorityGridRow(group.ToList()))
                .ToList();

                var initialFunctionAuthorityList = new List<FunctionAuthorityGridRow>
                {
                    new FunctionAuthorityGridRow(FunctionType.MasterImport),
                    new FunctionAuthorityGridRow(FunctionType.MasterExport),
                    new FunctionAuthorityGridRow(FunctionType.ModifyBilling),
                    new FunctionAuthorityGridRow(FunctionType.RecoverBilling),
                    new FunctionAuthorityGridRow(FunctionType.ModifyReceipt),
                    new FunctionAuthorityGridRow(FunctionType.RecoverReceipt),
                    new FunctionAuthorityGridRow(FunctionType.CancelMatching),
                };
                grdFunctionAuthority.DataSource = initialFunctionAuthorityList;
                return;
            }

            var menuAuthorityList = await GetMenuAuthorityListAsync(Login.SessionKey, CurrentCompany.Id, null);
            grdMenuAuthority.DataSource = menuAuthorityList
                .GroupBy(ma => ma.MenuId)
                .Select(group => new MenuAuthorityGridRow(group.ToList()))
                .ToList();

            var functionAuthorityList = await GetFunctionAuthorityListAsync(Login.SessionKey, CurrentCompany.Id);
            grdFunctionAuthority.DataSource = functionAuthorityList
                .GroupBy(fa => fa.FunctionType)
                .Select(group => new FunctionAuthorityGridRow(group.ToList()))
                .ToList();
        }

        private void ChangeMenuAuthorityCheckStatus(int authLevel, bool isChecked)
        {
            if (authLevel < 2 || 4 < authLevel)
            {
                var ex = new ArgumentOutOfRangeException(nameof(authLevel));
                ex.Data["authLevel"] = authLevel;
                throw ex;
            }

            var rows = (List<MenuAuthorityGridRow>)grdMenuAuthority.DataSource;

            rows.ForEach(row =>
            {
                var available = isChecked ? 1 : 0;

                switch (authLevel)
                {
                    case 2: row.IsLv2Available = available; break;
                    case 3: row.IsLv3Available = available; break;
                    case 4: row.IsLv4Available = available; break;
                }
            });

            grdMenuAuthority.Refresh();
        }

        /// <summary>
        /// コントロールの有効無効を制御する。無効化時はコントロールの入力値をクリアする。
        /// パスワード設定タブの制御に必要なコントロール型(VOneNumberControl, CheckBox)しか処理せず、他はスルーするので注意。
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="controls"></param>
        private void ChangeAvailability(bool enabled, params Control[] controls)
        {
            if (controls == null)
                return;

            foreach (var ctrl in controls)
            {
                ctrl.Enabled = enabled;

                if (enabled) continue;

                // 入力値をクリア
                if (ctrl is VOneNumberControl)
                {
                    ((VOneNumberControl)ctrl).Value = null;
                }
                else if (ctrl is CheckBox)
                {
                    ((CheckBox)ctrl).Checked = false;
                }
                else
                {
                    throw new NotImplementedException($"Unknown Type: {ctrl.GetType().Name}"); // bug 実装漏れ
                }
            }
        }

        #region 登録

        /// <summary>
        /// 入力値検証(登録時)
        /// </summary>
        private ValidationError ValidateInput()
        {
            // 会社設定
            var validatingTabPage = tbpCompany;
            if (CurrentCompany == null) // 新規登録時のみ
            {
                if (string.IsNullOrWhiteSpace(txtCompanyCode.Text))
                    return new ValidationError(validatingTabPage, txtCompanyCode, MsgWngInputRequired, lblCompanyCode.Text);

                if (string.IsNullOrWhiteSpace(txtProductKey.Text))
                    return new ValidationError(validatingTabPage, txtProductKey, MsgWngInputRequired, lblProductKey.Text);

                if (txtProductKey.TextLength != 7)
                    return new ValidationError(validatingTabPage, txtProductKey, MsgWngProductKeyNeed7Character);
            }

            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
                return new ValidationError(validatingTabPage, txtCompanyName, MsgWngInputRequired, lblCompanyName.Text);

            if (string.IsNullOrWhiteSpace(txtCompanyKana.Text))
                return new ValidationError(validatingTabPage, txtCompanyKana, MsgWngInputRequired, lblCompanyKana.Text);

            if (nmbClosingDay.Value == null)
                return new ValidationError(validatingTabPage, nmbClosingDay, MsgWngInputRequired, "締日");

            if (mskCompanyPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length > 0 &&
                mskCompanyPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length != 7)
                return new ValidationError(validatingTabPage, mskCompanyPostalCode, MsgWngInputNeedxxDigits, new string[] { lblCompanyPostalCode.Text,"7桁"});

            // 機能設定
            validatingTabPage = tbpAppControl;
            if (CurrentApplicationControl == null) // 新規登録時のみ
            {
                var nmbTuplesAC1 = new Tuple<VOneNumberControl, string>[] // nmb, 項目名
                {
                    new Tuple<VOneNumberControl, string>(nmbDepartmentCodeLength, "請求部門コード桁数"),
                    new Tuple<VOneNumberControl, string>(nmbAccountTitleCodeLength, "科目コード桁数"),
                    new Tuple<VOneNumberControl, string>(nmbCustomerCodeLength, "得意先コード桁数"),
                    new Tuple<VOneNumberControl, string>(nmbLoginUserCodeLength, "ログインユーザーコード桁数"),
                    new Tuple<VOneNumberControl, string>(nmbSectionCodeLength, "入金部門コード桁数"),
                    new Tuple<VOneNumberControl, string>(nmbStaffCodeLength, "担当者コード桁数"),
                };

                foreach (var tuple in nmbTuplesAC1)
                {
                    var nmb = tuple.Item1;
                    var name = tuple.Item2;

                    if (nmb.Value == null)
                        return new ValidationError(validatingTabPage, nmb, MsgWngInputRequired, name);
                }

                var nmbTuplesAC2 = new Tuple<VOneComboControl, string>[] // cmb, 項目名
                {
                    new Tuple<VOneComboControl, string>(cmbDepartmentCodeType, "請求部門コード文字種"),
                    new Tuple<VOneComboControl, string>(cmbAccountTitleCodeType, "科目コード文字種"),
                    new Tuple<VOneComboControl, string>(cmbCustomerCodeType, "得意先コード文字種"),
                    new Tuple<VOneComboControl, string>(cmbLoginUserCodeType, "ログインユーザーコード文字種"),
                    new Tuple<VOneComboControl, string>(cmbSectionCodeType, "入金部門コード文字種"),
                    new Tuple<VOneComboControl, string>(cmbStaffCodeType, "担当者コード文字種"),
                };

                foreach (var tuple in nmbTuplesAC2)
                {
                    var cmb = tuple.Item1;
                    var name = tuple.Item2;

                    if (cmb.SelectedIndex == -1)
                        return new ValidationError(validatingTabPage, cmb, MsgWngInputRequired, name);
                }

                if (rdoLimitAccessFolder1.Checked && string.IsNullOrWhiteSpace(txtRootPath.Text))
                    return new ValidationError(validatingTabPage, txtRootPath, MsgWngInputRequired, lblRootPath.Text);
            }

            // 権限設定
            validatingTabPage = tbpAuthority;

            // パスワード設定
            validatingTabPage = tbpPasswordPolicy;
            var nmbTuplesPP = new List<Tuple<VOneNumberControl, string>> // nmb, 項目名
            {
                new Tuple<VOneNumberControl, string>(nmbPasswordMinLength, "パスワード最小文字数"),
                new Tuple<VOneNumberControl, string>(nmbPasswordMaxLength, "パスワード最大文字数"),
                new Tuple<VOneNumberControl, string>(nmbPasswordHistoryCount, "パスワード履歴保存数"),
            };
            if (cbxPasswordUseAlphabet.Checked)
                nmbTuplesPP.Add(new Tuple<VOneNumberControl, string>(nmbPasswordMinAlphabetUseCount, "アルファベット文字数"));

            if (cbxPasswordUseNumber.Checked)
                nmbTuplesPP.Add(new Tuple<VOneNumberControl, string>(nmbPasswordMinNumberUseCount, "数字文字数"));

            if (cbxPasswordUseSymbol.Checked)
                nmbTuplesPP.Add(new Tuple<VOneNumberControl, string>(nmbPasswordMinSymbolUseCount, "記号文字数"));

            if (cbxPasswordMinSameCharactorRepeat.Checked)
                nmbTuplesPP.Add(new Tuple<VOneNumberControl, string>(nmbPasswordMinSameCharactorRepeat, "連続使用不可文字数"));

            if (cbxPasswordExpirationDays.Checked)
                nmbTuplesPP.Add(new Tuple<VOneNumberControl, string>(nmbPasswordExpirationDays, "パスワード有効期間日数"));

            foreach (var tuple in nmbTuplesPP)
            {
                var nmb = tuple.Item1;
                var name = tuple.Item2;

                if (nmb.Value == null)
                    return new ValidationError(validatingTabPage, nmb, MsgWngInputRequired, name);
            }

            if (nmbPasswordMaxLength.Value < nmbPasswordMinLength.Value)
                return new ValidationError(validatingTabPage, nmbPasswordMinLength, MsgWngInputRangeChecked, "パスワード文字数");

            if (!cbxPasswordUseAlphabet.Checked && !cbxPasswordUseNumber.Checked && !cbxPasswordUseSymbol.Checked)
                return new ValidationError(validatingTabPage, cbxPasswordUseAlphabet, MsgWngSelectionRequired, "使用する文字の種類");

            var cbxPasswordSymbols = new CheckBox[]
            {
                cbxPasswordExclamationMark, // !
                cbxPasswordNumericalSign,   // #
                cbxPasswordPercent,         // %
                cbxPasswordPlusSign,        // +
                cbxPasswordMinusSign,       // -
                cbxPasswordAsterisk,        // *
                cbxPasswordSlash,           // /
                cbxPasswordDollarsSign,     // $
                cbxPasswordUnderscore,      // _
                cbxPasswordTilde,           // ~
                cbxPasswordYenSign,         // \
                cbxPasswordSemicolon,       // ;
                cbxPasswordColon,           // :
                cbxPasswordAtSign,          // @
                cbxPasswordAmpersand,       // &
                cbxPasswordQuestionMark,    // ?
                cbxPasswordCaret,           // ^
            };
            if (cbxPasswordUseSymbol.Checked && cbxPasswordSymbols.All(cbx => !cbx.Checked))
                return new ValidationError(validatingTabPage, cbxPasswordUseSymbol, MsgWngSelectionRequired, "使用する文字の種類");

            return null;
        }

        private bool ValidateRootPath()
        {
            if (rdoLimitAccessFolder0.Checked) return true;
            var guidValue = Guid.NewGuid();
            var testPath = txtRootPath.Text + "\\" + guidValue.ToString("N") + ".txt";
            try
            {
                using (var writer = new StreamWriter(testPath))
                    writer.Write("Test");
            }
            catch (DirectoryNotFoundException ex)
            {
                ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess, txtRootPath.Text);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            catch (SecurityException ex)
            {
                ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            catch (ArgumentException ex)
            {
                ShowWarningDialog(MsgWngInputInvalidLetter, "ルートフォルダのパス");
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            catch (IOException ex)
            {
                ShowWarningDialog(MsgWngInputInvalidLetter, "ルートフォルダのパス");
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgWngInputInvalidLetter, "ルートフォルダのパス");
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            finally
            {
                try
                {
                    File.Delete(testPath);
                }
                catch { }
            }

            return true;
        }

        /// <summary>
        /// 画面入力値から登録データを作成する。
        /// </summary>
        /// <param name="company">会社設定</param>
        /// <param name="companyLogo">会社設定(会社ロゴ)</param>
        /// <param name="applicationControl">機能設定</param>
        /// <param name="menuAuthority">権限設定(メニュー権限)</param>
        /// <param name="functionAuthority">権限設定(セキュリティ権限)</param>
        /// <param name="passwordPolicy">パスワード設定</param>
        private void MakeRegisterData(out CompanySource companySource)
        {
            companySource = new CompanySource();
            // 共通登録値など ※ CompanyIdはWebサービス側でセットされる
            companySource.Company = (CurrentCompany != null) ?
                CurrentCompany : new Company() { CreateBy = Login.UserId, UpdateBy = Login.UserId };

            companySource.ApplicationControl = null;
            if (CurrentCompany == null) // 新規登録時のみセット ※ このタブのみ、既存更新時にApplicationControlテーブル(UpdateBy, UpdateAt)を更新しないようにするため。
            {
                companySource.ApplicationControl = (CurrentApplicationControl != null) ?
                    CurrentApplicationControl : new ApplicationControl() { CreateBy = Login.UserId, UpdateBy = Login.UserId };
            }

            companySource.PasswordPolicy = (CurrentPasswordPolicy != null) ?
                CurrentPasswordPolicy : new PasswordPolicy(); // CreateBy, UpdateBy なし

            // 会社設定
            companySource.Company.Code = txtCompanyCode.Text;
            companySource.Company.Name = txtCompanyName.Text;
            companySource.Company.Kana = txtCompanyKana.Text;
            companySource.Company.PostalCode = mskCompanyPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length == 0 ?
                                               string.Empty : mskCompanyPostalCode.Text;
            companySource.Company.Address1 = txtCompanyAddress1.Text;
            companySource.Company.Address2 = txtCompanyAddress2.Text;
            companySource.Company.Tel = txtCompanyTel.Text;
            companySource.Company.Fax = txtCompanyFax.Text;
            companySource.Company.ProductKey = txtProductKey.Text;
            companySource.Company.BankAccountName = txtBankAccountName.Text;
            companySource.Company.BankAccountKana = txtBankAccountKana.Text;
            companySource.Company.BankName1 = txtBankName1.Text;
            companySource.Company.BankName2 = txtBankName2.Text;
            companySource.Company.BankName3 = txtBankName3.Text;
            companySource.Company.BranchName1 = txtBankBranchName1.Text;
            companySource.Company.BranchName2 = txtBankBranchName2.Text;
            companySource.Company.BranchName3 = txtBankBranchName3.Text;
            companySource.Company.AccountType1 = cmbBankAccountType1.Text;
            companySource.Company.AccountType2 = cmbBankAccountType2.Text;
            companySource.Company.AccountType3 = cmbBankAccountType3.Text;
            companySource.Company.AccountNumber1 = txtBankAccountNumber1.Text;
            companySource.Company.AccountNumber2 = txtBankAccountNumber2.Text;
            companySource.Company.AccountNumber3 = txtBankAccountNumber3.Text;
            companySource.Company.ClosingDay = (int)nmbClosingDay.Value;
            companySource.Company.ShowConfirmDialog = cbxShowConfirmDialog.Checked ? 1 : 0;
            companySource.Company.PresetCodeSearchDialog = cbxPresetCodeSearchDialog.Checked ? 1 : 0;
            companySource.Company.ShowWarningDialog = cbxShowWarningDialog.Checked ? 1 : 0;
            companySource.Company.TransferAggregate = cbxTransferAggregate.Checked ? 1 : 0;
            companySource.Company.AutoCloseProgressDialog = cbxAutoCloseProgressDialog.Checked ? 1 : 0;

            // 会社ロゴ・社判・丸印
            PrepareLogos(companySource);

            // 機能設定
            if (companySource.ApplicationControl != null)
            {
                companySource.ApplicationControl.DepartmentCodeLength = (int)nmbDepartmentCodeLength.Value;
                companySource.ApplicationControl.DepartmentCodeType =   (int)cmbDepartmentCodeType.SelectedValue;
                companySource.ApplicationControl.SectionCodeLength = (int)nmbSectionCodeLength.Value;
                companySource.ApplicationControl.SectionCodeType =   (int)cmbSectionCodeType.SelectedValue;
                companySource.ApplicationControl.AccountTitleCodeLength = (int)nmbAccountTitleCodeLength.Value;
                companySource.ApplicationControl.AccountTitleCodeType =   (int)cmbAccountTitleCodeType.SelectedValue;
                companySource.ApplicationControl.CustomerCodeLength = (int)nmbCustomerCodeLength.Value;
                companySource.ApplicationControl.CustomerCodeType =   (int)cmbCustomerCodeType.SelectedValue;
                companySource.ApplicationControl.LoginUserCodeLength = (int)nmbLoginUserCodeLength.Value;
                companySource.ApplicationControl.LoginUserCodeType =   (int)cmbLoginUserCodeType.SelectedValue;
                companySource.ApplicationControl.StaffCodeLength = (int)nmbStaffCodeLength.Value;
                companySource.ApplicationControl.StaffCodeType =   (int)cmbStaffCodeType.SelectedValue;

                companySource.ApplicationControl.UseReceiptSection = rdoUseReceiptSection1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseAuthorization = rdoUseAuthorization1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseScheduledPayment = rdoUseScheduledPayment1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseLongTermAdvanceReceived = rdoUseLongTermAdvanceReceived1.Checked ? 1 : 0;
                companySource.ApplicationControl.RegisterContractInAdvance = rdoRegisterContractInAdvance1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseCashOnDueDates = rdoUseCashOnDueDates1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseDeclaredAmount = rdoUseDeclaredAmount1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseDiscount = rdoUseDiscount1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseForeignCurrency = rdoUseForeignCurrency1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseBillingFilter = rdoUseBillingFilter1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseDistribution = rdoUseDistribution1.Checked ? 1 : 0;
                companySource.ApplicationControl.UsePublishInvoice = rdoUsePublishInvoice1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseHatarakuDBWebApi = rdoUseHatarakuDBWebApi1.Checked ? 1 : 0;
                companySource.ApplicationControl.UsePCADXWebApi = rdoUsePCADXWebApi1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseReminder = rdoUseReminder1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseAccountTransfer = rdoUseAccountTransfer1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseMFWebApi = rdoUseMFWebApi1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseClosing = rdoUseClosing1.Checked ? 1 : 0;
                companySource.ApplicationControl.UseFactoring = rdoUseFactoring1.Checked ? 1 : 0;
                companySource.ApplicationControl.LimitAccessFolder = rdoLimitAccessFolder1.Checked ? 1 : 0;
                companySource.ApplicationControl.RootPath = rdoLimitAccessFolder1.Checked ? txtRootPath.Text : string.Empty;
            }

            // 権限設定
            var menuAuthorityGridRowList = (List<MenuAuthorityGridRow>)grdMenuAuthority.DataSource;
            companySource.MenuAuthorities = menuAuthorityGridRowList
                .Select(row => row.MakeModelList(Login.CompanyId, Login.UserId)) // グリッド1行 => AuthorityLevel 1～4 分のレコード
                .SelectMany(list => list).ToArray(); // Flatten(Row1{Lv1,Lv2,Lv3,Lv4}, Row2{Lv1,Lv2,Lv3,Lv4}, ... RowN{Lv1,Lv2,Lv3,Lv4})

            var functionAuthorityGridRowList = (List<FunctionAuthorityGridRow>)grdFunctionAuthority.DataSource;
            companySource.FunctionAuthorities = functionAuthorityGridRowList
                .Select(row => row.MakeModelList(Login.CompanyId, Login.UserId))
                .SelectMany(list => list).ToArray();

            // パスワード設定
            companySource.PasswordPolicy.MinLength = (int)nmbPasswordMinLength.Value;
            companySource.PasswordPolicy.MaxLength = (int)nmbPasswordMaxLength.Value;

            companySource.PasswordPolicy.UseAlphabet = cbxPasswordUseAlphabet.Checked ? 1 : 0;
            companySource.PasswordPolicy.MinAlphabetUseCount = (companySource.PasswordPolicy.UseAlphabet == 1) ?
                (int)nmbPasswordMinAlphabetUseCount.Value : 0;
            companySource.PasswordPolicy.CaseSensitive = cbxPasswordCaseSensitive.Checked ? 1 : 0;

            companySource.PasswordPolicy.UseNumber = cbxPasswordUseNumber.Checked ? 1 : 0;
            companySource.PasswordPolicy.MinNumberUseCount = (companySource.PasswordPolicy.UseNumber == 1) ?
                (int)nmbPasswordMinNumberUseCount.Value : 0;

            companySource.PasswordPolicy.UseSymbol = cbxPasswordUseSymbol.Checked ? 1 : 0;
            if (companySource.PasswordPolicy.UseSymbol == 0)
            {
                companySource.PasswordPolicy.MinSymbolUseCount = 0;
                companySource.PasswordPolicy.SymbolType = "";
            }
            else
            {
                companySource.PasswordPolicy.MinSymbolUseCount = (int)nmbPasswordMinSymbolUseCount.Value;

                var symbols = new StringBuilder();
                if (cbxPasswordExclamationMark.Checked)
                    symbols.Append('!');
                if (cbxPasswordNumericalSign.Checked)
                    symbols.Append('#');
                if (cbxPasswordPercent.Checked)
                    symbols.Append('%');
                if (cbxPasswordPlusSign.Checked)
                    symbols.Append('+');
                if (cbxPasswordMinusSign.Checked)
                    symbols.Append('-');
                if (cbxPasswordAsterisk.Checked)
                    symbols.Append('*');
                if (cbxPasswordSlash.Checked)
                    symbols.Append('/');
                if (cbxPasswordDollarsSign.Checked)
                    symbols.Append('$');
                if (cbxPasswordUnderscore.Checked)
                    symbols.Append('_');
                if (cbxPasswordTilde.Checked)
                    symbols.Append('~');
                if (cbxPasswordYenSign.Checked)
                    symbols.Append('\\');
                if (cbxPasswordSemicolon.Checked)
                    symbols.Append(';');
                if (cbxPasswordColon.Checked)
                    symbols.Append(':');
                if (cbxPasswordAtSign.Checked)
                    symbols.Append('@');
                if (cbxPasswordAmpersand.Checked)
                    symbols.Append('&');
                if (cbxPasswordQuestionMark.Checked)
                    symbols.Append('?');
                if (cbxPasswordCaret.Checked)
                    symbols.Append('^');
                companySource.PasswordPolicy.SymbolType = symbols.ToString();
            }

            companySource.PasswordPolicy.MinSameCharacterRepeat = cbxPasswordMinSameCharactorRepeat.Checked ?
                (int)nmbPasswordMinSameCharactorRepeat.Value : 0;

            companySource.PasswordPolicy.ExpirationDays = cbxPasswordExpirationDays.Checked ?
                (int)nmbPasswordExpirationDays.Value : 0;

            companySource.PasswordPolicy.HistoryCount = (int)nmbPasswordHistoryCount.Value;

            if (Util.GetCompany(Login, companySource.Company.Code) == null)
            {
                companySource.LoginUserLicense = new LoginUserLicense();
                companySource.LoginUserLicense.LicenseKey = VOne.Common.Security.License.GetLicenseKey
                (companySource.Company.ProductKey, 1);
            }

        }

        /// <summary>
        /// ロゴ・社判・丸印の画像ファイルの順番
        /// </summary>
        /// <param name="companySource"></param>
        private void PrepareLogos(CompanySource companySource)
        {
            companySource.SaveCompanyLogos = new List<CompanyLogo>();
            companySource.DeleteCompanyLogos = new List<CompanyLogo>();

            foreach (int logoType in Enum.GetValues(typeof(CompanyLogoType)))
            {
                var pictureBox = new PictureBox();
                if (logoType == (int)CompanyLogoType.Logo)
                    pictureBox = picLogo;
                else if (logoType == (int)CompanyLogoType.SquareSeal)
                    pictureBox = picSquareSeal;
                else if (logoType == (int)CompanyLogoType.RoundSeal)
                    pictureBox = picRoundSeal;

                if (CurrentCompanyLogos != null &&
                    CurrentCompanyLogos.Any(x => x.LogoType == logoType) &&
                    pictureBox.Image == null)
                {
                    var deleteLogo = CurrentCompanyLogos.FirstOrDefault(x => x.LogoType == logoType);
                    companySource.DeleteCompanyLogos.Add(deleteLogo);
                    continue;
                }

                var hasLogo = true;
                var companyLogo = new CompanyLogo();
                companyLogo.Logo = new byte[0];
                var filePath = pictureBox.ImageLocation;

                if (!string.IsNullOrEmpty(filePath))
                {
                    companyLogo.Logo = ChangeImageToBinary(filePath);
                    companyLogo.UpdateAt = DateTime.Now;
                }
                else if (pictureBox.Image != null)
                {
                    var currentLogo = CurrentCompanyLogos.Where(x => x.LogoType == logoType).FirstOrDefault();
                    companyLogo.Logo = currentLogo.Logo;
                    companyLogo.UpdateAt = currentLogo.UpdateAt;
                }
                else
                    hasLogo = false;

                if (hasLogo)
                {
                    companyLogo.CreateBy = Login.UserId;
                    companyLogo.UpdateBy = Login.UserId;
                    companyLogo.LogoType = logoType;
                    companySource.SaveCompanyLogos.Add(companyLogo);
                }
            }
        }

        #endregion 登録

        #region 削除

        /// <summary>
        /// 入力値検証(削除時)
        /// </summary>
        private ValidationError ValidateForDeletion()
        {
            // ログイン中の会社を削除しようとしているか
            if (CurrentCompany.Code == Login.CompanyCode)
                return new ValidationError(null, null, MsgWngCannotDeleteLoginCompany);

            // 会社に得意先が紐付いているか
            bool? customerExists = null;
            var customerTask = Task.Run(async () => {
                customerExists = await ExistCustomerAsync(Login.SessionKey, CurrentCompany.Id);
            });
            ProgressDialog.Start(BaseForm, customerTask, false, Login.SessionKey);
            if (!customerExists.HasValue)
                return new ValidationError(null, null, MsgErrDeleteError);

            if (customerExists.Value)
                return new ValidationError(null, null, MsgWngRegistedDataAndCannotDelete);

            // 会社に入金データが紐付いているか
            bool? receiptExists = null;
            var receiptTask = Task.Run(async () => {
                receiptExists = await ExistReceiptAsync(Login.SessionKey, CurrentCompany.Id);
            });
            ProgressDialog.Start(BaseForm, receiptTask, false, Login.SessionKey);

            if (!receiptExists.HasValue)
                return new ValidationError(null, null, MsgErrDeleteError);

            if (receiptExists.Value)
                return new ValidationError(null, null, MsgWngRegistedDataAndCannotDelete);

            return null;
        }

        #endregion 削除

        #endregion Functions

        #region Web Service

        /// <summary>
        /// 会社設定をデータベースに登録する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="company"></param>
        /// <param name="companyLogo"></param>
        /// <param name="applicationControl">この設定のみ、null時はデータベース登録対象外になる</param>
        /// <param name="menuAuthorities"></param>
        /// <param name="functionAuthorities"></param>
        /// <param name="passwordPolicy"></param>
        /// <returns>Company: 登録された会社，null: エラー</returns>
        private async Task<Company> RegisterCompanySettingsAsync(CompanySource CompanySource)
        {
            CompanyResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CompanyMasterService.CompanyMasterClient>();
                result = await client.CreateAsync(SessionKey, CompanySource);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.Company;
        }

        /// <summary>
        /// 会社に紐付く得意先が存在するかチェックする。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId"></param>
        /// <returns>true: 存在する，false: 存在しない，null: エラー</returns>
        private static async Task<bool?> ExistCustomerAsync(string sessionKey, int companyId)
        {
            ExistResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerMasterService.CustomerMasterClient>();
                result = await client.ExistCompanyAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.Exist;
        }

        /// <summary>
        /// 会社に紐付く入金データが存在するかチェックする。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId"></param>
        /// <returns>true: 存在する，false: 存在しない，null: エラー</returns>
        private static async Task<bool?> ExistReceiptAsync(string sessionKey, int companyId)
        {
            ExistResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<ReceiptService.ReceiptServiceClient>();
                result = await client.ExistCompanyAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.Exist;
        }

        /// <summary>
        /// 会社と会社設定を削除する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId"></param>
        /// <returns>int: 削除件数，null: エラー</returns>
        private static async Task<int?> DeleteCompanySettingsAsync(string sessionKey, int companyId)
        {
            CountResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CompanyMasterService.CompanyMasterClient>();
                result = await client.DeleteAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.Count;
        }

        /// <summary>
        /// 会社コードを指定して会社を取得する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyCode"></param>
        /// <returns>Company: 会社，null: エラー</returns>
        private static async Task<Company> GetCompanyAsync(string sessionKey, string companyCode)
        {
            CompanyResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CompanyMasterService.CompanyMasterClient>();
                result = await client.GetByCodeAsync(sessionKey, companyCode);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.Company;
        }

        /// <summary>
        /// 会社ロゴを取得する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId"></param>
        /// <returns>CompanyLogos: 会社ロゴ，null: エラー</returns>
        private async Task<List<CompanyLogo>> GetCompanyLogosAsync(string sessionKey, int companyId)
            => await ServiceProxyFactory.DoAsync(async (CompanyMasterClient client) =>
            {
                var results = await client.GetLogosAsync(sessionKey, companyId);
                return results.CompanyLogos;
            });

        /// <summary>
        /// アプリケーション設定を取得する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId"></param>
        /// <returns>ApplicationControl: アプリケーション設定，null: エラー</returns>
        private static async Task<ApplicationControl> GetApplicationControlAsync(string sessionKey, int companyId)
        {
            ApplicationControlResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<ApplicationControlMasterService.ApplicationControlMasterClient>();
                result = await client.GetAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.ApplicationControl;
        }

        /// <summary>
        /// メニュー権限を取得する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId">null時は絞り込み条件無しで全データを取得する。</param>
        /// <param name="userId"></param>
        /// <returns>List&lt;MenuAuthority&gt;: メニュー権限，null: エラー</returns>
        private static async Task<List<MenuAuthority>> GetMenuAuthorityListAsync(string sessionKey, int? companyId, int? userId)
        {
            MenuAuthoritiesResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<MenuAuthorityMasterService.MenuAuthorityMasterClient>();
                result = await client.GetItemsAsync(sessionKey, companyId, userId);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.MenuAuthorities;
        }

        /// <summary>
        /// セキュリティ権限を取得する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId"></param>
        /// <returns>List&lt;FunctionAuthority&gt;: セキュリティ権限，null: エラー</returns>
        private static async Task<List<FunctionAuthority>> GetFunctionAuthorityListAsync(string sessionKey, int companyId)
        {
            FunctionAuthoritiesResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<FunctionAuthorityMasterService.FunctionAuthorityMasterClient>();
                result = await client.GetItemsAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.FunctionAuthorities;
        }

        /// <summary>
        /// パスワードポリシーを取得する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="companyId"></param>
        /// <returns>PasswordPolicy: パスワードポリシー，null: エラー</returns>
        private static async Task<PasswordPolicy> GetPasswordPolicyAsync(string sessionKey, int companyId)
        {
            PasswordPolicyResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<PasswordPolicyMasterService.PasswordPolicyMasterClient>();
                result = await client.GetAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
                return null;

            return result.PasswordPolicy;
        }

        #endregion Web Service

        #region Helper

        /// <summary>
        /// Imageオブジェクトをbyte配列に変換する。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format">画像のファイル形式</param>
        /// <returns></returns>
        private static byte[] ChangeImageToBinary(string imagePath)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                var imgBinary = new byte[stream.Length];
                stream.Read(imgBinary, 0, (int)stream.Length);
                return imgBinary;
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }

        /// <summary>
        /// byte配列をImageオブジェクトに変換する。
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        private static Image ConvertToImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using (var stream = new MemoryStream(imageBytes))
            {
                return Image.FromStream(stream);
            }
        }

        /// <summary>
        /// VOneNumberControlに値をセットする。
        /// Mix/Maxチェックを行い、範囲外の場合はデフォルト値をセットする。
        /// </summary>
        /// <param name="nmb"></param>
        /// <param name="value"></param>
        private static void SetVOneNumberControlValue(VOneNumberControl nmb, int value, int? defaultValue = null)
        {
            if (nmb.MinValue <= value && value <= nmb.MaxValue)
                nmb.Value = value;
            else
                nmb.Value = defaultValue;
        }

        #endregion Helper

    }

    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
