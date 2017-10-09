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
    public partial class EditTablesID : Form
    {
        public EditTablesID()
        {
            InitializeComponent();
        }

        private void EditTablesID_Load(object sender, EventArgs e)
        {
            List<string> IDs = null;
            IDs = Sheets.GetTablesIDs();
            if (IDs != null)
            {
                EMailsIDTextBox.Text = IDs[0].ToString();
                LecturesIDTextBox.Text = IDs[1].ToString();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(EMailsIDTextBox.Text != string.Empty || LecturesIDTextBox.Text != string.Empty)
            {
                Sheets.SetTablesIDs(EMailsIDTextBox.Text.ToString(), LecturesIDTextBox.Text.ToString());
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
