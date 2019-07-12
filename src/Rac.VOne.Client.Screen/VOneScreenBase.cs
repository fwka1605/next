using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Screen.ApplicationControlMasterService;
using Rac.VOne.Client.Screen.CompanyMasterService;
using Rac.VOne.Client.Screen.ControlColorMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.Extensions;
using Rac.VOne.Client.Screen.FunctionAuthorityMasterService;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    public partial class VOneScreenBase :
        UserControl,
        IApplicationSetter,
        IFunctionKeysSetter,
        IMessageSetter,
        IScreenColors,
        IScreenNameSetter,
        ILoggable,
        IApplicationUsable,
        IClosingMonthSetter
    {

        /// <summary>入金部門利用</summary>
        protected bool UseSection { get { return ApplicationControl?.UseReceiptSection == 1; } }

        /// <summary>承認機能利用</summary>
        protected bool UseAuthorization { get { return ApplicationControl?.UseAuthorization == 1; } }
        /// <summary>外貨利用</summary>
        protected bool UseForeignCurrency { get { return ApplicationControl?.UseForeignCurrency == 1; } }
        /// <summary>入金予定入力利用 </summary>
        protected bool UseScheduledPayment { get { return ApplicationControl?.UseScheduledPayment == 1; } }
        /// <summary>入金予定入力 - 予定額を消込対象額に使用</summary>
        protected bool UseDeclaredAmount { get { return ApplicationControl?.UseDeclaredAmount == 1; } }
        /// <summary>期日現金利用 </summary>
        protected bool UseCashOnDueDates { get { return ApplicationControl?.UseCashOnDueDates == 1; } }

        /// <summary>配信機能利用</summary>
        protected bool UseDistribution { get { return ApplicationControl?.UseDistribution == 1; } }

        /// <summary>歩引利用</summary>
        protected bool UseDiscount { get { return ApplicationControl?.UseDiscount == 1; } }

        /// <summary>請求絞込 利用</summary>
        protected bool UseBillingFilter { get { return ApplicationControl?.UseBillingFilter == 1; } }

        /// <summary>長期前受管理 利用</summary>
        protected bool UseLongTermAdvanceReceived { get { return ApplicationControl?.UseLongTermAdvanceReceived == 1; } }

        /// <summary>長期前受管理 契約番号の事前登録を行う</summary>
        protected bool RegisterContractInAdvance { get { return ApplicationControl?.RegisterContractInAdvance == 1; } }

        /// <summary>フォルダ選択の制限</summary>
        protected bool LimitAccessFolder { get { return ApplicationControl?.LimitAccessFolder == 1; } }
        /// <summary>請求書発行機能利用</summary>
        protected bool UsePublishInvoice { get { return ApplicationControl?.UsePublishInvoice == 1; } }
        /// <summary>働くDB WebAPI 利用</summary>
        protected bool UseHatarakuDBWebApi { get { return ApplicationControl?.UseHatarakuDBWebApi == 1; } }
        /// <summary>PCA会計DX WebAPI利用</summary>
        protected bool UsePCADXWebApi { get { return ApplicationControl?.UsePCADXWebApi == 1; } }
        /// <summary>督促管理利用</summary>
        protected bool UseReminder { get { return ApplicationControl?.UseReminder == 1; } }
        /// <summary>督促管理利用</summary>
        protected bool UseAccountTransfer { get { return ApplicationControl?.UseAccountTransfer == 1; } }
        /// <summary> MFクラウド請求書 WebAPI利用 </summary>
        protected bool UseMFWebApi { get { return ApplicationControl?.UseMFWebApi == 1; } }
        /// <summary> 締め処理利用 </summary>

        protected bool UseClosing { get { return ApplicationControl?.UseClosing == 1; } }
        /// <summary> ファクタリング対応 </summary>

        protected bool UseFactoring { get { return ApplicationControl?.UseFactoring == 1; } }


        protected Form BaseForm { get { return TopLevelControl as Form; } }

        public new Form ParentForm { get { return BaseForm ?? base.ParentForm; } }


        public VOneScreenBase()
        {
            InitializeComponent();
            Load += (sender, e) =>
            {
                if (OnF10ClickHandler == null)
                {
                    BaseContext?.SetFunction10Caption("終了");
                    OnF10ClickHandler = Close;
                }

                if (ApplicationContext != null) this.InitializeFont(ApplicationContext.FontFamilyName);
            };
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm != null)
            {
                ParentForm.KeyPreview = true;
                ParentForm.KeyDown += (sender, e) =>
                {
                    if (e.KeyCode != Keys.Enter
                    || ActiveControl is Button
                    || IsMultiLineTextBox(ActiveControl)) return;

                    var containers = new Stack<ContainerControl>();
                    {
                        var container = (ContainerControl)this;
                        while (container.ActiveControl is ContainerControl)
                            containers.Push(container = (ContainerControl)container.ActiveControl);
                    }
                    try
                    {
                        var forward = !e.Shift;
                        foreach (var container in containers)
                            if (container.SelectNextControl(container.ActiveControl, forward,
                                tabStopOnly: true, nested: true, wrap: false)) return;
                        ParentForm.SelectNextControl(ActiveControl, forward,
                            tabStopOnly: true, nested: true, wrap: true);
                    }
                    finally
                    {
                        containers?.Clear();
                        GC.Collect();
                    }
                };
                ParentForm.Shown += (sender, e) => this.ViewOpened();
            }
        }

        private bool IsMultiLineTextBox(Control c)
        {
            return (c is TextBoxBase && (c as TextBoxBase).Multiline)
                || (c is GrapeCity.Win.Editors.GcTextBox && (c as GrapeCity.Win.Editors.GcTextBox).Multiline);
        }

        protected ApplicationControl ApplicationControl { get; set; }
        protected Company Company { get; set; }

        protected bool IsConfirmRequired { get { return Company?.ShowConfirmDialog == 1; } }

        protected bool IsWarningRequired { get { return Company?.ShowWarningDialog == 1; } }

        protected bool AutoCloseProgressDialog { get { return Company?.AutoCloseProgressDialog == 1; } }

        protected FunctionAuthorities Authorities { get; set; }

        protected XmlMessenger XmlMessenger { get; } = new XmlMessenger();

        /// <summary>編集状態管理用</summary>
        protected internal bool Modified { get; set; }

        #region load data
        protected async Task LoadApplicationControlAsync()
        {
            await ServiceProxyFactory.DoAsync<ApplicationControlMasterClient>(async client =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    ApplicationControl = result.ApplicationControl;
                }
            });
        }

        protected async Task LoadCompanyAsync()
        {
            await ServiceProxyFactory.DoAsync<CompanyMasterClient>(async client =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    Company = result.Company;
                }
            });
        }

        protected async Task LoadFunctionAuthorities(params FunctionType[] functionTypes)
        {
            IEnumerable<FunctionAuthority> authorities = null;
            await ServiceProxyFactory.DoAsync<FunctionAuthorityMasterClient>(async client =>
            {
                var result = await client.GetByLoginUserAsync(SessionKey, CompanyId, Login.UserCode, functionTypes.Cast<int>().ToArray());
                if (result.ProcessResult.Result)
                {
                    authorities = result.FunctionAuthorities;
                }
            });
            Authorities = new FunctionAuthorities(authorities);
        }

        protected async Task LoadControlColorAsync()
        {
            if (ColorSetting.Current != ColorSetting.Default)
            {
                InitializeColors();
                return;
            }
            await ServiceProxyFactory.DoAsync<ControlColorMasterClient>(async client =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId, Login.UserId);
                if (result.ProcessResult.Result && result.Color != null)
                {
                    ColorSetting.FromDb = new ColorSetting(result.Color.FirstOrDefault());
                }
                InitializeColors();
            });
        }

        #endregion

        #region IApplication

        protected IApplication ApplicationContext { get; set; }
        public void SetApplicationContext(IApplication applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        public void SetClosing()
        {
            var basicForm = BaseForm as BasicForm;
            if (basicForm == null || CompanyId == 0) return;
            var information = UtilClosing.GetClosingInformation(SessionKey, CompanyId);
            basicForm.SetClosingInformation(information);
        }
        #endregion

        #region IExecutableFunctionKeys

        public IFunctionKeys BaseContext { get; protected set; }

        public void SetBaseContext(IFunctionKeys baseContext)
        {
            this.BaseContext = baseContext;
            InitializeFunctionKeys();
        }

        public void OnFunctionKey01Click() { this.FunctionCalled(1, OnF01ClickHandler); }
        public void OnFunctionKey02Click() { this.FunctionCalled(2, OnF02ClickHandler); }
        public void OnFunctionKey03Click() { this.FunctionCalled(3, OnF03ClickHandler); }
        public void OnFunctionKey04Click() { this.FunctionCalled(4, OnF04ClickHandler); }
        public void OnFunctionKey05Click() { this.FunctionCalled(5, OnF05ClickHandler); }
        public void OnFunctionKey06Click() { this.FunctionCalled(6, OnF06ClickHandler); }
        public void OnFunctionKey07Click() { this.FunctionCalled(7, OnF07ClickHandler); }
        public void OnFunctionKey08Click() { this.FunctionCalled(8, OnF08ClickHandler); }
        public void OnFunctionKey09Click() { this.FunctionCalled(9, OnF09ClickHandler); }
        public void OnFunctionKey10Click() { this.FunctionCalled(10, OnF10ClickHandler); }
        public void OnFunctionKey11Click() { }
        public void OnFunctionKey12Click() { }

        #endregion

        #region IDisplayStatusMessage
        private IMessage statusContext { get; set; }

        public void SetStatusMessageContext(IMessage context)
        {
            this.statusContext = context;
            this.statusContext?.ClearStatusMessage();
        }

        protected void DispStatusMessage(string messageId, params string[] args)
        {
            try
            {
                DispStatusMessage(XmlMessenger.GetMessageInfo(messageId, args));
            }
            catch (Exception)
            {

            }
        }

        protected void DispStatusMessage(MessageInfo message)
        {
            if (message != null)
            {
                statusContext?.DispStatusMessage(
                        message.Text, message.Title, message.Color,
                        message.DoBeep, message.Icon, message.Buttons);
            }
        }

        protected void ClearStatusMessage()
        {
            statusContext?.ClearStatusMessage();
        }

        protected bool ShowConfirmDialog(string messageId, params string[] args)
        {
            if (!IsConfirmRequired) return true;

            var message = XmlMessenger.GetMessageInfo(messageId, args);
            var result = message.ShowMessageBox(ParentForm);
            this.Confirmed(result);
            return result == DialogResult.Yes
                || result == DialogResult.OK;
        }

        protected DialogResult ShowConfirmDialogYesNoCancel(string messageId, params string[] args)
        {
            if (!IsConfirmRequired) return DialogResult.No;
            var message = XmlMessenger.GetMessageInfo(messageId, args);
            var result = message.ShowMessageBox(ParentForm);
            this.Confirmed(result);
            return result;
        }

        protected void ShowWarningDialog(string messageId, params string[] args)
        {
            var message = XmlMessenger.GetMessageInfo(messageId, args);
            if (IsWarningRequired)
            {
                this.Confirmed(message.ShowMessageBox(ParentForm));
            }
            DispStatusMessage(message);
        }

        /// <summary>非同期処理から、画面側で何か行う場合に呼び出し
        /// しばらくお待ちくださいのダイアログが表示されているうえで処理されるので注意
        /// </summary>
        /// <param name="action"></param>
        protected void DoActionOnUI(Action action)
        {
            if (InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }

        #endregion

        #region IScreenColors

        protected IColors ColorContext
        {
            get { return ColorSetting.Current; }
        }

        public void SetColorsContext(IColors context)
        {
            ColorSetting.FromUser = context;
        }

        public void InitializeColors()
        {
            if (ColorContext != null)
            {
                this.BackColor = ColorContext.FormBackColor;
                this.ForeColor = ColorContext.FormForeColor;

                if (ParentForm != null
                    && ParentForm is BasicForm)
                {
                    ParentForm.BackColor = ColorContext.FormBackColor;
                    ParentForm.ForeColor = ColorContext.FormForeColor;
                }

                this.InitializeColor(ColorContext);
            }
        }

        #endregion

        #region IDisplayScreenName

        private IScreenName screenNameContext { get; set; }

        public void SetScreenNameContext(IScreenName context)
        {
            this.screenNameContext = context;
        }

        protected void SetScreenName()
        {
            screenNameContext?.SetScreenName(Text);
        }

        #endregion

        #region function keys

        protected virtual void InitializeFunctionKeys()
        {
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);
            BaseContext.SetFunction01Caption("更新");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction10Caption("終了");
        }

        protected Action OnF01ClickHandler = null;
        protected Action OnF02ClickHandler = null;
        protected Action OnF03ClickHandler = null;
        protected Action OnF04ClickHandler = null;
        protected Action OnF05ClickHandler = null;
        protected Action OnF06ClickHandler = null;
        protected Action OnF07ClickHandler = null;
        protected Action OnF08ClickHandler = null;
        protected Action OnF09ClickHandler = null;
        protected Action OnF10ClickHandler = null;

        #endregion

        private void Close()
        {
            ParentForm.Close();
        }

        #region Dialog

        protected TDialog CreateDialog<TDialog>()
            where TDialog : Dialog, new()
        {
            var dialog = new TDialog();
            dialog.CompanyInfo = Company;
            dialog.ApplicationContext = ApplicationContext;
            dialog.ApplicationControl = ApplicationControl;
            dialog.XmlMessenger = XmlMessenger;

            return dialog;
        }

        protected DialogResult ProgressDialogStart(IWin32Window parent,
            string caption,
            Task task,
            TaskProgressManager manager,
            ILogin login,
            bool autoClose,
            bool cancellable = false,
            System.Action onCancel = null)
        {
            if (manager.Completed) return DialogResult.OK;

            using (var dialog = CreateDialog<ProgressDialogState>())
            using (var cancel = new System.Threading.CancellationTokenSource())
            {
                manager.Progress = dialog.Progress;
                dialog.Manager = manager;
                dialog.Text = caption;
                dialog.OnCancel = onCancel;
                dialog.Initialize(cancellable);

                dialog.AutoClose = autoClose;
                dialog.Shown += async (sender, e) =>
                {
                    try
                    {
                        var reporter = (IProgress<bool>)dialog.Progress;
                        reporter?.Report(false);

                        await task.ContinueWith(t =>
                        {
                            var cancelled = dialog.CancellationToken.IsCancellationRequested;
                            if (cancelled)
                            {
                                manager.Cancel();
                                if (dialog.AutoClose)
                                    dialog.DialogResult = DialogResult.Cancel;
                            }
                        }, cancel.Token);

                    }
                    catch (Exception)
                    {
                        manager.Abort();
                    }
                };

                dialog.ShowDialog(parent);

                return dialog.DialogResult;
            }
        }

        /// <summary>ファイルを開く ダイアログの表示用 helper</summary>
        /// <param name="initialDirectory">初期フォルダ</param>
        /// <param name="fileNames">決定したファイルリスト</param>
        /// <param name="filter">開くするファイル種別</param>
        /// <param name="index">初期表示するファイル種別</param>
        /// <param name="multiSelect">複数ファイル選択可否フラグ</param>
        /// <param name="initialFileName">初期表示するファイル名</param>
        /// <returns></returns>
        protected bool ShowOpenFileDialog(string initialDirectory,
            out List<string> fileNames,
            string filter = "すべてのファイル (*.*)|*.*|CSVファイル (*.csv)|*.csv",
            int index = 2,
            bool multiSelect = false,
            string initialFileName = "")
        {
            fileNames = new List<string>();
            DialogResult res = DialogResult.Cancel;
            using (var dialog = new OpenFileDialog())
            {
                dialog.Reset();
                dialog.InitialDirectory = initialDirectory;
                dialog.Filter = filter;
                dialog.FilterIndex = index;
                dialog.Multiselect = multiSelect;
                dialog.FileName = initialFileName;
                res = this.FileSelected(dialog, dialog.ShowDialog(ParentForm));
                fileNames = dialog.FileNames.ToList();
            }
            return (res == DialogResult.OK);
        }

        /// <summary>フォルダを開く ダイアログの表示用 helper</summary>
        /// <param name="initialDirectory">初期フォルダ</param>
        /// <param name="selectedPath">決定したフォルダ</param>
        /// <param name="description">説明テキスト</param>
        /// <returns></returns>
        protected bool ShowFolderBrowserDialog(string initialSelectedPath,
            out string selectedPath,
            string description = "フォルダーの参照")
        {
            selectedPath = string.Empty;
            DialogResult res = DialogResult.Cancel;

            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Reset();
                dialog.Description = description;
                dialog.SelectedPath = initialSelectedPath;
                res = this.Confirmed(dialog.ShowDialog(ParentForm));
                selectedPath = dialog.SelectedPath;
            }
            return (res == DialogResult.OK);
        }

        /// <summary>ファイルを保存 ダイアログの表示用 helper</summary>
        /// <param name="initialDirectory">初期フォルダ</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="filePath">決定したファイル名</param>
        /// <param name="filter">保存するファイル種別</param>
        /// <param name="index">初期表示するファイル種別</param>
        /// <param name="confirmMessagging">出力確認のメッセージ</param>
        /// <param name="cancellationMessaging">出力キャンセルのメッセージ</param>
        /// <returns></returns>
        protected bool ShowSaveFileDialog(string initialDirectory,
            string fileName,
            out string filePath,
            string filter = "すべてのファイル (*.*)|*.*|CSVファイル (*.csv)|*.csv",
            int index = 2,
            Func<bool> confirmMessagging = null,
            Action cancellationMessaging = null)
        {
            filePath = string.Empty;
            var result = false;
            if (!IsConfirmRequired) confirmMessagging = null;

            if (LimitAccessFolder)
            {
                List<string> filesPath;
                result = ShowRootFolderBrowserDialog(initialDirectory, out filesPath, FolderBrowserType.SaveFile, fileName, confirmMessagging, cancellationMessaging);
                filePath = filesPath?.FirstOrDefault() ?? string.Empty;
                return result;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Reset();
                dialog.InitialDirectory = initialDirectory;
                dialog.FileName = fileName;
                dialog.Filter = filter;
                dialog.FilterIndex = index;

                if (this.FileSelected(dialog, dialog.ShowDialog(ParentForm)) != DialogResult.OK
                    || !(confirmMessagging?.Invoke() ?? true))
                {
                    cancellationMessaging?.Invoke();
                    return result;
                }
                try
                {
                    filePath = dialog.FileName;
                    result = !string.IsNullOrEmpty(filePath);
                }
                catch (System.IO.IOException)
                {
                    ShowWarningDialog(Constants.MsgWngPathTooLong);
                }
            }

            return result;
        }
        /// <summary>
        /// ファイルを保存 ダイアログの表示用 helper エクスポート用
        /// </summary>
        /// <param name="initialDirectory">初期フォルダ</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="filePath">決定したファイル名</param>
        /// <param name="filter">保存するファイル種別</param>
        /// <param name="index">初期表示するファイル種別</param>
        /// <returns></returns>
        protected bool ShowSaveExportFileDialog(string initialDirectory,
            string fileName,
            out string filePath,
            string filter = "すべてのファイル (*.*)|*.*|CSVファイル (*.csv)|*.csv",
            int index = 2)
            => ShowSaveFileDialog(initialDirectory, fileName, out filePath, filter, index,
                () => ShowConfirmDialog(Constants.MsgQstConfirmExport),
                () => DispStatusMessage(Constants.MsgInfCancelProcess, "エクスポート"));

        protected bool ShowRootFolderBrowserDialog(string initialDirectory,
            out List<string> filesPath,
            FolderBrowserType browserType,
            string fileName = "",
            Func<bool> confirmMessagging = null,
            Action cancellationMessaging = null)
        {
            var result = false;
            using (var dialog = CreateDialog<dlgRootFolderBrowser>())
            {
                filesPath = new List<string>();
                dialog.FolderBrowserType = browserType;
                dialog.InitialDirectory = initialDirectory;
                dialog.FileName = fileName;

                dialog.RootPath = ApplicationControl.RootPath;
                dialog.StartPosition = FormStartPosition.CenterParent;

                if (this.Confirmed(ApplicationContext.ShowDialog(ParentForm, dialog, true)) != DialogResult.OK
                    || !(confirmMessagging?.Invoke() ?? true))
                {
                    cancellationMessaging?.Invoke();
                    filesPath = null;
                    return result;
                }
                try
                {
                    switch (browserType)
                    {
                        case FolderBrowserType.SelectMultiFile:
                            filesPath = dialog.MultiFiles;
                            result = filesPath.Any();
                            break;
                        default:
                            filesPath.Add(dialog.SelectedPath);
                            result = !string.IsNullOrEmpty(filesPath.FirstOrDefault());
                            break;
                    }
                }
                catch (System.IO.IOException)
                {
                    filesPath = null;
                    ShowWarningDialog(Constants.MsgWngPathTooLong);
                }
            }
            return result;
        }

        /// <summary>
        /// 印刷プレビューの表示
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="report"></param>
        /// <param name="initialDirectory"></param>
        /// <param name="outputHandler"></param>
        /// <returns></returns>
        protected DialogResult ShowDialogPreview(
            IWin32Window owner,
            GrapeCity.ActiveReports.SectionReport report,
            string initialDirectory = null,
            Action<Form> outputHandler = null)
        {
            using (var form = new frmVOnePreviewForm(report))
            {
                form.InitialExportPdfPath = initialDirectory;
                form.OutputHandler = outputHandler;
                form.ShowSaveFileDialogHandler = ShowSaveFileDialogForPdfExport;
                var ownerForm = owner as Form;
                form.StartPosition = FormStartPosition.Manual;
                form.SetBounds(ownerForm.Left, ownerForm.Top, ownerForm.Width, ownerForm.Height);
                return form.ShowDialog(ParentForm);
            }
        }

        /// <summary>印刷プレビューから PDF出力時の ファイル保存ダイアログの実装</summary>
        /// <param name="initialDirectory"></param>
        /// <param name="fileName"></param>
        /// <param name="filter"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool ShowSaveFileDialogForPdfExport(string initialDirectory, string fileName, string filter, out string path)
        {
            path = string.Empty;
            if (LimitAccessFolder)
            {
                List<string> paths;
                var result = ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out paths, FolderBrowserType.SaveFile, fileName);
                if (result) path = paths.FirstOrDefault();
                return result;
            }
            else
            {
                return ShowSaveFileDialog(initialDirectory, fileName, out path, filter);
            }
        }


        /// <summary>インポート共通処理</summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TIdentity">Id特定方法</typeparam>
        /// <param name="importer">インポーター</param>
        /// <param name="setting">取込設定</param>
        /// <param name="importSuccessPostAction">取込成功後のクリア処理など メッセージ表示より前に実施する項目</param>
        /// <returns></returns>
        protected bool DoImport<TModel, TIdentity>(Import.CsvImporter<TModel, TIdentity> importer,
            ImportSetting setting,
            Action importSuccessPostAction = null)
            where TModel : class, new()
        {
            using (var form = ApplicationContext.Create(nameof(PH9907)))
            {
                var screen = form.GetAll<PH9907>().First();
                var result = screen.ConfirmImportSetting(ParentForm, importer, setting);
                if (result != DialogResult.OK)
                {
                    DispStatusMessage(Constants.MsgInfCancelProcess, "インポート");
                    return false;
                }
                importer.ErrorLogPath = setting.GetErrorLogPath();
            }
            ImportResult res = null;
            DialogResult pgres = DialogResult.None;

            NLogHandler.WriteDebug(this, $"{typeof(TModel).Name}:インポート処理開始");
            try
            {
                pgres = ProgressDialog.Start(ParentForm, async (cancel, progress) =>
                    res = await importer.ImportAsync(setting.ImportFileName,
                        (Import.ImportMethod)setting.ImportMode, cancel, progress),
                    true, SessionKey);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex.InnerException, SessionKey);
            }
            NLogHandler.WriteDebug(this, $"{typeof(TModel).Name}:インポート処理終了");
            var importResult = false;
            if (pgres == DialogResult.Cancel)
            {
                DispStatusMessage(Constants.MsgInfCancelProcess, "インポート");
            }
            else if (pgres == DialogResult.Abort)
            {
                DispStatusMessage(Constants.MsgErrImportErrorWithoutLog);
            }
            else if (res != null)
            {
                var message = XmlMessenger.GetMessageInfo(res.GetMessageId());
                if (message.Category == MessageCategory.Information)
                {
                    importSuccessPostAction?.Invoke();
                    DispStatusMessage(message);
                    importResult = true;
                }
                else
                    ShowWarningDialog(message.ID);
            }
            return importResult;
        }

        protected string FormTitle { get; set; }
        protected int FormWidth { get; set; }
        protected int FormHeight { get; set; }
        protected Action<IEnumerable<Button>> FunctionKeysSetter { get; set; }

        public void InitializeParentForm(string formTitle,
            int? width = null, int? height = null)
        {
            if (ParentForm == null) return;
            Text = formTitle;
            SetScreenName();
            FormTitle = formTitle;
            if (width.HasValue) FormWidth = width.Value;
            if (height.HasValue) FormHeight = height.Value;

            var basicForm = ParentForm as BasicForm;
            if (basicForm == null) return;
            this.MinimumSize = new Size(0, 0);

            basicForm.StartPosition = FormStartPosition.CenterParent;
            basicForm.Text = FormTitle;
            basicForm.MaximizeBox = false;
            basicForm.MinimizeBox = false;

            var headerPanel = basicForm.GetAll<Panel>()
                .Where(x => x.Name == "pnlHeader").FirstOrDefault();
            if (headerPanel != null)
                headerPanel.Visible = false;

            var buttons = basicForm.GetAll<Button>()
                .Where(x => x.Name.StartsWith("btnF"));
            if (buttons != null) FunctionKeysSetter?.Invoke(buttons);

            basicForm.MinimumSize = new Size(FormWidth + 2, FormHeight);
            basicForm.MaximumSize = basicForm.MinimumSize;
        }

        #endregion

        protected new bool ValidateChildren()
        {
            ((Control)ParentForm ?? this).Focus();
            return true;
        }

        protected bool IsNeedValidate(int codeType, int textLenght, int codeLength)
            => (codeType == 0 && textLenght != 0 && textLenght != codeLength);

        protected bool IsValidClosingDay(string closingDay, int codeLength)
        {
            int day = 0;
            int.TryParse(closingDay, out day);
            return (day < 28 || day == 99) && (closingDay.Length == codeLength);
        }

        protected string ZeroLeftPadding(VOneTextControl textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) return string.Empty;
            return textBox.Text.Trim().PadLeft(textBox.MaxLength, textBox.PaddingChar ?? '0');
        }

        #region ILoggableView
        ApplicationControl ILoggable.ApplicationControl { get { return ApplicationControl; } }

        string ILoggable.Caption { get { return Text; } }

        ILogin ILoggable.Login { get { return Login; } }

        protected ILogin Login { get { return ApplicationContext?.Login; } }


        protected string SessionKey { get { return Login?.SessionKey; } }
        protected int CompanyId { get { return Login?.CompanyId ?? 0; } }
        #endregion

        #region IApplicationUseable
        IApplication IApplicationUsable.ApplicationContext { get { return ApplicationContext; } }
        #endregion
    }
}
