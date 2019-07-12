using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Extensions;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class Dialog : Form, ILoggable, IApplicationUsable
    {
        public IApplication ApplicationContext { get; set; }
        public ApplicationControl ApplicationControl { get; set; }
        public Company CompanyInfo { get; set; }
        public new Form ParentForm { get { return TopLevelControl as Form; } }
        protected IColors ColorContext
        {
            get { return ColorSetting.Current; }
        }

        private XmlMessenger messenger = null;
        public XmlMessenger XmlMessenger
        {
            get { return messenger ?? new XmlMessenger(); }
            set { messenger = value; }
        }

        public Dialog()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyCode != Keys.Enter
                || ActiveControl is Button) return;

                var forward = !e.Shift;
                SelectNextControl(ActiveControl, forward, true, true, true);
            };
            ClearStatusMessage();
            statusStrip.Visible = false;
            statusStrip.DoubleClick += (sender, e) =>
            {
                if (!statusStrip.Visible
                || string.IsNullOrEmpty(statusMessageLabel.Text)) return;
                MessageBox.Show(statusMessageLabel.Text);
            };
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CP_NOCLOSE_BUTTON = 0x200;

                CreateParams p = base.CreateParams;
                p.ClassStyle |= CP_NOCLOSE_BUTTON;
                return p;
            }
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            if (ColorContext != null)
            {
                this.BackColor = ColorContext.FormBackColor;
                this.ForeColor = ColorContext.FormForeColor;
                this.InitializeColor(ColorContext);
            }
            if (ApplicationContext != null) this.InitializeFont(ApplicationContext.FontFamilyName);
            var isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
            Icon = (isCloudEdition) ? Properties.Resources.cloud_icon : Properties.Resources.app_icon;
        }

        #region display message IMessage implementations

        protected bool ShowConfirmDialog(string messageId, params string[] args)
        {
            if (!IsConfirmRequired) return true;

            var message = XmlMessenger.GetMessageInfo(messageId, args);
            var result = message.ShowMessageBox(ParentForm);
            this.Confirmed(result);
            return result == DialogResult.Yes;
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

        protected void DispStatusMessage(string messageId, params string[] args)
            => DispStatusMessage(XmlMessenger.GetMessageInfo(messageId, args));

        protected void DispStatusMessage(MessageInfo message)
        {
            if (message == null) return;
            DispStatusMessageInner(message.Text, message.Title, message.Color, message.DoBeep, message.Icon, message.Buttons);
        }

        private void DispStatusMessageInner(string message,
            string caption,
            Color backColor,
            bool doBeep,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            SetStatusMessage(message, caption, backColor, doBeep);
        }

        private void DispNotEnteredMessageInner(string message,
            string caption,
            Color backColor,
            bool doBeep,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            SetStatusMessage(message, caption, backColor, doBeep);
        }

        private void SetStatusMessage(string message, string caption, Color backColor, bool doBeep)
        {
            statusMessageLabel.Text = message;
            statusMessageLabel.BackColor = backColor;
            statusMessageLabel.ForeColor = Color.Red;
            if (doBeep) System.Media.SystemSounds.Beep.Play();
        }

        public void ClearStatusMessage()
        {
            statusMessageLabel.Text = "";
            statusMessageLabel.BackColor = Color.White;
        }

        #endregion

        #region ILoggableView
        string ILoggable.Caption => Text;

        ILogin ILoggable.Login => Login;

        protected ILogin Login => ApplicationContext?.Login;

        protected int CompanyId => Login?.CompanyId ?? 0;
        protected string SessionKey => Login?.SessionKey;
        /// <summary>
        ///  歩引利用
        /// </summary>
        protected bool UseDiscount => ApplicationControl?.UseDiscount == 1;

        /// <summary>
        ///  外貨利用
        /// </summary>
        protected bool UseForeignCurrency => ApplicationControl?.UseForeignCurrency == 1;

        /// <summary>
        ///  入金部門利用
        /// </summary>
        protected bool UseReceiptSection => ApplicationControl?.UseReceiptSection == 1;

        protected bool IsWarningRequired => CompanyInfo?.ShowWarningDialog == 1;
        protected bool IsConfirmRequired => CompanyInfo?.ShowConfirmDialog == 1;
        #endregion

        #region IApplicationUsable
        IApplication IApplicationUsable.ApplicationContext => ApplicationContext;
        #endregion
    }
}
