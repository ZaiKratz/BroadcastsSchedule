using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BroadcastsSchedule
{
    public partial class GoogleSpreadsheetAccount : Form
    {
        public GoogleSpreadsheetAccount()
        {
            InitializeComponent();
        }

        private void GoogleSpreadsheetAccount_Load(object sender, EventArgs e)
        {
            string[] lines = null;
            try
            {
                lines = System.IO.File.ReadAllLines(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\GoogleSpreadsheetAccount.txt");

                if (lines != null)
                    for (int i = 0; i < lines.Length; i++)
                        AccTextBox.AppendText(lines[i] + "\n");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string line = null;
            line = AccTextBox.Text;
            
            if (line != null)
            {
                System.IO.File.WriteAllText(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\GoogleSpreadsheetAccount.txt", "");
                System.IO.File.WriteAllText(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\GoogleSpreadsheetAccount.txt", line);
                MessageBox.Show("Saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private static bool Pred(String s)
        {
            return s == string.Empty;
        }
    }
}
