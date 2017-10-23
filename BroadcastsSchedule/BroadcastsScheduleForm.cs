using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;


namespace BroadcastsSchedule
{
    public partial class BroadcastsScheduleClass : Form
    {
        private Google.Apis.YouTube.v3.Data.LiveBroadcast CurrentBroadcast = null;
        private Google.Apis.YouTube.v3.Data.LiveStream CurrentStream = null;
        private Google.Apis.YouTube.v3.YouTubeService YouTubeService = null;
        private Google.Apis.Sheets.v4.SheetsService SheetService = null;
        private string User = null;

        public BroadcastsScheduleClass()
        {
            InitializeComponent();
            SheetService = GoogleSheets.AuthenticateOauth("3dmaya.com.ua@gmail.com");
        }

        
        private void UpdateData()
        {
            UpdateLectureListAsync();
            UpdateEmailsListAsync();
        }

        private void UpdateCoursesList()
        {
            List<List<object>> CoursesList = null;
            if (SheetService != null)
                CoursesList = GoogleSheets.GetCourses(SheetService);

            if (CoursesList != null)
            {
                if (Courses_List.Items.Count > 0)
                    Courses_List.Items.Clear();

                foreach (var Item in CoursesList)
                    Courses_List.Items.AddRange(Item.ToArray());

                Courses_List.SelectedIndex = 0;
            }
        }

        private void UpdateLectureListAsync()
        {
            UpdateList.BeginInvoke((MethodInvoker)delegate () { UpdateList.Enabled = false; });
            var SelectedItem = "";
            Courses_List.Invoke(
                new Action(
                    delegate ()
                    {
                        SelectedItem = Courses_List.SelectedItem.ToString();
                    }
                )
            );
            var LecturesList = GoogleSheets.GetLectures(SheetService, SelectedItem);

            var LecturesCount = 0;
            LecturesGrid.Invoke(
                new Action(
                    delegate ()
                    {
                        LecturesCount = LecturesGrid.Rows.Count;
                    }
                )
            );
            if (LecturesCount > 0)
                LecturesGrid.Invoke(
                    new Action(
                        delegate ()
                        {
                            LecturesGrid.Rows.Clear();
                        }
                    )
            );

            foreach (var Item in LecturesList)
            {
                var date = DateTime.ParseExact(Item[1].ToString(), "dd.MM", null);
                var time = DateTime.Parse(Item[2].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                LecturesGrid.Invoke(
                                new Action(
                                    delegate ()
                                    {
                                        LecturesGrid.Rows.Add(
                                            Item[0].ToString(), 
                                            date,
                                            time, 
                                            Item[3].ToString());
                                    }
                                )
                            );
            }
        }

        private void UpdateEmailsListAsync()
        {
            var SelIndex = 0;
            Courses_List.Invoke(
                new Action(
                    delegate ()
                    {
                        SelIndex = Courses_List.SelectedIndex;
                    }
                )
            );

            var EMailList = GoogleSheets.GetEMails(SheetService, SelIndex);
            var EmailsCount = 0;
            EMails_List.Invoke(
                new Action(
                    delegate ()
                    {
                        EmailsCount = EMails_List.Items.Count;
                    }
                )
            );

            if (EmailsCount > 0)
                EMails_List.Invoke(
                    new Action(
                        delegate ()
                        {
                            EMails_List.Items.Clear();
                        }
                    )
                );

            if(EMailList != null)
            {
                foreach (var Item in EMailList)
                    EMails_List.Invoke(
                        new Action(
                            delegate ()
                            {
                                EMails_List.Items.AddRange(Item.ToArray());
                            }
                        )
                    );
            }

            UpdateList.BeginInvoke((MethodInvoker)delegate () { UpdateList.Enabled = true; });
            CopyToClipboardButton.BeginInvoke((MethodInvoker)delegate () { CopyToClipboardButton.Enabled = true; });
        }

        private void CoursesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            UpdateData();
        }
        
        private void UpdateList_Click(object sender, EventArgs e)
        {
            string[] lines = null;
            lines = System.IO.File.ReadAllLines(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt");
            if (lines != null)
            {
                if (Accounts_List.Items.Count == 0)
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] != string.Empty)
                            Accounts_List.Items.Add(lines[i]);
                    }
                else
                {
                    Accounts_List.Items.Clear();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] != string.Empty)
                            Accounts_List.Items.Add(lines[i]);
                    }
                }
                Accounts_List.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No Accounts in " + System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt" + " file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateCoursesList();
            UpdateData();
        }

        private void StartEvent_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();        
        }            

        private void CreateLiveEventAsync()
        {
            Courses_List.BeginInvoke((MethodInvoker)delegate () { Courses_List.Enabled = false; });
            UpdateList.BeginInvoke((MethodInvoker)delegate () { UpdateList.Enabled = false; });

            StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.Enabled = false; });
            StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.BackColor = System.Drawing.Color.LightGray; });

            CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.Enabled = true; });
            CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.BackColor = System.Drawing.Color.Red; });
            SetLabelText("Creating YouTube Live Broadcast...");

            string Stream = "";
            string Name = "";
            string Description = "";
            DateTime Now = DateTime.Now;

            if (LecturesGrid.InvokeRequired)
            {
                LecturesGrid.Invoke(
                    new Action(
                        delegate ()
                        {
                            Stream = Courses_List.SelectedItem.ToString();
                        }
                    )
                );
                LecturesGrid.Invoke(
                    new Action(
                        delegate ()
                        {
                            Name = LecturesGrid.Rows[LecturesGrid.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                        }
                    )
                );
                LecturesGrid.Invoke(
                    new Action(
                        delegate ()
                        {
                            Description = LecturesGrid.Rows[LecturesGrid.SelectedCells[0].RowIndex].Cells[3].Value.ToString();
                        }
                    )
                );

                DateTime Date = DateTime.Now;
                LecturesGrid.Invoke(
                    new Action(
                        delegate ()
                        {
                            Date = DateTime.Parse(LecturesGrid.Rows[LecturesGrid.SelectedCells[0].RowIndex].Cells[1].Value.ToString());
                        }
                    )
                );

                DateTime Time = DateTime.Now;
                LecturesGrid.Invoke(
                    new Action(
                        delegate ()
                        {
                            Time = DateTime.Parse(LecturesGrid.Rows[LecturesGrid.SelectedCells[0].RowIndex].Cells[2].Value.ToString());
                        }
                    )
                );

                Now = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second).ToUniversalTime();
            }

            CurrentBroadcast = GoogleYouTube.CreateLiveEvent(
                            YouTubeService,
                            Stream,
                            Name,
                            Now,
                            Description);


            if(CurrentBroadcast != null)
            {
                CurrentStream = GoogleYouTube.GetStreamByTitle(Stream, YouTubeService);
                if (CurrentBroadcast != null)
                {
                    CurrentBroadcast = GoogleYouTube.StartEvent(YouTubeService, CurrentBroadcast.Id, CurrentStream.Id);
                    if(CurrentBroadcast != null)
                    {
                        EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.Enabled = true; });
                        EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.BackColor = System.Drawing.Color.Red; });

                        CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.Enabled = false; });
                        CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; });
                    }
                }
            }

        }

        private void EndLiveEventButton_Click(object sender, EventArgs e)
        {
            CurrentBroadcast = GoogleYouTube.EndEvent(YouTubeService, CurrentBroadcast.Id);

            if (CurrentBroadcast != null)
            {
                StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.Enabled = true; });
                StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.BackColor = System.Drawing.Color.LimeGreen; });

                EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.Enabled = false; });
                EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.BackColor = System.Drawing.Color.LightGray; });

                Courses_List.BeginInvoke((MethodInvoker)delegate () { Courses_List.Enabled = true; });
                UpdateList.BeginInvoke((MethodInvoker)delegate () { UpdateList.Enabled = true; });

                CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.Enabled = false; });
                CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; });
                SetLabelText("Stream is complete. Don't forget to stop OBS stream.");
            }
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CreateLiveEventAsync();
        }

        private void CancelEventButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy == true)
            {
                backgroundWorker.Abort();
                backgroundWorker.Dispose();

                if (CurrentBroadcast != null)
                {
                    if(GoogleYouTube.DeleteEvent(YouTubeService, CurrentBroadcast.Id))
                    {
                        CurrentBroadcast = null;
                        CurrentStream = null;
                    }
                    
                }
            }
            if (CurrentBroadcast == null)
            {
                StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.Enabled = true; });
                StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.BackColor = System.Drawing.Color.LimeGreen; });

                EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.Enabled = false; });
                EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.BackColor = System.Drawing.Color.LightGray; });

                Courses_List.BeginInvoke((MethodInvoker)delegate () { Courses_List.Enabled = true; });
                UpdateList.BeginInvoke((MethodInvoker)delegate () { UpdateList.Enabled = true; });

                CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.Enabled = false; });
                CancelEventButton.BeginInvoke((MethodInvoker)delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; });
                SetLabelText("Canceled by user");
            }

        }

        public string GetCurrentUser()
        {
            return User;
        }

        public int GetCurrentCourse()
        {
            int SelIndex = 0;
            Courses_List.Invoke(
                new Action(
                    delegate ()
                    {
                        SelIndex = Courses_List.SelectedIndex;
                    }
                )
            );
            return SelIndex;
        }

        public void SetLabelText(string Text)
        {
            label4.BeginInvoke((MethodInvoker)delegate () { label4.Text = Text; });
        }

        private void BroadcastsScheduleClass_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentBroadcast != null)
            {
                if (CurrentBroadcast.Status.LifeCycleStatus.ToLower() != "complete")
                {
                    if (MessageBox.Show("You are streaming right now. Do you want to stop stream?", "Attention", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void BroadcastsScheduleClass_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(CurrentBroadcast != null)
            {
                if (backgroundWorker.IsBusy)
                {
                    backgroundWorker.Abort();
                    backgroundWorker.Dispose();
                    SetLabelText("Canceled by user");
                }

                var LiveCycleStatus = CurrentBroadcast.Status.LifeCycleStatus.ToLower();

                switch (LiveCycleStatus)
                {
                    case "abandoned":
                    case "created":
                    case "ready":
                    case "teststarting":
                    case "testing":
                        GoogleYouTube.DeleteEvent(YouTubeService, CurrentBroadcast.Id);
                        break;
                    case "live":
                    case "livestarting":
                    case "reclaimed":
                        GoogleYouTube.EndEvent(YouTubeService, CurrentBroadcast.Id);
                        break;
                }

                Application.Exit();
            }
            else
                Application.Exit();
        }

        private void CopyToClipboardButton_Click(object sender, EventArgs e)
        {
            string Items = string.Empty;
            foreach (var Item in EMails_List.Items)
                Items += Item.ToString() + "\n";

            Items = Items.Remove(Items.Length - 1);
            Clipboard.SetText(Items);
        }

        private void editEmailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EMailsEditing EMails = new EMailsEditing();
            //EMails.ShowDialog();
        }

        private void BroadcastsScheduleClass_Load(object sender, EventArgs e)
        {
            string[] lines = null;
            lines = System.IO.File.ReadAllLines(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt");
            if (lines != null)
                if (Accounts_List.Items.Count == 0)
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] != string.Empty)
                            Accounts_List.Items.Add(lines[i]);
                    }
                else
                {
                    Accounts_List.Items.Clear();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] != string.Empty)
                            Accounts_List.Items.Add(lines[i]);
                    }
                }
            else
            {
                MessageBox.Show("No Accounts in " + System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt" + " file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SetLabelText("Select lecture and press \"Start Stream\" button");
            Accounts_List.SelectedIndex = 0;
            EndEventButton.Enabled = false;
            EndEventButton.BackColor = System.Drawing.Color.LightGray;

            CopyToClipboardButton.Enabled = false;
            CancelEventButton.Enabled = false;
            CancelEventButton.BackColor = System.Drawing.Color.LightGray;

            LecturesGrid.Columns.Add("LecturesColumn", "Lectures");

            CalendarColumn Date = new CalendarColumn();
            Date.HeaderText = "Date";
            TimeColumn Time = new TimeColumn();
            Time.HeaderText = "Time";

            LecturesGrid.Columns.Add(Date);
            LecturesGrid.Columns.Add(Time);

            LecturesGrid.Columns.Add("DescriptionColumn", "Description");

            UpdateCoursesList();
        }

        private void Accounts_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            User = Accounts_List.SelectedItem.ToString();
            try
            {
                YouTubeService = GoogleYouTube.AuthenticateOauth(User);
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

        private void editTablesIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditTablesID Edit = new EditTablesID();
            Edit.ShowDialog();
        }

        private void editAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accounts Acc = new Accounts();
            Acc.ShowDialog();
        }
    }

    public class AbortableBackgroundWorker : BackgroundWorker
    {
        private Thread workerThread;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            workerThread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true; //We must set Cancel property to true!
                Thread.ResetAbort(); //Prevents ThreadAbortException propagation
            }
        }


        public void Abort()
        {
            if (workerThread != null)
            {
                workerThread.Abort();
                workerThread = null;
            }
        }
    }
}
