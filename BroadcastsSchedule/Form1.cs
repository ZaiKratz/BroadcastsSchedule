using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace BroadcastsSchedule
{
    public partial class BroadcastsSchedule : Form
    {
        private Google.Apis.YouTube.v3.Data.LiveBroadcast CreatedBroadcast;
        private Google.Apis.YouTube.v3.Data.LiveStream CurrentStream;
        private Google.Apis.YouTube.v3.YouTubeService Service = YoutubeStream.AuthenticateOauth();

        public BroadcastsSchedule()
        {
            InitializeComponent();
        }

        private void BroadcastsSchedule_Load(object sender, EventArgs e)
        {
            SetLabelText("Select lecture and press \"Start Stream\" button");
            EndEventButton.Enabled = false;
            LecturesGrid.Columns.Add("LecturesColumn", "Lectures");

            CalendarColumn Date = new CalendarColumn();
            Date.HeaderText = "Date";
            TimeColumn Time = new TimeColumn();
            Time.HeaderText = "Time";

            LecturesGrid.Columns.Add(Date);
            LecturesGrid.Columns.Add(Time);

            UpdateCoursesList();

        }

        private void UpdateData()
        {
            var LecturesList = Sheets.GetLectures(Courses_List.SelectedItem.ToString());

            if (LecturesGrid.Rows.Count > 0)
                LecturesGrid.Rows.Clear();

            foreach(var Item in LecturesList)
                LecturesGrid.Rows.Add(Item[0].ToString(), DateTime.Parse(Item[1].ToString()), DateTime.Parse(Item[2].ToString()));

            var EMailList = Sheets.GetEMails(Courses_List.SelectedIndex);
            if (EMails_List.Items.Count > 0)
                EMails_List.Items.Clear();

            foreach (var Item in EMailList)
                EMails_List.Items.AddRange(Item.ToArray());
        }

        private void UpdateCoursesList()
        {
            var CoursesList = Sheets.GetCourses();

            if (Courses_List.Items.Count > 0)
                Courses_List.Items.Clear();

            foreach (var Item in CoursesList)
                Courses_List.Items.AddRange(Item.ToArray());

            Courses_List.SelectedIndex = 0;
        }

        private void CoursesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }
        

        private void UpdateList_Click(object sender, EventArgs e)
        {
            UpdateCoursesList();
            UpdateData();
        }

        private void StartEvent_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            
        }            

        private void CreateLiveEventOnBackground()
        {
            Courses_List.BeginInvoke((MethodInvoker)delegate () { Courses_List.Enabled = false; });
            UpdateList.BeginInvoke((MethodInvoker)delegate () { UpdateList.Enabled = false; });
            StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.Enabled = false; });
            SetLabelText("Creating YouTube Live Broadcast...");

            string Stream = "";
            string Name = "";

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
            }

            CreatedBroadcast = YoutubeStream.CreateLiveEvent(
                Service,
                Stream,
                Name,
                "Smthg");

            CurrentStream = YoutubeStream.GetStreamByTitle(Stream, Service);

            YoutubeStream.StartEvent(Service, CreatedBroadcast.Id, CurrentStream.Id);

            EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.Enabled = true; });
        }

        public void SetLabelText(string Text)
        {
            label4.BeginInvoke((MethodInvoker)delegate () { label4.Text = Text; });
        }

        private void EndEventButton_Click(object sender, EventArgs e)
        {
            SetLabelText("Ending Stream...");
            YoutubeStream.EndEvent(Service, CreatedBroadcast.Id);
            StartStreamButton.BeginInvoke((MethodInvoker)delegate () { StartStreamButton.Enabled = true; });
            EndEventButton.BeginInvoke((MethodInvoker)delegate () { EndEventButton.Enabled = false; });
            Courses_List.BeginInvoke((MethodInvoker)delegate () { Courses_List.Enabled = true; });
            UpdateList.BeginInvoke((MethodInvoker)delegate () { UpdateList.Enabled = true; });
            SetLabelText("Stream is complete. Don't forget to stop OBS stream");
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            CreateLiveEventOnBackground();
        }

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            
        }
    }
}
