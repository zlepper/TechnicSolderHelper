using System;
using System.Windows.Forms;
using TechnicSolderHelper.Properties;

namespace TechnicSolderHelper
{
    public static class Prompt
    {


        public static string ShowDialog(string text, string caption, Boolean showSkip = true,  String extraText = null)
        {
            Form prompt = new Form
            {
                Width = 500,
                Height = 180,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label { Left = 50, Top = 20, Text = text, Width = 350, Height = 80 };
            if (extraText != null)
            {
                Label extraLabel = new Label { Left = 20, Top = 110, Text = extraText, Width = 200 };
                prompt.Controls.Add(extraLabel);
            }
            TextBox textBox = new TextBox { Left = 50, Top = 80, Width = 400 };
            Button confirmation = new Button { Text = "Ok", Left = 350, Width = 100, Top = 110 };
            confirmation.Click += (sender, e) => prompt.Close();
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            Button skip = new Button { Text = "Skip", Left = 240, Width = 100, Top = 110, Visible = showSkip };
            skip.Click += (sender, e) =>
            {
                textBox.Text = @"skip";
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

        public static String ModsLeftString(short totalmods, short currentMod) {
            return String.Format("You are at mod {0} of {1} mods total", currentMod, totalmods);
        }
    }
}
