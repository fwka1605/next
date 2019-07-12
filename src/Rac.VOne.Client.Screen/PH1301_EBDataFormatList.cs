using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.EBFileSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>EBファイル設定一覧 / EBファイル設定マスター</summary>
    public partial class PH1301 : VOneScreenBase
    {
        #region 変数宣言
        private List<EBFileSetting> FileSettingList { get; set; } = new List<EBFileSetting>();
        #endregion

        #region PH1301 EBファイル設定一覧
        public PH1301()
        {
            InitializeComponent();
            grdEBFileFormatSetting.SetupShortcutKeys();
            Text = "EBファイル設定一覧";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            Load += PH1301_Load;
            grdEBFileFormatSetting.CellDoubleClick += grdEBFileFormatSetting_CellContentClick;
        }
        #endregion
        
        #region PH1301 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction07Caption("新規作成");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF07ClickHandler = Create;
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PH1301_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            await Task.WhenAll(
                    LoadCompanyAsync(),
                    LoadApplicationControlAsync(),
                    LoadControlColorAsync()
                );
            InitializeGridTemplate();
            await LoadGridDataAsync();
        }
        #endregion

        #region グリッド初期設定
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  20, "Header"                                                                                    , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height,  30, nameof(EBFileSetting.IsUseable)         , dataField: nameof(EBFileSetting.IsUseable)        , cell: builder.GetCheckBoxCell()            , caption: "使用"            , readOnly: false),
                new CellSetting(height, 220, nameof(EBFileSetting.Name)              , dataField: nameof(EBFileSetting.Name)             , cell: builder.GetTextBoxCell()             , caption: "EBファイル設定名"  ),
                new CellSetting(height, 120, nameof(EBFileSetting.FileFieldTypeName) , dataField: nameof(EBFileSetting.FileFieldTypeName), cell: builder.GetTextBoxCell(middleCenter) , caption: "区切文字"        ),
                new CellSetting(height,  65, nameof(EBFileSetting.BankCode)          , dataField: nameof(EBFileSetting.BankCode)         , cell: builder.GetTextBoxCell(middleCenter) , caption: "銀行コード"      ),
                new CellSetting(height, 150, nameof(EBFileSetting.ImportableValues)  , dataField: nameof(EBFileSetting.ImportableValues) , cell: builder.GetTextBoxCell()             , caption: "取込区分"        ),
                new CellSetting(height, 130, nameof(EBFileSetting.FilePath)          , dataField: nameof(EBFileSetting.FilePath)         , cell: builder.GetTextBoxCell()             , caption: "取込ファイルパス"),
            });
            grdEBFileFormatSetting.Template = builder.Build();
        }
        #endregion

        #region グリッドのリスト設定
        private async Task LoadGridDataAsync()
            => await ServiceProxyFactory.DoAsync(async (EBFileSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                var source = new List<EBFileSetting>();
                if (result.ProcessResult.Result)
                    source = result.EBFileSettings;
                grdEBFileFormatSetting.DataSource = new BindingSource(source, null);
            });
        #endregion

        #region 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            BaseForm.Close();
        }
        #endregion

        #region 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var source = (grdEBFileFormatSetting.DataSource as BindingSource).DataSource as List<EBFileSetting>;
                var ids = GetIdForSave();
                var saveTask = SaveAsync(ids);

                ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);

                if (!saveTask.Result)
                {
                    DispStatusMessage(MsgErrSaveError);
                    return;
                }

                ProgressDialog.Start(ParentForm, LoadGridDataAsync(), false, SessionKey);
                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion
        private List<int> GetIdForSave()
        {
            return grdEBFileFormatSetting.Rows
                .Select(x => x.DataBoundItem as EBFileSetting)
                .Where(x => x.IsUseable == 1)
                .Select(x => x.Id).ToList();
        }
        private async Task<bool> SaveAsync(List<int> ids)
            => await ServiceProxyFactory.DoAsync(async (EBFileSettingMasterClient client) => {
                var result= await client.UpdateIsUseableAsync(SessionKey, CompanyId, Login.UserId, ids.ToArray());
                return result?.ProcessResult.Result ?? false;
            });

        #region 新規作成
        [OperationLog("新規作成")]
        private void Create()
        {
            CallPH1302();
        }
        private void CallPH1302(EBFileSetting format = null)
        {
            using (var form = ApplicationContext.Create(nameof(PH1302)))
            {
                var screen = form.GetAll<PH1302>().First();
                screen.Format = format;
                screen.ReturnScreen = this;
                form.StartPosition = FormStartPosition.CenterParent;
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form);

                ProgressDialog.Start(ParentForm, LoadGridDataAsync(), false, SessionKey);
                ClearStatusMessage();
                if (dialogResult != DialogResult.OK) return;
            }
        }
        #endregion
        private void grdEBFileFormatSetting_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            CallPH1302(grdEBFileFormatSetting.Rows[e.RowIndex].DataBoundItem as EBFileSetting);
        }
    }
}