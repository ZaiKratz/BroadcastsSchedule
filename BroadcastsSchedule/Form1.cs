using System;
using System.Linq;
using System.Windows.Forms;


namespace BroadcastsSchedule
{
    public partial class BroadcastsSchedule : Form
    {
        public BroadcastsSchedule()
        {
            InitializeComponent();
        }

        private void BroadcastsSchedule_Load(object sender, EventArgs e)
        {
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

        private void StartStream_Click(object sender, EventArgs e)
        {
            //var Service = YoutubeStream.AuthenticateOauth();

            //YoutubeStream.StartEvent(Service, TODO);
        }

        private void CreateBroadcast_Click(object sender, EventArgs e)
        {
            //YoutubeStream.CreateLiveEvent(TODO, TODO, TODO);
        }
    }
}
