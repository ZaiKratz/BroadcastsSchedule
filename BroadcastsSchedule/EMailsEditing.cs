﻿using System;
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
    public partial class EMailsEditing : Form
    {
        private Google.Apis.Sheets.v4.SheetsService SheetService = null;
        public EMailsEditing()
        {
            InitializeComponent();
            SheetService = GoogleSheets.AuthenticateOauth("3dmaya.com.ua@gmail.com");
        }

        private void EMailsEditing_Load(object sender, EventArgs e)
        {
            var EMails = GoogleSheets.GetEMails(SheetService, Program.BSForm.GetCurrentCourse());

            foreach (var Item in EMails)
                foreach (var Value in Item)
                    EMailsList.AppendText(Value.ToString() + "\n");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            List<IList<object>> EMails = new List<IList<object>>();
            string TMP = EMailsList.Text.Remove(EMailsList.Text.Length -1);
            EMails.Add(TMP.Split('\n'));
            try
            {
                GoogleSheets.EditEmailsSheet(GoogleSheets.AuthenticateOauth(Program.BSForm.GetCurrentUser()), Program.BSForm.GetCurrentCourse(), EMails);
            }
            catch(Google.GoogleApiException ex)
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
}
