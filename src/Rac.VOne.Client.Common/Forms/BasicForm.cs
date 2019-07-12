using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client;

namespace Rac.VOne.Client.Common
{
    public partial class BasicForm : Form, IFunctionKeys, IMessage, IColors, IScreenName
    {
        public IFunctionKeysSetter ComponentContext { get; set; }
        public BasicForm()
            : base()
        {
            InitializeComponent();
            KeyPreview = true;

            var isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
            Icon = (isCloudEdition) ? Properties.Resources.cloud_icon : Properties.Resources.app_icon;
        }

        /// <summary>
        /// 共通ヘッダの表示内容をセットする。
        /// </summary>
        /// <param name="applicationContext"></param>
        public void SetHeaderContents(IApplication applicationContext)
        {
            if (applicationContext == null || applicationContext.Login == null)
            {
                lblCompanyName.Text = "";
                lblUserName.Text = "";
                return;
            }

            var login = applicationContext.Login;
            lblCompanyName.Text = $"【{login.CompanyCode}】 {login.CompanyName}";
            lblUserName.Text = login.UserName;

            lblCompanyName.Location = new Point(pnlHeader.Width - (lblCompanyName.Width + 12), lblCompanyName.Top);
            lblUserName.Location = new Point(pnlHeader.Width - (lblUserName.Width + 12), lblUserName.Top);
        }

        /// <summary>締済年月の表示</summary>
        public void SetClosingInformation(Web.Models.ClosingInformation information)
        {
            if (information.UseClosing)
                lblClosingMonth.Text = information.ClosingDisplay;
            else
                lblClosingMonth.Visible = false;
        }

        public void SetCompoent(IFunctionKeysSetter context)
        {
            ComponentContext = context;
            SetComponentHandler();
        }

        private void SetComponentHandler()
        {
            if (ComponentContext == null) return;
            btnF01.Click += (sender, e) => ComponentContext.OnFunctionKey01Click();
            btnF02.Click += (sender, e) => ComponentContext.OnFunctionKey02Click();
            btnF03.Click += (sender, e) => ComponentContext.OnFunctionKey03Click();
            btnF04.Click += (sender, e) => ComponentContext.OnFunctionKey04Click();
            btnF05.Click += (sender, e) => ComponentContext.OnFunctionKey05Click();
            btnF06.Click += (sender, e) => ComponentContext.OnFunctionKey06Click();
            btnF07.Click += (sender, e) => ComponentContext.OnFunctionKey07Click();
            btnF08.Click += (sender, e) => ComponentContext.OnFunctionKey08Click();
            btnF09.Click += (sender, e) => ComponentContext.OnFunctionKey09Click();
            btnF10.Click += (sender, e) => ComponentContext.OnFunctionKey10Click();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1: if (btnF01.Enabled) { btnF01.Focus(); ComponentContext.OnFunctionKey01Click(); } return true;
                case Keys.F2: if (btnF02.Enabled) { btnF02.Focus(); ComponentContext.OnFunctionKey02Click(); } return true;
                case Keys.F3: if (btnF03.Enabled) { btnF03.Focus(); ComponentContext.OnFunctionKey03Click(); } return true;
                case Keys.F4: if (btnF04.Enabled) { btnF04.Focus(); ComponentContext.OnFunctionKey04Click(); } return true;
                case Keys.F5: if (btnF05.Enabled) { btnF05.Focus(); ComponentContext.OnFunctionKey05Click(); } return true;
                case Keys.F6: if (btnF06.Enabled) { btnF06.Focus(); ComponentContext.OnFunctionKey06Click(); } return true;
                case Keys.F7: if (btnF07.Enabled) { btnF07.Focus(); ComponentContext.OnFunctionKey07Click(); } return true;
                case Keys.F8: if (btnF08.Enabled) { btnF08.Focus(); ComponentContext.OnFunctionKey08Click(); } return true;
                case Keys.F9: if (btnF09.Enabled) { btnF09.Focus(); ComponentContext.OnFunctionKey09Click(); } return true;
                case Keys.F10: if (btnF10.Enabled) { btnF10.Focus(); ComponentContext.OnFunctionKey10Click(); } return true;
            }
            return false;
        }

        public void SetFunction01Enabled(bool enabled) { btnF01.Enabled = enabled; }
        public void SetFunction02Enabled(bool enabled) { btnF02.Enabled = enabled; }
        public void SetFunction03Enabled(bool enabled) { btnF03.Enabled = enabled; }
        public void SetFunction04Enabled(bool enabled) { btnF04.Enabled = enabled; }
        public void SetFunction05Enabled(bool enabled) { btnF05.Enabled = enabled; }
        public void SetFunction06Enabled(bool enabled) { btnF06.Enabled = enabled; }
        public void SetFunction07Enabled(bool enabled) { btnF07.Enabled = enabled; }
        public void SetFunction08Enabled(bool enabled) { btnF08.Enabled = enabled; }
        public void SetFunction09Enabled(bool enabled) { btnF09.Enabled = enabled; }
        public void SetFunction10Enabled(bool enabled) { btnF10.Enabled = enabled; }
        public void SetFunction11Enabled(bool enabled) { }
        public void SetFunction12Enabled(bool enabled) { }

        public void SetFunction01Caption(string caption) { btnF01.Text = "F1/" + caption; }
        public void SetFunction02Caption(string caption) { btnF02.Text = "F2/" + caption; }
        public void SetFunction03Caption(string caption) { btnF03.Text = "F3/" + caption; }
        public void SetFunction04Caption(string caption) { btnF04.Text = "F4/" + caption; }
        public void SetFunction05Caption(string caption) { btnF05.Text = "F5/" + caption; }
        public void SetFunction06Caption(string caption) { btnF06.Text = "F6/" + caption; }
        public void SetFunction07Caption(string caption) { btnF07.Text = "F7/" + caption; }
        public void SetFunction08Caption(string caption) { btnF08.Text = "F8/" + caption; }
        public void SetFunction09Caption(string caption) { btnF09.Text = "F9/" + caption; }
        public void SetFunction10Caption(string caption) { btnF10.Text = "F10/" + caption; }
        public void SetFunction11Caption(string caption) { }
        public void SetFunction12Caption(string caption) { }

        public void DispStatusMessage(string message, string caption, Color backColor, bool doBeep, MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            SetStatusMessage(message, caption, backColor, doBeep);
        }

        public void DispNotEnteredMessage(string message, string caption, Color backColor, bool doBeep, MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            SetStatusMessage(message, caption, backColor, doBeep);
        }

        private void SetStatusMessage(string message, string caption, Color backColor, bool doBeep)
        {
            statusMessageLabel.Text = message.Replace("\r", "").Replace("\n", "");
            statusMessageLabel.BackColor = backColor;
            statusMessageLabel.ForeColor = Color.Red;
            if (doBeep) System.Media.SystemSounds.Beep.Play();
        }

        public void ClearStatusMessage()
        {
            statusMessageLabel.Text = "";
            statusMessageLabel.BackColor = Color.White;
        }

        public Color FormBackColor { get; set; }
        public Color FormForeColor { get; set; }
        public Color ControlEnableBackColor { get; set; }
        public Color ControlDisableBackColor { get; set; }
        public Color ControlForeColor { get; set; }
        public Color ControlRequiredBackColor { get; set; }
        public Color ControlActiveBackColor { get; set; }
        public Color ButtonBackColor { get; set; }
        public Color GridRowBackColor { get; set; }
        public Color GridAlternatingRowBackColor { get; set; }
        public Color GridLineColor { get; set; }
        public Color InputGridBackColor { get; set; }
        public Color InputGridAlternatingBackColor { get; set; }
        public Color MatchingGridBillingBackColor { get; set; }
        public Color MatchingGridReceiptBackColor { get; set; }
        public Color MatchingGridBillingSelectedRowBackColor { get; set; }
        public Color MatchingGridBillingSelectedCellBackColor { get; set; }
        public Color MatchingGridReceiptSelectedRowBackColor { get; set; }
        public Color MatchingGridReceiptSelectedCellBackColor { get; set; }
        public Color CollationDupedReceiptCellBackColor { get; set; }

        public void SetScreenName(string name)
        {
            this.Text = name;
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
    }
}
