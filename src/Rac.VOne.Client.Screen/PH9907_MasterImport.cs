using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Rac.VOne.Web.Models;
using Rac.VOne.Import;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.Dialogs;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    public partial class PH9907 : VOneScreenBase
    {
        /// <summary>ログファイル出力有無</summary>
        private bool OutputLogFile
        {
            get { return cbxOutputLogFile.Checked; }
            set
            {
                var changed = cbxOutputLogFile.Checked != value;
                cbxOutputLogFile.Checked = value;
                cmbLogFilePath.Enabled = (!LimitAccessFolder) ? value : false;
                if (!value)
                    cmbLogFilePath.SelectedIndex = -1;
                else if (changed)
                    cmbLogFilePath.SelectedIndex = (!LimitAccessFolder) ? 0 : 1;
            }
        }
        private ImportSetting setting { get; set; }

        /// <summary>インポートのパス保存</summary>
        private Action<string> SaveFilePathCallback { get; set; }

        public PH9907() : base()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            FormWidth = 500;
            FormHeight = 280;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF04"
                        || button.Name == "btnF06"
                        || button.Name == "btnF08")
                    {
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        if (button.Name == "btnF04")
                            button.Location = new Point(1, 0);
                        if (button.Name == "btnF06")
                            button.Location = new Point(102, 0);
                        if (button.Name == "btnF08")
                            button.Location = new Point(203, 0);
                    }
                    else if (button.Name == "btnF10")
                    {
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    }
                    else
                        button.Visible = false;
                }
            };
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Shown += parentForm_Shown;
            cbxOutputLogFile.CheckedChanged += cbxOutputLogFile_CheckedChanged;
        }

        private void parentForm_Shown(object sender, EventArgs e)
        {
            pbxIcon.Image = SystemIcons.Question.ToBitmap();

            InitializeApplicationData();

            OutputLogFile = (setting.ExportErrorLog == 1);
            if (OutputLogFile)
                cmbLogFilePath.SelectedIndex = setting.ErrorLogDestination;

            if (!(ParentForm is BasicForm)) return;

            var buttonName = string.Empty;
            switch (setting.ImportMode)
            {
                case 0: buttonName = "btnF04"; break;
                case 1: buttonName = "btnF06"; break;
                case 2: buttonName = "btnF08"; break;
            }
            var button = ParentForm.GetAll<Button>()
                .FirstOrDefault(x => x.Name == buttonName);
            button?.Focus();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("");
            BaseContext.SetFunction01Enabled(false);
            OnF01ClickHandler = null;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = null;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction04Caption("上書");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = ImportReplace;

            BaseContext.SetFunction06Caption("追加");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ImportAdditional;

            BaseContext.SetFunction08Caption("更新");
            BaseContext.SetFunction08Enabled(true);
            OnF08ClickHandler = ImportUpdate;

            BaseContext.SetFunction10Caption("キャンセル");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Cancel;
        }

        private void InitializeApplicationData()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
            };
            ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
        }

        public DialogResult ConfirmImportSetting<TModel, TIdentity>(IWin32Window owner,
            CsvImporter<TModel, TIdentity> importer,
            ImportSetting setting)
            where TModel : class, new()
        {
            SaveFilePathCallback = SaveFilePath<TModel>;

            // 初期設定
            var fileName = $"{importer.RowDef.FileNameToken}{DateTime.Today:yyyyMMdd}.csv";
            var filePath = GetFullPath<TModel>(fileName);
            setting.ImportFileName = filePath;
            return ConfirmImportSettingInner(owner, setting);
        }

        /// <summary>得意先マスター<see cref="Customer"/>用</summary>
        /// <param name="owner"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        /// <remarks>フォルダは ImporterSetting から取得したものを設定</remarks>
        public DialogResult ConfirmImportSetting(IWin32Window owner,
            ImportSetting setting)
        {
            return ConfirmImportSettingInner(owner, setting);
        }

        private string GetFullPath<TModel>(string fileName)
        {
            var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // 設定ファイル読込(なければ作成)
            var prevPath = Settings.RestorePath<TModel>(Login);
            var initialDirectory = Path.GetDirectoryName(string.IsNullOrEmpty(prevPath) ? directoryPath : prevPath);
            return Path.Combine(initialDirectory, fileName);
        }

        private void SaveFilePath<TModel>(string path)
        {
            try
            {
                Settings.SavePath<TModel>(Login, path);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private DialogResult ConfirmImportSettingInner(IWin32Window owner,
            ImportSetting setting)
        {
            this.setting = setting;
            if (setting.Confirm == 1)
            {
                InitializeParentForm("インポート方法の選択");
                return ApplicationContext.ShowDialog(owner, ParentForm, true);
            }
            else
            {
                var path = GetSelectedFile(owner, setting.ImportFileName);
                if (string.IsNullOrEmpty(path)) return DialogResult.Cancel;

                setting.ImportFileName = path;
                return DialogResult.OK;
            }
        }

        /// <summary>ユーザーからのファイル指定を受け付ける。</summary>
        /// <param name="owner">親画面</param>
        /// <param name="path">初期パス</param>
        /// <returns>選択されたファイルのフルパス</returns>
        private string GetSelectedFile(IWin32Window owner, string path)
        {
            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);

            var currentPathBackup = Directory.GetCurrentDirectory();
            try
            {
                var fileNames = new List<string>();
                if (!LimitAccessFolder ?
                    !ShowOpenFileDialog(dir, out fileNames, initialFileName : fileName) :
                    !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectFile, fileName)) return null;

                path = fileNames?.FirstOrDefault() ?? string.Empty;
                SaveFilePathCallback?.Invoke(path);
                return fileNames?.FirstOrDefault() ?? string.Empty;

            }
            finally
            {
                Directory.SetCurrentDirectory(currentPathBackup);
            }
        }
        private bool ConfirmImportStart()
        {
            var messageId
                = setting.ImportMode == 0 ? MsgQstImportOverWriteExistsData
                : setting.ImportMode == 1 ? MsgQstImportAddNewDataOnly
                : setting.ImportMode == 2 ? MsgQstImportUpdateExistsAndAddNewData
                : string.Empty;
            return ShowConfirmDialog(messageId);
        }

        #region event handler and process cmd key

        [OperationLog("上書")]
        private void ImportReplace()
        {
            ConfirmImportMethod(importMode: 0);
        }

        [OperationLog("追加")]
        private void ImportAdditional()
        {
            ConfirmImportMethod(importMode: 1);
        }

        [OperationLog("更新")]
        private void ImportUpdate()
        {
            ConfirmImportMethod(importMode: 2);
        }

        [OperationLog("キャンセル")]
        private void Cancel()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 取込方法決定 かつ ファイル選択完了確定処理
        /// </summary>
        /// <param name="importMode">0 : 上書き, 1 : 追加, 2 : 更新</param>
        private void ConfirmImportMethod(int importMode)
        {
            setting.ImportMode = importMode;
            var filePath = GetSelectedFile(ParentForm, setting.ImportFileName);
            if (string.IsNullOrEmpty(filePath))
            {
                ParentForm.DialogResult = DialogResult.Cancel;
                return;
            }
            setting.ImportFileName = filePath;
            setting.ExportErrorLog = OutputLogFile ? 1 : 0;
            setting.ErrorLogDestination = cmbLogFilePath.SelectedIndex;
            ParentForm.DialogResult = (!ConfirmImportStart()) ? DialogResult.Cancel : DialogResult.OK;
        }

        private void cbxOutputLogFile_CheckedChanged(object sender, EventArgs e)
        {
            cmbLogFilePath.Enabled = (!LimitAccessFolder) ? cbxOutputLogFile.Checked : false;
            cmbLogFilePath.SelectedIndex = cbxOutputLogFile.Checked ? (!LimitAccessFolder) ? 0:1 : -1;
        }

        #endregion
    }
}
