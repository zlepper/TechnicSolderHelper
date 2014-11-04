using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnicSolderHelper
{
    public static class Prompt
    {


        public static string ShowDialog(string text, string caption, Boolean showSkip,  String extraText)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 180;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 350, Height = 80 };
            if (extraText != null)
            {
                Label extraLabel = new Label { Left = 20, Top = 110, Text = extraText };
                prompt.Controls.Add(extraLabel);
            }
            TextBox textBox = new TextBox() { Left = 50, Top = 80, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 110 };
            confirmation.Click += (sender, e) =>
            {
                prompt.Close();
            };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            Button skip = new Button() { Text = "Skip", Left = 240, Width = 100, Top = 110, Visible = showSkip };
            skip.Click += (sender, e) =>
            {
                textBox.Text = "skip";
                prompt.Close();
            };
            prompt.Controls.Add(skip);
            if (showSkip)
            {
                prompt.CancelButton = skip;
            }
            prompt.ShowDialog();
            return textBox.Text;
        }

        public static string ShowDialog(string text, string caption)
        {
            return ShowDialog(text, caption, false, null);
        }

        public static string ShowDialog(string text, string caption, Boolean showSkip)
        {
            return ShowDialog(text, caption, true, null);
        }

        public static String modsLeftString(short totalmods, short modsLeft) {
            return String.Format("{0} of {1} mods total", modsLeft.ToString(), totalmods.ToString());
        }
    }
}
