using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModpackHelper.GUI.Forms
{
    public class ReadOnlyRadioButton : RadioButton
    {
        protected override void OnClick(EventArgs e)
        {
            // pass the event up only if its not readonly
            if (!ReadOnly) base.OnClick(e);
        }

        public bool ReadOnly { get; set; }
    }
}
