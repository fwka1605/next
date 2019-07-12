using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Screen.ApplicationControlMasterService;
using Rac.VOne.Client.Screen.Extensions;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>メインメニュー</summary>
    public partial class PA0201 : Form
    {
        /// <summary>
        /// アプリケーション共有データ
        /// </summary>
        private readonly IApplication ApplicationContext;

        /// <summary>
        /// カテゴリボタン配列
        /// </summary>
        private List<SQNumButton> CategoryButtonList;

        /// <summary>
        /// メニューボタン配列
        /// </summary>
        private CircleNumButton[] MenuButtonList;

        /// <summary>
        /// カテゴリとメニュー項目リストを関連付けるディクショナリ
        /// <para>Key=MenuCategory, Value=IEnumerable(MenuAuthority)</para>
        /// </summary>
        private Dictionary<string, IEnumerable<MenuAuthority>> CategoryMenuItemListDictionary;

        /// <summary>
        /// カテゴリとメニュータイトルを関連付けるディクショナリ
        /// <para>Key=MenuCategory, Value=IEnumerable(MenuAuthority)</para>
        /// </summary>
        private Dictionary<string, string> CategoryMenuTitleDictionary;

        #region 初期化

        public PA0201()
        {
            InitializeComponent();

            var isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
            Text = (isCloudEdition) ? CloudApplicationName : StandardApplicationName;
            Icon = (isCloudEdition) ? Properties.Resources.cloud_icon : Properties.Resources.app_icon;
            picLogo.Image = (isCloudEdition) ? Properties.Resources.cloud_logo : Properties.Resources.logo2;
            MinimizeBox = !isCloudEdition;
        }

        public PA0201(IApplication applicationContext) : this()
        {
            ApplicationContext = applicationContext;
        }

        /// <summary>
        /// 閉じるボタンを無効化する。
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_NOCLOSE = 0x200;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;
                return cp;
            }
        }

        private void PA0201_Load(object sender, EventArgs e)
        {
            // ログイン情報
            var login = ApplicationContext.Login;
            lblCompanyName.Text = $"【{login.CompanyCode}】 {login.CompanyName}";
            lblUserName.Text = $"{login.UserName}";

            //締め情報
            SetClosing();

            // 番号選択ボタン
            btnMove.FlatAppearance.BorderColor = Color.FromArgb(159, 194, 225);
            btnMove.Select();

            // カテゴリ／メニュー
            var menuItems = GetMenuItems(login.SessionKey, login.CompanyId, login.UserId);
            if (menuItems == null)
            {
                return; // UNDONE: Error Message
            }

            CategoryMenuItemListDictionary = GetCategoryMenuListDictionary(menuItems.MenuAuthorities);
            CategoryMenuTitleDictionary = new Dictionary<string, string>
            {
                { "PB", "設定" },
                { "PC", "請求" },
                { "PD", "入金" },
                { "PE", "消込" },
                { "PG", "配信" },
                { "PI", "督促管理"},
                { "PF", "管理帳票" },
                { "PH", "メンテナンス" },
            };

            var CategoryIconList = new Dictionary<string, Bitmap>
            {
                {"PB", Properties.Resources.btn_01},
                {"PC", Properties.Resources.btn_02},
                {"PD", Properties.Resources.btn_03},
                {"PE", Properties.Resources.btn_04},
                {"PG", Properties.Resources.btn_05},
                {"PI", Properties.Resources.btn_09},
                {"PF", Properties.Resources.btn_06},
                {"PH", Properties.Resources.btn_07},
            };

            CategoryButtonList = new List<SQNumButton>()
            {
                btnCategory1, btnCategory2, btnCategory3, btnCategory4, btnCategory5,
                btnCategory6, btnCategory7, btnCategory8, btnCategory9, btnCategory10,
            };
            
            var i = 0;
            foreach(var key in CategoryMenuTitleDictionary.Keys)
            {
                if (CategoryMenuItemListDictionary.ContainsKey(key))
                {
                    var button = CategoryButtonList[i];
                    button.ButtonIcon = CategoryIconList[key];
                    var caption = CategoryMenuTitleDictionary[key];
                    if (caption.Length == 2) caption = caption.Insert(1, " ");
                    button.TextCaption = caption;
                    button.TextNumber = (i + 1).ToString("00");
                    button.Visible = true;
                    button.InternalCode = key;
                    i++;
                }
            }

            //ログアウトボタン
            var logoutButton = CategoryButtonList[i];
            logoutButton.ButtonIcon = Properties.Resources.btn_08;
            logoutButton.TextCaption = "ログアウト";
            logoutButton.TextNumber = (i + 1).ToString("00");
            logoutButton.Visible = true;
            logoutButton.InternalCode = "$logout";


            MenuButtonList = new CircleNumButton[]
            {
                btnMenu1,   btnMenu2,   btnMenu3,
                btnMenu4,   btnMenu5,   btnMenu6,
                btnMenu7,   btnMenu8,   btnMenu9,
                btnMenu10,  btnMenu11,  btnMenu12,
                btnMenu13,  btnMenu14,  btnMenu15,
                btnMenu16,  btnMenu17,  btnMenu18,
                btnMenu19,  btnMenu20,  btnMenu21,
                btnMenu22,  btnMenu23,  btnMenu24,
            };

            HideMenu();
            txtNumber.Select();
        }
        private void SetClosing()
        {
            var information = UtilClosing.GetClosingInformation(ApplicationContext.Login.SessionKey,
                ApplicationContext.Login.CompanyId);
            if (information.UseClosing)
                lblClosingMonth.Text = information.ClosingDisplay;
            else
                lblClosingMonth.Visible = false;
        }

        /// <summary>
        /// Webサービスから取得したメニュー項目一覧データをカテゴリ引きの辞書データに加工する。
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private Dictionary<string, IEnumerable<MenuAuthority>> GetCategoryMenuListDictionary(List<MenuAuthority> items)
        {
            var dict = new Dictionary<string, IEnumerable<MenuAuthority>>();

            if (items == null || items.Count == 0)
            {
                return dict;
            }

            dict = items
                .OrderBy(m => m.MenuCategory)
                .ThenBy(m => m.Sequence)
                .GroupBy(m => m.MenuCategory, (key, list) => new { key, list })
                .ToDictionary(m => m.key, m => m.list);

            return dict;
        }

        private void PA0201_Shown(object sender, EventArgs e)
        {
            ArrangeLoginInfo();
        }

        /// <summary>
        /// ログイン情報表示ラベル位置調整
        /// </summary>
        private void ArrangeLoginInfo()
        {
            lblCompanyName.Location = new Point(pnlHeader.Width - (lblCompanyName.Width + 12), lblCompanyName.Top);
            lblUserName.Location = new Point(pnlHeader.Width - (lblUserName.Width + 12), lblUserName.Top);

            lblSetting.Top = (lblMenuHeaderHorizonLine.Top - lblSetting.Height) / 2;
        }

        #endregion 初期化

        #region 画面操作関連イベントハンドラ

        /// <summary>
        /// カテゴリボタン
        /// </summary>
        private void btnCategory_Click(object sender, EventArgs e)
        {
            var button = sender as SQNumButton;
            if (button != null) // Hack: チェックせずバグ出ししたほうが良いかも？
            {
                HandleCategoryButtonClick(button);
            }
        }

        /// <summary>
        /// メニューボタン
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            var button = sender as CircleNumButton;
            if (button != null) // Hack: チェックせずバグ出ししたほうが良いかも？
            {
                InvokeScreen(button);
            }
        }

        /// <summary>
        /// 「移動」ボタン
        /// </summary>
        private void btnMove_Click(object sender, EventArgs e)
        {
            InvokeSpecifiedItem();
        }

        /// <summary>
        /// 「戻る」「終了」ボタン
        /// </summary>
        private void btnBackOrExit_Click(object sender, EventArgs e)
        {
            if (HasMenuShown)
            {
                HideMenu();
            }
            else
            {
                if (ShowWarningDialog(MsgQstConfirmApplicationExit) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        #endregion 画面操作関連イベントハンドラ

        private bool _HasMenuShown;
        /// <summary>
        /// メニュータイトルまたはメニュー項目を表示中か否か。
        /// <para>値がSetterに与えられた際、併せて戻る／終了ボタンの表示内容も制御する。</para>
        /// </summary>
        private bool HasMenuShown
        {
            get
            {
                return _HasMenuShown;
            }
            set
            {
                _HasMenuShown = value;
                btnBackOrExit.ButtonText = value ? "戻る" : "終了";
            }
        }

        private void HideMenu()
        {
            ShowMenu(null);
        }
        /// <summary>
        /// カテゴリコードからメニュー項目を表示する。
        /// </summary>
        /// <param name="categoryCode">null時は初期化</param>
        private void ShowMenu(string categoryCode)
        {
            lblSetting.Text = "";
            MenuButtonList.ForEach((menuButton, idx) =>
            {
                var buttonNumber = (idx + 1).ToString("00");
                menuButton.SetText(buttonNumber, "");
                menuButton.InternalCode = null;
            });

            HasMenuShown = false;

            if (categoryCode == null)
            {
                return;
            }

            SetMenuTitle(categoryCode);
            SetMenuButtons(categoryCode);
        }

        /// <summary>
        /// メニュータイトルを表示する。
        /// </summary>
        /// <param name="categoryCode"></param>
        private void SetMenuTitle(string categoryCode)
        {
            string title;
            if (categoryCode == null || !CategoryMenuTitleDictionary.TryGetValue(categoryCode, out title))
            {
                return;
            }

            lblSetting.Text = title;
            HasMenuShown = true;
        }

        /// <summary>
        /// メニューボタン群を設定しラベルを表示する。
        /// </summary>
        /// <param name="categoryCode"></param>
        private void SetMenuButtons(string categoryCode)
        {
            if (categoryCode == null || !CategoryMenuItemListDictionary.ContainsKey(categoryCode))
            {
                return;
            }

            CategoryMenuItemListDictionary[categoryCode]
                .Take(MenuButtonList.Length)
                .ForEach((menu, idx) =>
                {
                    var buttonNumber = (idx + 1).ToString("00");
                    MenuButtonList[idx].SetText(buttonNumber, menu.MenuName);
                    MenuButtonList[idx].InternalCode = menu.MenuId; // 遷移先画面ID
                });

            HasMenuShown = true;
        }

        /// <summary>
        /// カテゴリボタンからメニュー項目を表示、またはボタン固有処理を実行する。
        /// </summary>
        private void HandleCategoryButtonClick(SQNumButton categoryButton)
        {
            var category = categoryButton.InternalCode; // カテゴリコード、または $ で始まる処理コード

            if (string.IsNullOrEmpty(category))
            {
                return;
            }

            if (!category.StartsWith("$"))
            {
                ShowMenu(category);
                return;
            }

            switch (category.Substring(1).ToLower())
            {
                case "logout":
                    if (ShowWarningDialog(MsgQstConfirmLogout) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    return;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// メニューボタン
        /// </summary>
        private void InvokeScreen(CircleNumButton menuButton)
        {
            var screenId = menuButton.InternalCode;

            if (screenId == null)
            {
                return;
            }

            // メニュー画面を操作不可(画面移動は可)にして機能画面をモードレス表示する。
            // 表示位置とサイズがメニュー画面と一致するよう調整する。
            // 機能画面が閉じられたらメニューを操作可能にする。
            // Ownerになってしまうとメニュー画面最小化時に機能画面も最小化してしまうので、Ownerなしとする。

            var form = ApplicationContext.Create(screenId);
            form.StartPosition = FormStartPosition.Manual;
            form.SetBounds(this.Location.X, this.Location.Y, this.Width, this.Height);
            form.FormClosed += (sender, e) => SetClosing();
            try
            {
                ChangeAllEnabledStatus(this, false);

                form.FormClosed += (_, __) => ChangeAllEnabledStatus(this, true);
                form.Show(this);
            }
            catch
            {
                ChangeAllEnabledStatus(this, true);
                throw;
            }
        }

        /// <summary>
        /// 「番号入力」欄に入力された番号と画面状態に従い「移動」ボタン処理を実行する。
        /// </summary>
        private void InvokeSpecifiedItem()
        {
            int number;
            if (txtNumber.Text == null || !int.TryParse(txtNumber.Text, out number))
            {
                txtNumber.Select();
                return;
            }

            txtNumber.Text = "";
            txtNumber.Select();

            // 戻る/終了
            if (number == 99)
            {
                btnBackOrExit_Click(null, null);
                return;
            }

            // Menu (画面を起動)
            if (HasMenuShown)
            {
                if (number < 1 || MenuButtonList.Length < number)
                {
                    return;
                }

                InvokeScreen(MenuButtonList[number - 1]);
            }

            // Category (該当メニューを表示)
            else
            {
                if (number < 1 || CategoryButtonList.Count < number)
                {
                    return;
                }

                var numberText = number.ToString("00");
                var button = CategoryButtonList.Where(b => b.TextNumber == numberText).FirstOrDefault();
                if (button != null)
                    HandleCategoryButtonClick(button);
            }
        }

        #region Web Service

        /// <summary>
        /// メニュー項目取得処理(MenuAuthorityMaster.svc:GetItems)を呼び出して結果を取得する。
        /// 権限が判定されるので、取得項目はユーザーによって可変。
        /// </summary>
        private static MenuAuthoritiesResult GetMenuItems(string sessionKey, int companyId, int loginUserId)
        {
            MenuAuthoritiesResult result = null;
            Exception error = null;

            ServiceProxyFactory.Do<MenuAuthorityMasterService.MenuAuthorityMasterClient>(client =>
            {
                try
                {
                    result = client.GetItems(sessionKey, companyId, loginUserId);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            });

            if (result == null || error != null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result;
        }

        #endregion Web Service

        #region Helper

        private static XmlMessenger Messenger = new XmlMessenger();

        /// <summary>
        /// メッセージボックスを表示する。
        /// </summary>
        /// <remarks>
        /// VOneScreenBaseから流用。
        /// </remarks>
        private DialogResult ShowWarningDialog(string messageId, params string[] args)
        {
            var message = Messenger.GetMessageInfo(messageId, args);

            return message.ShowMessageBox(this);
        }

        /// <summary>
        /// 指定したコントロール配下のコントロールEnabled状態を一括制御する。
        /// 最初に指定した最上位コントロールのEnabled状態は変更しない。
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state"></param>
        /// <param name="isRecursive">下位階層まで再帰的に処理するか否か</param>
        /// <remarks>
        /// 元々Enabled状態をプログラム制御していた場合を含め、全て上書きしてしまうので注意。
        /// </remarks>
        private static void ChangeAllEnabledStatus(Control control, bool state, bool isRecursive = false)
        {
            foreach (Control child in control.Controls)
            {
                child.Enabled = state;

                if (isRecursive && 0 < child.Controls.Count)
                {
                    ChangeAllEnabledStatus(child, state);
                }
            }
        }

        #endregion Helper

        //ロゴクリックでバージョン情報を表示する
        private void picLogo_Clicked(object sender, EventArgs e)
        {
            dlgShowVOneVersion sv = new dlgShowVOneVersion();
            sv.StartPosition = FormStartPosition.CenterParent;
            sv.ShowDialog();
        }
    }

    /// <summary>
    /// Ix(Interactive Extensions)のソースコードから拝借。
    /// </summary>
    /// <remarks>
    /// Copyright (c) .NET Foundation and Contributors
    /// All Rights Reserved
    /// Apache License Version 2.0
    /// http://www.apache.org/licenses/LICENSE-2.0
    /// </remarks>
    public static partial class EnumerableEx
    {
        // ForEach 072abb25ea23e47a49ccae7f1c3a1e4ec4a379e5 2017-01-25 master

        /// <summary>
        ///     Enumerates the sequence and invokes the given action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            foreach (var item in source)
                onNext(item);
        }

        /// <summary>
        ///     Enumerates the sequence and invokes the given action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            var i = 0;
            foreach (var item in source)
                onNext(item, checked(i++));
        }

        // IsEmpty a9a14360314c611be2d8411f6afeb61063d2e56b 2017-05-28 master

        /// <summary>
        ///     Determines whether an enumerable sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>true if the sequence is empty; false otherwise.</returns>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return !source.Any();
        }
    }
}
