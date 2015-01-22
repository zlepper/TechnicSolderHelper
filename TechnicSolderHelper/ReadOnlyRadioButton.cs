using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicSolderHelper
{
    public class ReadOnlyRadioButton : System.Windows.Forms.RadioButton
    {
        protected override void OnClick(EventArgs e)
        {
            // pass the event up only if its not readlonly
            if (!ReadOnly) base.OnClick(e);
        }

        public bool ReadOnly { get; set; }
    }
}
