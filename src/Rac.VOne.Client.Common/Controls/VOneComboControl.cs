using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.Editors;
using Rac.VOne.Client;

namespace Rac.VOne.Client.Common.Controls
{
    [LicenseProviderAttribute(typeof(LicenseProvider))]
    public class VOneComboControl : GrapeCity.Win.Editors.GcComboBox, IRequired
    {

        public VOneComboControl()
            : base()
        {
            InitializeComponent();
        }

        public VOneComboControl(IContainer container)
            : base(container)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            HighlightText = true;
            DropDown.AllowResize = false;
            ListHeaderPane.Visible = false;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyleEx.Flat;
            ImeMode = ImeMode.Disable;
        }

        protected override Dictionary<Keys, string> GetDefaultShortcuts()
        {
            return null;
        }

        private bool _required;

        public bool Required
        {
            get { return _required; }
            set
            {
                _required = value;
                RequiredChanged?.Invoke(this, new EventArgs());
            }
        }

        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }

        public event EventHandler RequiredChanged;
    }
}
