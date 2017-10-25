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
        private string User = null;

        private static object AuthLocker = new object();
        private static object LecturesLocker = new object();
        private static object CoursesLocker = new object();
        private static object EmailsLocker = new object();

        private Thread AuthServisesThread = null;

        public BroadcastsScheduleClass()
        {
            InitializeComponent();

            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(UpdateButton, "Update All");
        }

        private void BroadcastsScheduleClass_Load(object sender, EventArgs e)
        {
            
            AuthServisesThread = new Thread(Authenticate);
            AuthServisesThread.Start();
            

            UpdateAccountsList();

            SetStatus("Select lecture and press \"Start Stream\" button");
            if(Accounts_List.Items.Count > 0)
                Accounts_List.SelectedIndex = 0;
            EndEventButton.Enabled = false;
            EndEventButton.BackColor = System.Drawing.Color.LightGray;

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

            //UpdateCoursesList();
        }

        private void CoursesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }
        
        private void UpdateList_Click(object sender, EventArgs e)
        {
            UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = false; }));
            Courses_List.Invoke(new Action(delegate () { Courses_List.Enabled = false; }));
            Accounts_List.Invoke(new Action(delegate () { Accounts_List.Enabled = false; }));
            if (AuthServisesThread.IsAlive)
            {
                AuthServisesThread.Abort();
                if (Accounts_List.Enabled == false)
                {
                    Accounts_List.Enabled = true;
                    UpdateAccountsList();
                    if(Accounts_List.Items.Count > 0)
                        Accounts_List.SelectedIndex = 0;
                }                
            }
            else
            {
                UpdateAccountsList();
                if (Accounts_List.Items.Count > 0)
                    Accounts_List.SelectedIndex = 0;
            }
        }

        private void StartLiveEvent_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();        
        }

        private void EndLiveEventButton_Click(object sender, EventArgs e)
        {
            EndLiveEvent();
        }

        private void CancelLiveEventCreationButton_Click(object sender, EventArgs e)
        {
            CancelLiveEventCreation();
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CreateLiveEventAsync();
        }

        private void BroadcastsScheduleClass_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(AuthServisesThread.IsAlive)
            {
                AuthServisesThread.Abort();
            } 

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
                    SetStatus("Canceled by user");
                }

                var LiveCycleStatus = CurrentBroadcast.Status.LifeCycleStatus.ToLower();

                switch (LiveCycleStatus)
                {
                    case "abandoned":
                    case "created":
                    case "ready":
                    case "teststarting":
                    case "testing":
                        GoogleYouTube.DeleteEvent(CurrentBroadcast.Id);
                        break;
                    case "live":
                    case "livestarting":
                    case "reclaimed":
                        GoogleYouTube.EndEvent(CurrentBroadcast.Id);
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
            if(Items != string.Empty)
            {
                Items = Items.Remove(Items.Length - 1);
                Clipboard.SetText(Items);
            }
        }

        private void editEmailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EMailsEditing EMails = new EMailsEditing();
            //EMails.ShowDialog();
        }

        private void Accounts_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllData();
            User = Accounts_List.SelectedItem.ToString();
            if (!AuthServisesThread.IsAlive)
            {
                AuthServisesThread = new Thread(Authenticate);
                AuthServisesThread.Start();
            }

        }

        private void editTablesIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditIDs Edit = new EditIDs();
            Edit.ShowDialog();
        }

        private void editAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accounts Acc = new Accounts();
            Acc.ShowDialog();
        }

        private void BroadcastsScheduleClass_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                NotifyIcon.Visible = true;
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NotifyIcon.Visible = false;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void BroadcastSettingsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/my_live_events?event_id=" + CurrentBroadcast.Id + "&action_edit_live_event=1");
        }

        private void CancelAuth_Click(object sender, EventArgs e)
        {
            if(AuthServisesThread.IsAlive)
            {
                AuthServisesThread.Abort();
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                Courses_List.Invoke(new Action(delegate () { Courses_List.Enabled = true; }));
                Accounts_List.Invoke(new Action(delegate () { Accounts_List.Enabled = true; }));
                CancelAuth.Invoke(new Action(delegate () { CancelAuth.Visible = false; }));
            }
        }


        private void Authenticate()
        {
            lock (AuthLocker)
            {
                CancelAuth.Invoke(new Action(delegate () { CancelAuth.Visible = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = false; }));
                Courses_List.Invoke(new Action(delegate () { Courses_List.Enabled = false; }));
                StartEventButton.Invoke(
                    new Action(
                        delegate ()
                        {
                            StartEventButton.Enabled = false;
                            StartEventButton.BackColor = System.Drawing.Color.LightGray;
                        }
                    )
                );
                //StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LightGray; });

                Accounts_List.Invoke(
                    new Action(
                        delegate ()
                        {
                            Accounts_List.Enabled = false;
                        }
                    )
                );

                GoogleSheets.AuthenticateOauth("3dmaya.com.ua@gmail.com").Wait();
                GoogleYouTube.AuthenticateOauth(User).Wait();

                Accounts_List.Invoke(
                    new Action(
                        delegate ()
                        {
                            Accounts_List.Enabled = true;
                        }
                    )
                );

                UpdateCoursesList();
                UpdateData();
                CancelAuth.Invoke(new Action(delegate () { CancelAuth.Visible = false; }));
                Thread.Sleep(200);
            }
        }

        private void UpdateData()
        {
            UpdateLectureListAsync();
            UpdateEmailsListAsync();
        }

        private void UpdateCoursesList()
        {
            //lock(CoursesLocker)
            {
                List<List<object>> CoursesList = null;
                CoursesList = GoogleSheets.GetCourses();

                if (CoursesList != null)
                {
                    if (Courses_List.Items.Count > 0)
                        Courses_List.Invoke(
                                        new Action(
                                            delegate ()
                                            {
                                                Courses_List.Items.Clear();
                                            }
                                        )
                                    );

                    foreach (var Item in CoursesList)
                        Courses_List.Invoke(
                                    new Action(
                                        delegate ()
                                        {
                                            Courses_List.Items.AddRange(Item.ToArray());
                                        }
                                    )
                                );

                    Courses_List.Invoke(
                                new Action(
                                    delegate ()
                                    {
                                        Courses_List.SelectedIndex = 0;
                                    }
                                )
                            );
                }
            }
        }

        private void UpdateLectureListAsync()
        {
            //lock (LecturesLocker)
            {
                var SelectedItem = "";

                if (Courses_List.Items.Count != 0)
                {
                    Courses_List.Invoke(
                                    new Action(
                                        delegate ()
                                        {
                                            SelectedItem = Courses_List.SelectedItem.ToString();
                                        }
                                    )
                                );
                    var LecturesList = GoogleSheets.GetLectures(SelectedItem);

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

                    if (LecturesList.Count > 0)
                    {
                        StartEventButton.Invoke(
                                        new Action(
                                            delegate ()
                                            {
                                                StartEventButton.Enabled = true;
                                                StartEventButton.BackColor = System.Drawing.Color.LimeGreen;
                                            }
                                        )
                                    );
                    }
                    else
                    {
                        StartEventButton.Invoke(
                                        new Action(
                                            delegate ()
                                            {
                                                StartEventButton.Enabled = false;
                                                StartEventButton.BackColor = System.Drawing.Color.LightGray;
                                            }
                                        )
                                    );
                    }

                    foreach (var Item in LecturesList)
                    {
                        string tmpdate = Item.ElementAtOrDefault(1).ToString();
                        DateTime Date = DateTime.MinValue;

                        string tmptime = Item.ElementAtOrDefault(2).ToString();
                        DateTime Time = DateTime.MinValue;

                        if (tmpdate != string.Empty)
                            Date = DateTime.ParseExact(
                               Item.ElementAtOrDefault(1).ToString(),
                               "dd.MM",
                               null);

                        if (tmptime != string.Empty)
                            Time = DateTime.Parse(
                            Item.ElementAtOrDefault(2).ToString(), 
                            System.Globalization.CultureInfo.InvariantCulture);

                        LecturesGrid.Invoke(
                                    new Action(
                                        delegate ()
                                        {
                                            LecturesGrid.Rows.Add(
                                                Item.ElementAtOrDefault(0).ToString(),
                                                Date,
                                                Time,
                                                Item.ElementAtOrDefault(3).ToString());
                                        }
                                    )
                                );
                    }
                }
            }
        }

        private void UpdateEmailsListAsync()
        {
            //lock (EmailsLocker)
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

                var EMailList = GoogleSheets.GetEMails(SelIndex);
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

                if (EMailList != null)
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
  
                Courses_List.Invoke(new Action(delegate () { Courses_List.Enabled = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
            }
        }

        private void CreateLiveEventAsync()
        {
            Courses_List.Invoke(new Action(delegate () { Courses_List.Enabled = false; }));
            UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = false; }));
            Accounts_List.Invoke(new Action(delegate () { Accounts_List.Enabled = false; }));

            StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = false; }));
            StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LightGray; }));

            CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = true; }));
            CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.Red; }));
            SetStatus("Creating YouTube Live Broadcast...");

            string StreamTitle = "";
            string Name = "";
            string Description = "";
            DateTime ScheduledDateTime = DateTime.Now;


            LecturesGrid.Invoke(
                new Action(
                    delegate ()
                    {
                        StreamTitle = Courses_List.SelectedItem.ToString();
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

            DateTime Date = DateTime.MinValue;
            LecturesGrid.Invoke(
                new Action(
                    delegate ()
                    {
                        string tmp = LecturesGrid.Rows[LecturesGrid.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                        if(tmp != null)
                            Date = DateTime.Parse(tmp);
                    }
                )
            );

            DateTime Time = DateTime.MinValue;
            LecturesGrid.Invoke(
                new Action(
                    delegate ()
                    {
                        string tmp = LecturesGrid.Rows[LecturesGrid.SelectedCells[0].RowIndex].Cells[2].Value.ToString();
                        if (tmp != null)
                            Time = DateTime.Parse(tmp);
                    }
                )
            );

            ScheduledDateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second).ToUniversalTime();


            GoogleYouTube.CreateLiveEvent(
                ref CurrentBroadcast,
                    StreamTitle,
                    Name,
                    ScheduledDateTime,
                    Description);

            if (CurrentBroadcast != null)
            {
                BroadcastSettingsLink.Invoke(new Action(delegate () { BroadcastSettingsLink.Enabled = true; }));
                CurrentStream = GoogleYouTube.GetStreamByTitle(StreamTitle);
                if (CurrentBroadcast != null)
                {
                    CurrentBroadcast = GoogleYouTube.StartEvent(CurrentBroadcast.Id, CurrentStream.Id);
                    if (CurrentBroadcast != null)
                    {
                        EndEventButton.Invoke(new Action(delegate () { EndEventButton.Enabled = true; }));
                        EndEventButton.Invoke(new Action(delegate () { EndEventButton.BackColor = System.Drawing.Color.Red; }));

                        CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = false; }));
                        CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; }));
                    }
                }
            }

        }

        private void EndLiveEvent()
        {
            CurrentBroadcast = GoogleYouTube.EndEvent(CurrentBroadcast.Id);

            if (CurrentBroadcast != null)
            {
                BroadcastSettingsLink.Invoke(new Action(delegate () { BroadcastSettingsLink.Enabled = false; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = true; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LimeGreen; }));

                EndEventButton.Invoke(new Action(delegate () { EndEventButton.Enabled = false; }));
                EndEventButton.Invoke(new Action(delegate () { EndEventButton.BackColor = System.Drawing.Color.LightGray; }));

                Courses_List.Invoke(new Action(delegate () { Courses_List.Enabled = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                Accounts_List.Invoke(new Action(delegate () { Accounts_List.Enabled = true; }));

                CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = false; }));
                CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; }));
                SetStatus("Stream is complete. Don't forget to stop OBS stream.");
            }
        }

        private void CancelLiveEventCreation()
        {
            if (backgroundWorker.IsBusy == true)
            {
                backgroundWorker.Abort();
                backgroundWorker.Dispose();

                if (CurrentBroadcast != null)
                {
                    if(CurrentBroadcast.Id != null)
                        if (GoogleYouTube.DeleteEvent(CurrentBroadcast.Id))
                        {
                            CurrentBroadcast = null;
                            CurrentStream = null;
                        }
                }
            }
            if (CurrentBroadcast == null)
            {
                BroadcastSettingsLink.Invoke(new Action(delegate () { BroadcastSettingsLink.Enabled = false; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = true; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LimeGreen; }));

                EndEventButton.Invoke(new Action(delegate () { EndEventButton.Enabled = false; }));
                EndEventButton.Invoke(new Action(delegate () { EndEventButton.BackColor = System.Drawing.Color.LightGray; }));

                Courses_List.Invoke(new Action(delegate () { Courses_List.Enabled = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                Accounts_List.Invoke(new Action(delegate () { Accounts_List.Enabled = true; }));

                CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = false; }));
                CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; }));
                SetStatus("Canceled by user");
            }
        }

        private void ClearAllData()
        {
            if (LecturesGrid.Rows.Count > 0)
                LecturesGrid.Rows.Clear();
            if (EMails_List.Items.Count > 0)
                EMails_List.Items.Clear();
            if (Courses_List.Items.Count > 0)
                Courses_List.Items.Clear();
        }

        private void UpdateAccountsList()
        {
            string[] lines = null;
            lines = System.IO.File.ReadAllLines(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt");
            if (lines.Length != 0)
                if (Accounts_List.Items.Count == 0)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] != string.Empty)
                            Accounts_List.Items.Add(lines[i]);
                    }
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
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = false; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LightGray; }));
                MessageBox.Show("No Accounts in " + System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt" + " file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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

        public void SetStatus(string Text)
        {
            CurrentStatusLabel.Text = Text;
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
