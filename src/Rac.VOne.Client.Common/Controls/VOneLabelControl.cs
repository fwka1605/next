using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common.Controls
{
    public class VOneLabelControl : System.Windows.Forms.Label
    {
        public VOneLabelControl()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {

        }

        [DefaultValue(false)]
        public new bool UseMnemonic { get; set; }

    }
}
