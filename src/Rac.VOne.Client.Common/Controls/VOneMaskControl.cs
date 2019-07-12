using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Client;

namespace Rac.VOne.Client.Common.Controls
{
    [LicenseProviderAttribute(typeof(LicenseProvider))]
    public class VOneMaskControl : GrapeCity.Win.Editors.GcMask, IRequired
    {
        public VOneMaskControl()
            : base()
        {
            InitializeComponent();
        }

        public VOneMaskControl(IContainer container)
            : base(container)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {

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
