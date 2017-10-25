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
    public partial class EditIDs : Form
    {
        public EditIDs()
        {
            InitializeComponent();
        }

        private void EditTablesID_Load(object sender, EventArgs e)
        {
            List<string> TablesIDs = null;
            TablesIDs = GoogleSheets.GetTablesIDs();
            string FolderID = GoogleDrive.YouTubePicturesFolderID;

            if (TablesIDs != null)
            {
                EMailsIDTextBox.Text = TablesIDs[0].ToString();
                LecturesIDTextBox.Text = TablesIDs[1].ToString();
            }
            if(FolderID != null)
            {
                FolderTextBox.Text = FolderID;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(EMailsIDTextBox.Text != string.Empty || LecturesIDTextBox.Text != string.Empty)
            {
                try
                {
                    GoogleSheets.SetTablesIDs(EMailsIDTextBox.Text.ToString(), LecturesIDTextBox.Text.ToString());
                    GoogleDrive.YouTubePicturesFolderID = FolderTextBox.Text.ToString();
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
