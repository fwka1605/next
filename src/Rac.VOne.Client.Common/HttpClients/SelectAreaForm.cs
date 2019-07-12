using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Web.Models.PcaModels;

namespace Rac.VOne.Client.Common.HttpClients
{
    public partial class SelectAreaForm : Form
    {
        public SelectAreaForm()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {

        }

        private void InitializeHandlers()
        {
            SelectButton.Click += OnSelectClick;
            AreasListBox.DoubleClick += OnSelectClick;
            CancelationButton.Click += OnCancelClick;
        }

        private void OnSelectClick(object sender, EventArgs e) => DialogResult = DialogResult.OK;

        private void OnCancelClick(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        public static BECommonDataArea SelectByGUI(IEnumerable<BECommonDataArea> areas)
        {
            BECommonDataArea result = null;
            using (var form = new SelectAreaForm())
            {
                form.AreasListBox.DataSource = new BindingSource(areas, null);
                var dialogResult = form.ShowDialog();
                if (dialogResult == DialogResult.OK)
                    result = form.AreasListBox.SelectedItem as BECommonDataArea;
            }
            return result;
        }
    }
}
