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
    public partial class Accounts : Form
    {
        public Accounts()
        {
            InitializeComponent();
        }

        private void Accounts_Load(object sender, EventArgs e)
        {
            string[] lines = null;
            lines = System.IO.File.ReadAllLines(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt");

            if(lines != null)
                for (int i = 0; i < lines.Length; i++)
                    textBox1.AppendText(lines[i] + "\n");

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            List<string> lines = null;
            lines = textBox1.Text.Split('\n').ToList();
            lines.RemoveAll(Pred);
            if (lines != null)
            {
                System.IO.File.WriteAllLines(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt", lines);
                MessageBox.Show("Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("No lines added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static bool Pred(String s)
        {
            return s == string.Empty;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
