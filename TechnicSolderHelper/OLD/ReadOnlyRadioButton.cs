using System;
using System.Windows.Forms;

namespace TechnicSolderHelper.OLD
{
    public class ReadOnlyRadioButton : RadioButton
    {
        protected override void OnClick(EventArgs e)
        {
            // pass the event up only if its not readlonly
            if (!ReadOnly) base.OnClick(e);
        }

        public bool ReadOnly { get; set; }
    }
}
