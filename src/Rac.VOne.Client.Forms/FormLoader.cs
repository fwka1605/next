using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Client.Common;
using System.Windows.Forms;
using System.Drawing;

namespace Rac.VOne.Client.Forms
{
    internal class FormLoader : IApplication
    {
        public ILogin Login { get; set; } = new Login();
        public string FontFamilyName { get; set; } = "Meiryo UI";
        private Control GetScreenControl(
            string dllName,
            string className)
        {
            var dir = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            var info = new System.IO.DirectoryInfo(dir);
            var path = info.GetFiles(dllName).Select(x => x.FullName).FirstOrDefault();
            var assembly = System.Reflection.Assembly.LoadFrom(path);
            var typeString = string.Concat(assembly.GetName().Name, ".", className);

            return assembly.CreateInstance(typeString) as Control;
        }

        public Form Create(string className)
        {
            const string DllName = "Rac.VOne.Client.Screen.dll";
            var form = new BasicForm();
            var baseContext = form as IFunctionKeys;
            var screen = GetScreenControl(DllName, className);
            var functionkeysSetter = screen as IFunctionKeysSetter;
            var applicationSetter = screen as IApplicationSetter;
            var closingMonthSetter = screen as IClosingMonthSetter;
            var statusMessageContext = screen as IMessageSetter;
            var colorContext = screen as IScreenColors;
            var screenNameContext = screen as IScreenNameSetter;

            form.SetHeaderContents(this);
            baseContext.SetCompoent(functionkeysSetter);
            functionkeysSetter.SetBaseContext(baseContext);
            applicationSetter.SetApplicationContext(this as IApplication);
            statusMessageContext.SetStatusMessageContext(form as IMessage);
            screenNameContext.SetScreenNameContext(form as IScreenName);

            screen.Dock = DockStyle.Fill;
            screen.Visible = true;
            form.pnlMain.Controls.Add(screen);
            closingMonthSetter?.SetClosing();

            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]))
            {
                form.MinimizeBox = false;
            }

            return form;
        }

        public DialogResult ShowDialog(IWin32Window owner, Form subForm, bool centerParent = false)
        {
            var ownerForm = owner as Form;
            if (ownerForm != null && (subForm is BasicForm || subForm is Screen.PA0201))
            {
                if (centerParent)
                {
                    subForm.StartPosition = FormStartPosition.CenterParent;
                }
                else
                {
                    subForm.StartPosition = FormStartPosition.Manual;
                    subForm.SetBounds(ownerForm.Location.X, ownerForm.Location.Y, ownerForm.Width, ownerForm.Height);
                }
            }
            return subForm.ShowDialog(owner);
        }

        public DialogResult ShowPreviewForm(IWin32Window owner, GrapeCity.ActiveReports.SectionReport report)
        {
            var previewForm = new frmVOnePreviewForm(report);
            var ownerForm = owner as Form;
            if (ownerForm != null)
            {
                previewForm.StartPosition = FormStartPosition.Manual;
                previewForm.SetBounds(ownerForm.Location.X, ownerForm.Location.Y, ownerForm.Width, ownerForm.Height);
            }
            return previewForm.ShowDialog(owner);
        }
    }
}
