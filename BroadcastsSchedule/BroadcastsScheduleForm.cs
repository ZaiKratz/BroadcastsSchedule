using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Google.Apis.YouTube.v3.Data;

namespace BroadcastsSchedule
{
    public struct BroadcastData
    {
        public string streamTitle;
        public string broadcastName;
        public DateTime scheduledDateTime;
        public string broadcastDescription;
    }


    public partial class BroadcastsScheduleClass : Form
    {
        private DataGridView selectedGrid = null;

        private List<Google.Apis.YouTube.v3.Data.LiveBroadcast> liveBroadcasts;
        private List<Google.Apis.YouTube.v3.Data.LiveBroadcast> onlineBroadcasts;

        private Google.Apis.YouTube.v3.Data.LiveBroadcast CurrentBroadcast = null;
        private Google.Apis.YouTube.v3.Data.LiveStream CurrentStream = null;
        private string YouTubeUser = null;

        private static object AuthLocker = new object();

        private Thread AuthServisesThread = null;

        private System.Windows.Forms.Timer timer = null;
        private int startTime;
        private const int timerExpireIn = 300;

        public BroadcastsScheduleClass()
        {
            InitializeComponent();

            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(UpdateButton, "Update All");
            timer = new System.Windows.Forms.Timer();
        }

        private void BroadcastsScheduleClass_Load(object sender, EventArgs e)
        {
            UpdateAccountsList();

            if (AccountsList_ComboBox.Items.Count > 0)
                AccountsList_ComboBox.SelectedIndex = 0;

            EndEventButton.Enabled = false;
            EndEventButton.BackColor = System.Drawing.Color.LightGray;

            CancelEventButton.Enabled = false;
            CancelEventButton.BackColor = System.Drawing.Color.LightGray;

            InitLecturesGrid();
            InitScheduledBroadcastsGrid();
            InitOnlineBroadcastsGrid();

            timer.Tick += new EventHandler(OnCheckBroadcasts);
            timer.Interval = timerExpireIn * 1000;
            timer.Start();
            startTime = DateTime.Now.Second;
        }

        private void InitLecturesGrid()
        {
            Lectures_GridView.Columns.Add("LecturesColumn", "Lectures");

            CalendarColumn Date = new CalendarColumn();
            Date.HeaderText = "Date";
            TimeColumn Time = new TimeColumn();
            Time.HeaderText = "Time";

            Lectures_GridView.Columns.Add(Date);
            Lectures_GridView.Columns.Add(Time);

            Lectures_GridView.Columns.Add("DescriptionColumn", "Description");
        }

        private void InitScheduledBroadcastsGrid()
        {
            ScheduledBroadcasts_GridView.Columns.Add("BroadcastName", "Broadcast Name");
            ScheduledBroadcasts_GridView.Columns.Add("CourseName", "Course");
            ScheduledBroadcasts_GridView.Columns.Add("CreationDate", "Creation Date");
            ScheduledBroadcasts_GridView.Columns.Add("StartDate", "Start Date");
            ScheduledBroadcasts_GridView.Columns.Add("Description", "Description");
        }

        private void InitOnlineBroadcastsGrid()
        {
            CurrentStreams_GridView.Columns.Add("BroadcastName", "Broadcast Name");
            CurrentStreams_GridView.Columns.Add("CourseName", "Course");
            CurrentStreams_GridView.Columns.Add("CreationDate", "Creation Date");
            CurrentStreams_GridView.Columns.Add("StartDate", "Start Date");
            CurrentStreams_GridView.Columns.Add("Description", "Description");
        }

        private void CoursesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetStatus("Getting data from YouTube account. Please wait...");
            UpdateData();
            SetStatus("Ready");
        }
        
        private void UpdateList_Click(object sender, EventArgs e)
        {
            UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = false; }));
            CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = false; }));
            AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = false; }));
            if (AuthServisesThread != null)
            {
                if (AuthServisesThread.IsAlive)
                {
                    AuthServisesThread.Abort();
                    AuthServisesThread = null;
                    if (AccountsList_ComboBox.Enabled == false)
                    {
                        AccountsList_ComboBox.Enabled = true;
                        UpdateAccountsList();
                        if (AccountsList_ComboBox.Items.Count > 0)
                            AccountsList_ComboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    UpdateAccountsList();
                    if (AccountsList_ComboBox.Items.Count > 0)
                        AccountsList_ComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                UpdateAccountsList();
                if (AccountsList_ComboBox.Items.Count > 0)
                    AccountsList_ComboBox.SelectedIndex = 0;
            }
        }

        private void StartLiveEvent_Click(object sender, EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
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

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (selectedGrid.SelectedRows.Count == 1)
            {
                int SelectedIndex = 0;
                tabControl.Invoke(new Action(delegate () { SelectedIndex = tabControl.SelectedIndex; }));
                if(SelectedIndex == 0)
                    CreateLiveEventAsync(selectedGrid);
                else
                {
                    var broadcast = liveBroadcasts[selectedGrid.SelectedRows[0].Index];
                    var Stream = GoogleYouTube.GetStreamByID(broadcast.ContentDetails.BoundStreamId);
                    var startedSteam = GoogleYouTube.StartEvent(broadcast.Id, Stream.Id);
                    if (startedSteam != null)
                        onlineBroadcasts.Add(startedSteam);
                    UpdateOnlineBroadcastsListAsync();
                }
            }
                
            else
                CreateListOfLiveBroadcasts(selectedGrid);
        }

        private void BroadcastsScheduleClass_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AuthServisesThread != null)
            {
                if (AuthServisesThread.IsAlive)
                {
                    AuthServisesThread.Abort();
                    AuthServisesThread = null;
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
                        //GoogleYouTube.DeleteEvent(CurrentBroadcast.Id);
                        //break;
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
            foreach (var Item in EMailsList_ListView.Items)
                Items += Item.ToString() + Environment.NewLine;
            if(Items != string.Empty)
            {
                Items = Items.Remove(Items.Length - 1);
                Clipboard.SetText(Items);
                SetStatus("Emails copied to clipboard");
            }
        }

        private void EditEmailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EMailsEditing EMails = new EMailsEditing();
            //EMails.ShowDialog();
        }

        private void Accounts_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllData();
            YouTubeUser = AccountsList_ComboBox.SelectedItem.ToString();
            if (AuthServisesThread != null)
            {
                AuthServisesThread.Abort();
                AuthServisesThread = null;
            }
            if (AuthServisesThread == null)
            {
                AuthServisesThread = new Thread(Authenticate);
                AuthServisesThread.Start();
            }

        }

        private void EditTablesIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditIDs Edit = new EditIDs();
            Edit.ShowDialog();
        }

        private void EditAccountsToolStripMenuItem_Click(object sender, EventArgs e)
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
                AuthServisesThread = null;
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = true; }));
                AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = true; }));
                CancelAuth.Invoke(new Action(delegate () { CancelAuth.Visible = false; }));
            }
            else
            {
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = true; }));
                AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = true; }));
                CancelAuth.Invoke(new Action(delegate () { CancelAuth.Visible = false; }));
            }
        }

        private void Authenticate()
        {
            lock (AuthLocker)
            {
                string acc = null;
                AccountsList_ComboBox.Invoke(new Action(delegate () { acc = AccountsList_ComboBox.SelectedItem.ToString(); }));
                SetStatus("Logging into " + acc);
                CancelAuth.Invoke(new Action(delegate () { CancelAuth.Visible = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = false; }));
                CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = false; }));
                StartEventButton.Invoke(
                    new Action(
                        delegate ()
                        {
                            StartEventButton.Enabled = false;
                            StartEventButton.BackColor = System.Drawing.Color.LightGray;
                        }
                    )
                );

                ListOfStreams_Button.Invoke(
                    new Action(
                        delegate ()
                        {
                            ListOfStreams_Button.Enabled = false;
                            ListOfStreams_Button.BackColor = System.Drawing.Color.LightGray;
                        }
                    )
                );
                //StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LightGray; });

                AccountsList_ComboBox.Invoke(
                    new Action(
                        delegate ()
                        {
                            AccountsList_ComboBox.Enabled = false;
                        }
                    )
                );

                string SpreadsheetUser = null;
                try
                {
                    SpreadsheetUser = System.IO.File.ReadAllText(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\GoogleSpreadsheetAccount.txt");
                    SpreadsheetUser = SpreadsheetUser.Replace("\n", "");
                    GoogleSheets.AuthenticateOauth(SpreadsheetUser).Wait();
                    GoogleYouTube.AuthenticateOauth(YouTubeUser).Wait();
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (AggregateException)
                {
                    MessageBox.Show("Google spreadsheet account is wrong or empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SetStatus("Login is completed");

                AccountsList_ComboBox.Invoke(
                    new Action(
                        delegate ()
                        {
                            AccountsList_ComboBox.Enabled = true;
                        }
                    )
                );

                UpdateCoursesList();
                CancelAuth.Invoke(new Action(delegate () { CancelAuth.Visible = false; }));
                SetStatus("Ready");
                Thread.Sleep(200);
            }
        }

        private void UpdateData()
        {
            UpdateLectureListAsync();
            UpdateScheduledBroadcastsListAsync();
            UpdateOnlineBroadcastsListAsync();
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
                    if (CoursesList_ComboBox.Items.Count > 0)
                    {
                        CoursesList_ComboBox.Invoke(
                                        new Action(
                                            delegate ()
                                            {
                                                CoursesList_ComboBox.Items.Clear();
                                            }
                                        )
                                    );
                    }

                    foreach (var Item in CoursesList)
                    {
                        CoursesList_ComboBox.Invoke(
                                    new Action(
                                        delegate ()
                                        {
                                            CoursesList_ComboBox.Items.AddRange(Item.ToArray());
                                        }
                                    )
                                );
                    }

                    CoursesList_ComboBox.Invoke(
                                new Action(
                                    delegate ()
                                    {
                                        CoursesList_ComboBox.SelectedIndex = 0;
                                    }
                                )
                            );
                    
                }
            }
        }

        private void UpdateScheduledBroadcastsListAsync()
        {
            if(liveBroadcasts != null)
            {
                ScheduledBroadcasts_GridView.Invoke(
                                   new Action(
                                       delegate ()
                                       {
                                           ScheduledBroadcasts_GridView.Rows.Clear();
                                       }
                                   )
                               );
                liveBroadcasts.Clear();
            }
                
            liveBroadcasts = GoogleYouTube.GetScheduledBroadcasts();

            if (liveBroadcasts != null)
            {
                foreach (var broadcast in liveBroadcasts)
                {
                    var Stream = GoogleYouTube.GetStreamByID(broadcast.ContentDetails.BoundStreamId);
                    
                    ScheduledBroadcasts_GridView.Invoke(
                                   new Action(
                                       delegate ()
                                       {
                                           ScheduledBroadcasts_GridView.Rows.Add(
                                               broadcast.Snippet.Title,
                                               Stream != null ? Stream.Snippet.Title : "unknown",
                                               broadcast.Snippet.PublishedAt.ToString(),
                                               broadcast.Snippet.ScheduledStartTime.ToString(),
                                               broadcast.Snippet.Description
                                               );
                                       }
                                   )
                               );
                }
            }

        }

        private void UpdateOnlineBroadcastsListAsync()
        {
            if(onlineBroadcasts != null)
            {
                CurrentStreams_GridView.Invoke(
                                     new Action(
                                         delegate ()
                                         {
                                             CurrentStreams_GridView.Rows.Clear();
                                         }
                                     )
                                 );
                onlineBroadcasts.Clear();
            }
                
            onlineBroadcasts = GoogleYouTube.GetOnlineBroadcasts();

            if (onlineBroadcasts != null)
            {
                foreach (var broadcast in onlineBroadcasts)
                {
                    var Stream = GoogleYouTube.GetStreamByID(broadcast.ContentDetails.BoundStreamId);

                    CurrentStreams_GridView.Invoke(
                                   new Action(
                                       delegate ()
                                       {
                                           CurrentStreams_GridView.Rows.Add(
                                               broadcast.Snippet.Title,
                                               Stream != null ? Stream.Snippet.Title : "unknown",
                                               broadcast.Snippet.PublishedAt.ToString(),
                                               broadcast.Snippet.ScheduledStartTime.ToString(),
                                               broadcast.Snippet.Description
                                               );
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

                if (CoursesList_ComboBox.Items.Count != 0)
                {
                    CoursesList_ComboBox.Invoke(
                                    new Action(
                                        delegate ()
                                        {
                                            SelectedItem = CoursesList_ComboBox.SelectedItem.ToString();
                                        }
                                    )
                                );
                    var LecturesList = GoogleSheets.GetLectures(SelectedItem);

                    var LecturesCount = 0;
                    Lectures_GridView.Invoke(
                        new Action(
                            delegate ()
                            {
                                LecturesCount = Lectures_GridView.Rows.Count;
                            }
                        )
                    );
                    if (LecturesCount > 0)
                        Lectures_GridView.Invoke(
                            new Action(
                                delegate ()
                                {
                                    Lectures_GridView.Rows.Clear();
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
                               "dd.MM.yyyy",
                               null);

                        if (tmptime != string.Empty)
                            Time = DateTime.Parse(
                            Item.ElementAtOrDefault(2).ToString(), 
                            System.Globalization.CultureInfo.InvariantCulture);

                        Lectures_GridView.Invoke(
                                    new Action(
                                        delegate ()
                                        {
                                            Lectures_GridView.Rows.Add(
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
                CoursesList_ComboBox.Invoke(
                    new Action(
                        delegate ()
                        {
                            SelIndex = CoursesList_ComboBox.SelectedIndex;
                        }
                    )
                );

                var EMailList = GoogleSheets.GetEMails(SelIndex);
                var EmailsCount = 0;
                EMailsList_ListView.Invoke(
                    new Action(
                        delegate ()
                        {
                            EmailsCount = EMailsList_ListView.Items.Count;
                        }
                    )
                );

                if (EmailsCount > 0)
                    EMailsList_ListView.Invoke(
                        new Action(
                            delegate ()
                            {
                                EMailsList_ListView.Items.Clear();
                            }
                        )
                    );

                if (EMailList != null)
                {
                    foreach (var Item in EMailList)
                        EMailsList_ListView.Invoke(
                            new Action(
                                delegate ()
                                {
                                    EMailsList_ListView.Items.AddRange(Item.ToArray());
                                }
                            )
                        );
                }
  
                CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
            }
        }

        private BroadcastData GetBroadcastDataFromCells(DataGridViewRow row)
        {
            string StreamTitle = "";
            string Name = "";
            string Description = "";
            DateTime ScheduledDateTime = DateTime.Now;


            Lectures_GridView.Invoke(
                new Action(
                    delegate ()
                    {
                        StreamTitle = CoursesList_ComboBox.SelectedItem.ToString();
                    }
                )
            );
            Lectures_GridView.Invoke(
                new Action(
                    delegate ()
                    {
                        
                        Name = row.Cells[0].Value.ToString();
                        //Name = Lectures_GridView.Rows[Lectures_GridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                    }
                )
            );
            Lectures_GridView.Invoke(
                new Action(
                    delegate ()
                    {
                        Description = row.Cells[3].Value.ToString();
                        //Description = Lectures_GridView.Rows[Lectures_GridView.SelectedCells[0].RowIndex].Cells[3].Value.ToString();
                    }
                )
            );

            DateTime Date = DateTime.MinValue;
            Lectures_GridView.Invoke(
                new Action(
                    delegate ()
                    {
                        string tmp = row.Cells[1].Value.ToString();
                        //string tmp = Lectures_GridView.Rows[Lectures_GridView.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                        if (tmp != null)
                            Date = DateTime.Parse(tmp);
                    }
                )
            );

            DateTime Time = DateTime.MinValue;
            Lectures_GridView.Invoke(
                new Action(
                    delegate ()
                    {
                        string tmp = row.Cells[2].Value.ToString();
                        //string tmp = Lectures_GridView.Rows[Lectures_GridView.SelectedCells[0].RowIndex].Cells[2].Value.ToString();
                        if (tmp != null)
                            Time = DateTime.Parse(tmp);
                    }
                )
            );

            ScheduledDateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second).ToUniversalTime();

            BroadcastData broadcastData = new BroadcastData()
            {
                streamTitle = StreamTitle,
                broadcastName = Name,
                scheduledDateTime = ScheduledDateTime,
                broadcastDescription = Description
            };
            return broadcastData;
        }

        private void CreateLiveEventAsync(DataGridView dataGridView)
        {
            CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = false; }));
            UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = false; }));
            AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = false; }));

            StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = false; }));
            StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LightGray; }));

            CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = true; }));
            CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.Red; }));
            
            BroadcastData broadcastData = GetBroadcastDataFromCells(Lectures_GridView.SelectedRows[0]);
            SetStatus("Creating YouTube Live Broadcast: " + broadcastData.broadcastName);

            CurrentBroadcast = GoogleYouTube.CreateLiveEvent(broadcastData);

            if (CurrentBroadcast != null)
            {
                SelectedBroadcastSettingsLink.Invoke(new Action(delegate () { SelectedBroadcastSettingsLink.Enabled = true; }));
                CurrentStream = GoogleYouTube.GetStreamByTitle(broadcastData.streamTitle);
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

        private void StartBroadcast(string BroadcastID, string StreamID)
        {
            var createrBroadcast = GoogleYouTube.StartEvent(BroadcastID, StreamID);
            if (createrBroadcast != null)
                onlineBroadcasts.Add(createrBroadcast);
            UpdateOnlineBroadcastsListAsync();
        }

        private void EndLiveEvent()
        {
            CurrentBroadcast = GoogleYouTube.EndEvent(CurrentBroadcast.Id);

            if (CurrentBroadcast != null)
            {
                SelectedBroadcastSettingsLink.Invoke(new Action(delegate () { SelectedBroadcastSettingsLink.Enabled = false; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = true; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LimeGreen; }));

                EndEventButton.Invoke(new Action(delegate () { EndEventButton.Enabled = false; }));
                EndEventButton.Invoke(new Action(delegate () { EndEventButton.BackColor = System.Drawing.Color.LightGray; }));

                CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = true; }));

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
                    //if (CurrentBroadcast.Id != null)
                        //if (GoogleYouTube.DeleteEvent(CurrentBroadcast.Id))
                        {
                            CurrentBroadcast = null;
                            CurrentStream = null;
                        }
                }
            }
            if (CurrentBroadcast == null)
            {
                SelectedBroadcastSettingsLink.Invoke(new Action(delegate () { SelectedBroadcastSettingsLink.Enabled = false; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = true; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LimeGreen; }));

                EndEventButton.Invoke(new Action(delegate () { EndEventButton.Enabled = false; }));
                EndEventButton.Invoke(new Action(delegate () { EndEventButton.BackColor = System.Drawing.Color.LightGray; }));

                CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = true; }));
                UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = true; }));

                CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = false; }));
                CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; }));
                SetStatus("Canceled by user");
            }
        }

        private void ClearAllData()
        {
            if (Lectures_GridView.Rows.Count > 0)
                Lectures_GridView.Rows.Clear();
            if (ScheduledBroadcasts_GridView.Rows.Count > 0)
                ScheduledBroadcasts_GridView.Rows.Clear();
            if (CurrentStreams_GridView.Rows.Count > 0)
                CurrentStreams_GridView.Rows.Clear();
            if (EMailsList_ListView.Items.Count > 0)
                EMailsList_ListView.Items.Clear();
            if (CoursesList_ComboBox.Items.Count > 0)
                CoursesList_ComboBox.Items.Clear();
        }

        private void UpdateAccountsList()
        {
            string[] lines = null;
            try
            {
                lines = System.IO.File.ReadAllLines(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Accounts.txt");
                if (lines.Length != 0)
                    if (AccountsList_ComboBox.Items.Count == 0)
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i] != string.Empty)
                                AccountsList_ComboBox.Items.Add(lines[i]);
                        }
                    }
                    else
                    {
                        AccountsList_ComboBox.Items.Clear();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i] != string.Empty)
                                AccountsList_ComboBox.Items.Add(lines[i]);
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
            catch(System.IO.FileNotFoundException ex)
            {
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = false; }));
                StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LightGray; }));
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        
        public string GetCurrentUser()
        {
            return YouTubeUser;
        }

        public int GetCurrentCourse()
        {
            int SelIndex = 0;
            CoursesList_ComboBox.Invoke(
                new Action(
                    delegate ()
                    {
                        SelIndex = CoursesList_ComboBox.SelectedIndex;
                    }
                )
            );
            return SelIndex;
        }

        public void SetStatus(string Text)
        {
            CurrentStatusLabel.Text = Text;
        }

        private void ChangeGoogleSpreadSheetsAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoogleSpreadsheetAccount GoogleAcc = new GoogleSpreadsheetAccount();
            GoogleAcc.ShowDialog();
        }

        private void ListOfStreams_Button_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void CreateListOfLiveBroadcasts(DataGridView dataGridView)
        {
            CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = false; }));
            UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = false; }));
            AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = false; }));

            ListOfStreams_Button.Invoke(new Action(delegate () { ListOfStreams_Button.Enabled = false; }));
            ListOfStreams_Button.Invoke(new Action(delegate () { ListOfStreams_Button.BackColor = System.Drawing.Color.LightGray; }));

            CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = true; }));
            CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.Red; }));

            List<BroadcastData> data = new List<BroadcastData>();
            var SelectedRows = dataGridView.SelectedRows;
            foreach (var SelectedRow in SelectedRows)
            {
                if (SelectedRow is DataGridViewRow row)
                {
                    BroadcastData broadcastData = GetBroadcastDataFromCells(row);
                    data.Add(broadcastData);

                }
            }

            if (data.Count > 0)
            {
                var CreatedBroadcasts = GoogleYouTube.CreateListOfLiveEvents(data);

                if(CreatedBroadcasts.Count > 0)
                {
                    AddDataToScheduledBroadcastsGrid(CreatedBroadcasts);

                    CoursesList_ComboBox.Invoke(new Action(delegate () { CoursesList_ComboBox.Enabled = true; }));
                    UpdateButton.Invoke(new Action(delegate () { UpdateButton.Enabled = true; }));
                    AccountsList_ComboBox.Invoke(new Action(delegate () { AccountsList_ComboBox.Enabled = true; }));

                    if (Lectures_GridView.SelectedRows.Count > 1)
                    {
                        ListOfStreams_Button.Invoke(new Action(delegate () { ListOfStreams_Button.Enabled = true; }));
                        ListOfStreams_Button.Invoke(new Action(delegate () { ListOfStreams_Button.BackColor = System.Drawing.Color.LimeGreen; }));
                    }
                    else
                    {
                        StartEventButton.Invoke(new Action(delegate () { StartEventButton.Enabled = true; }));
                        StartEventButton.Invoke(new Action(delegate () { StartEventButton.BackColor = System.Drawing.Color.LimeGreen; }));
                    }

                    CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.Enabled = false; }));
                    CancelEventButton.Invoke(new Action(delegate () { CancelEventButton.BackColor = System.Drawing.Color.LightGray; }));
                }
            }
        }

        private void AddDataToScheduledBroadcastsGrid(List<LiveBroadcast> createdBroadcasts)
        {
            if (createdBroadcasts != null)
            {
                if (liveBroadcasts != null)
                    liveBroadcasts.AddRange(createdBroadcasts);
                else
                    liveBroadcasts = new List<LiveBroadcast>(createdBroadcasts);

                foreach (var broadcast in createdBroadcasts)
                {
                    var Stream = GoogleYouTube.GetStreamByID(broadcast.ContentDetails.BoundStreamId);
                    ScheduledBroadcasts_GridView.Invoke(
                        new Action(delegate ()
                        {
                            ScheduledBroadcasts_GridView.Rows.Add(
                            broadcast.Snippet.Title.ToString(),
                            Stream != null ? Stream.Snippet.Title : "unknown",
                            broadcast.Snippet.PublishedAt.ToString(),
                            broadcast.Snippet.ScheduledStartTime.ToString(),
                            broadcast.Snippet.Description
                            );
                        }
                        )
                    );
                }
            }

        }
        
        private void Lectures_GridView_SelectionChanged(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy) return;

            if (Lectures_GridView.SelectedRows.Count > 1)
            {
                ListOfStreams_Button.Enabled = true;
                ListOfStreams_Button.BackColor = System.Drawing.Color.LimeGreen;

                StartEventButton.Enabled = false;
                StartEventButton.BackColor = System.Drawing.Color.LightGray;
            }
            else
            { 
                ListOfStreams_Button.Enabled = false;
                ListOfStreams_Button.BackColor = System.Drawing.Color.LightGray;

                StartEventButton.Enabled = true;
                StartEventButton.BackColor = System.Drawing.Color.LimeGreen;
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    selectedGrid = Lectures_GridView;
                    if (selectedGrid.SelectedRows.Count > 1)
                    {
                        ListOfStreams_Button.Enabled = true;
                        ListOfStreams_Button.BackColor = System.Drawing.Color.LimeGreen;
                    }
                    else
                    {
                        ListOfStreams_Button.Enabled = false;
                        ListOfStreams_Button.BackColor = System.Drawing.Color.LightGray;
                    }
                    break;
                case 1:
                    selectedGrid = ScheduledBroadcasts_GridView;
                    ListOfStreams_Button.Enabled = false;
                    ListOfStreams_Button.BackColor = System.Drawing.Color.LightGray;
                    break;
                case 2:
                    selectedGrid = CurrentStreams_GridView;
                    ListOfStreams_Button.Enabled = false;
                    ListOfStreams_Button.BackColor = System.Drawing.Color.LightGray;
                    break;
            }
        }

        private void OnCheckBroadcasts(object sender, EventArgs e)
        {
            UpdateData();
            if (onlineBroadcasts != null)
                CheckOnlineBroadcasts();
        }

        private void CheckOnlineBroadcasts()
        {
            foreach (var ob in onlineBroadcasts)
            {
                if (ob != null)
                {
                    var stream = GoogleYouTube.GetStreamByID(ob.ContentDetails.BoundStreamId);
                    if (stream != null)
                    {
                        if(stream.Status.StreamStatus != "active")
                        {
                            GoogleYouTube.EndEvent(ob.Id);
                            MessageBox.Show("Live broadcast " + ob.Snippet.Title + " is stopped due to lack of stream", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }


        private void TabControl_HandleCreated(object sender, EventArgs e)
        {
            if (Lectures_GridView != null)
                selectedGrid = Lectures_GridView;
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
