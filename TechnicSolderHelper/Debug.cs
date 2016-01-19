using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnicSolderHelper
{
    public class Debug
    {
        private static readonly string Output =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                "DebugFromModpackHelper.txt");
        private static readonly StringBuilder sb = new StringBuilder();
        private static CheckBox _box;

        public static void AssignCheckbox(CheckBox b)
        {
            _box = b;
        }

        public static void WriteLine(string text, bool condition = false)
        {
            System.Diagnostics.Debug.WriteLine(text);
            if (condition || _box != null && _box.Checked)
            {
                sb.AppendLine(text);
            }
        }

        public static void WriteLine(object o, bool condition = false)
        {
            if(o != null)
                WriteLine(o.ToString(), condition);
        }

        public static void Save()
        {
            if (!string.IsNullOrWhiteSpace(sb.ToString()))
                File.AppendAllText(Output, sb.ToString());
            sb.Clear();
        }
    }
}
