using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.Editors;
using GrapeCity.Win.Editors.Fields;
using Rac.VOne.Client;

namespace Rac.VOne.Client.Common.Controls
{
    [LicenseProvider(typeof(LicenseProvider))]
    public class VOneNumberControl : GrapeCity.Win.Editors.GcNumber, IRequired
    {
        public VOneNumberControl()
            : base()
        {
            InitializeComponent();
        }

        public VOneNumberControl(IContainer container)
            : base(container)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AllowDeleteToNull = true;
            DropDown.AllowDrop = false;
            HighlightText = true;
            ImeMode = ImeMode.Disable;
            Spin.AllowSpin = false;
            MaxMinBehavior = MaxMinBehavior.AdjustToMaxMin;
        }

        protected override Dictionary<Keys, string> GetDefaultShortcuts()
        {
            return null;
        }

        protected override List<Type> GetDefaultSideButtonTypes()
        {
            return null;
        }

        protected override NumberDisplayField[] GetDefaultDisplayFields()
        {
            return base.GetDefaultDisplayFields();
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

        public event EventHandler RequiredChanged;
    }
}
