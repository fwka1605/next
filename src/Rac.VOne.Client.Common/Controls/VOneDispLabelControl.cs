using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Common.Controls
{
    [LicenseProviderAttribute(typeof(LicenseProvider))]
    public class VOneDispLabelControl : VOneTextControl
    {
        public VOneDispLabelControl()
            : base()
        {
            InitializeComponent();
        }

        public VOneDispLabelControl(IContainer container)
            : base(container)
        {
            InitializeComponent();
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            Enabled = false;
            ReadOnly = true;
        }


        protected override List<Type> GetDefaultSideButtonTypes()
        {
            return null;
        }

        protected override Dictionary<Keys, string> GetDefaultShortcuts()
        {
            return null;
        }

    }
}
