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
            IDs = GoogleSheets.GetTablesIDs();
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
                try
                {
                    GoogleSheets.SetTablesIDs(EMailsIDTextBox.Text.ToString(), LecturesIDTextBox.Text.ToString());
                    MessageBox.Show("Saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Google.GoogleApiException ex)
                {
                    MessageBox.Show(ex.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
