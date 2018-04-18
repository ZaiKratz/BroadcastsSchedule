namespace BroadcastsSchedule
{
    partial class BroadcastsScheduleClass
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BroadcastsScheduleClass));
            this.UpdateButton = new System.Windows.Forms.Button();
            this.CoursesList_ComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCoursesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editLecturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editEmailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTablesIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editAccountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.EMailsList_ListView = new System.Windows.Forms.ListBox();
            this.Lectures_GridView = new System.Windows.Forms.DataGridView();
            this.StartEventButton = new System.Windows.Forms.Button();
            this.EndEventButton = new System.Windows.Forms.Button();
            this.CancelEventButton = new System.Windows.Forms.Button();
            this.CopyToClipboardButton = new System.Windows.Forms.Button();
            this.AccountsList_ComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SelectedBroadcastSettingsLink = new System.Windows.Forms.LinkLabel();
            this.CancelAuth = new System.Windows.Forms.Button();
            this.CurrentStatus = new System.Windows.Forms.StatusStrip();
            this.CurrentStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ListOfStreams_Button = new System.Windows.Forms.Button();
            this.ScheduledBroadcasts_GridView = new System.Windows.Forms.DataGridView();
            this.CurrentStreams_GridView = new System.Windows.Forms.DataGridView();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.LecturesPage = new System.Windows.Forms.TabPage();
            this.ScheduledBroadcastsPage = new System.Windows.Forms.TabPage();
            this.BroadcastsOnlinePage = new System.Windows.Forms.TabPage();
            this.backgroundWorker = new BroadcastsSchedule.AbortableBackgroundWorker();
            this.abortableBackgroundWorker1 = new BroadcastsSchedule.AbortableBackgroundWorker();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Lectures_GridView)).BeginInit();
            this.CurrentStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduledBroadcasts_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentStreams_GridView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.LecturesPage.SuspendLayout();
            this.ScheduledBroadcastsPage.SuspendLayout();
            this.BroadcastsOnlinePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateButton
            // 
            this.UpdateButton.Enabled = false;
            this.UpdateButton.Image = ((System.Drawing.Image)(resources.GetObject("UpdateButton.Image")));
            this.UpdateButton.Location = new System.Drawing.Point(139, 40);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(25, 23);
            this.UpdateButton.TabIndex = 0;
            this.UpdateButton.TabStop = false;
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateList_Click);
            // 
            // CoursesList_ComboBox
            // 
            this.CoursesList_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CoursesList_ComboBox.FormattingEnabled = true;
            this.CoursesList_ComboBox.Location = new System.Drawing.Point(12, 41);
            this.CoursesList_ComboBox.Name = "CoursesList_ComboBox";
            this.CoursesList_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.CoursesList_ComboBox.TabIndex = 1;
            this.CoursesList_ComboBox.SelectedIndexChanged += new System.EventHandler(this.CoursesList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Courses";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(853, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCoursesToolStripMenuItem,
            this.editLecturesToolStripMenuItem,
            this.editEmailsToolStripMenuItem,
            this.editTablesIDToolStripMenuItem,
            this.editAccountsToolStripMenuItem,
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // editCoursesToolStripMenuItem
            // 
            this.editCoursesToolStripMenuItem.Name = "editCoursesToolStripMenuItem";
            this.editCoursesToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.editCoursesToolStripMenuItem.Text = "Edit Courses";
            this.editCoursesToolStripMenuItem.Visible = false;
            // 
            // editLecturesToolStripMenuItem
            // 
            this.editLecturesToolStripMenuItem.Name = "editLecturesToolStripMenuItem";
            this.editLecturesToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.editLecturesToolStripMenuItem.Text = "Edit Lectures";
            this.editLecturesToolStripMenuItem.Visible = false;
            // 
            // editEmailsToolStripMenuItem
            // 
            this.editEmailsToolStripMenuItem.Name = "editEmailsToolStripMenuItem";
            this.editEmailsToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.editEmailsToolStripMenuItem.Text = "Edit Emails";
            this.editEmailsToolStripMenuItem.Visible = false;
            this.editEmailsToolStripMenuItem.Click += new System.EventHandler(this.EditEmailsToolStripMenuItem_Click);
            // 
            // editTablesIDToolStripMenuItem
            // 
            this.editTablesIDToolStripMenuItem.Name = "editTablesIDToolStripMenuItem";
            this.editTablesIDToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.editTablesIDToolStripMenuItem.Text = "Edit IDs";
            this.editTablesIDToolStripMenuItem.Click += new System.EventHandler(this.EditTablesIDToolStripMenuItem_Click);
            // 
            // editAccountsToolStripMenuItem
            // 
            this.editAccountsToolStripMenuItem.Name = "editAccountsToolStripMenuItem";
            this.editAccountsToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.editAccountsToolStripMenuItem.Text = "Edit Accounts";
            this.editAccountsToolStripMenuItem.Click += new System.EventHandler(this.EditAccountsToolStripMenuItem_Click);
            // 
            // changeGoogleSpreadSheetsAccountToolStripMenuItem
            // 
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Name = "changeGoogleSpreadSheetsAccountToolStripMenuItem";
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Text = "Change Google Spreadsheet Account";
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Click += new System.EventHandler(this.ChangeGoogleSpreadSheetsAccountToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(668, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Available EMails";
            // 
            // EMailsList_ListView
            // 
            this.EMailsList_ListView.FormattingEnabled = true;
            this.EMailsList_ListView.Location = new System.Drawing.Point(671, 106);
            this.EMailsList_ListView.Name = "EMailsList_ListView";
            this.EMailsList_ListView.Size = new System.Drawing.Size(173, 225);
            this.EMailsList_ListView.TabIndex = 7;
            // 
            // Lectures_GridView
            // 
            this.Lectures_GridView.AllowUserToAddRows = false;
            this.Lectures_GridView.AllowUserToDeleteRows = false;
            this.Lectures_GridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Lectures_GridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Lectures_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Lectures_GridView.Location = new System.Drawing.Point(6, 6);
            this.Lectures_GridView.Name = "Lectures_GridView";
            this.Lectures_GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Lectures_GridView.Size = new System.Drawing.Size(633, 437);
            this.Lectures_GridView.TabIndex = 8;
            this.Lectures_GridView.SelectionChanged += new System.EventHandler(this.Lectures_GridView_SelectionChanged);
            // 
            // StartEventButton
            // 
            this.StartEventButton.BackColor = System.Drawing.Color.LimeGreen;
            this.StartEventButton.FlatAppearance.BorderSize = 0;
            this.StartEventButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartEventButton.Location = new System.Drawing.Point(671, 434);
            this.StartEventButton.Margin = new System.Windows.Forms.Padding(0);
            this.StartEventButton.Name = "StartEventButton";
            this.StartEventButton.Size = new System.Drawing.Size(128, 36);
            this.StartEventButton.TabIndex = 10;
            this.StartEventButton.Text = "Start selected stream";
            this.StartEventButton.UseVisualStyleBackColor = false;
            this.StartEventButton.Click += new System.EventHandler(this.StartLiveEvent_Click);
            // 
            // EndEventButton
            // 
            this.EndEventButton.FlatAppearance.BorderSize = 0;
            this.EndEventButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EndEventButton.Location = new System.Drawing.Point(671, 478);
            this.EndEventButton.Name = "EndEventButton";
            this.EndEventButton.Size = new System.Drawing.Size(128, 36);
            this.EndEventButton.TabIndex = 12;
            this.EndEventButton.Text = "End selected stream";
            this.EndEventButton.UseVisualStyleBackColor = true;
            this.EndEventButton.Click += new System.EventHandler(this.EndLiveEventButton_Click);
            // 
            // CancelEventButton
            // 
            this.CancelEventButton.FlatAppearance.BorderSize = 0;
            this.CancelEventButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelEventButton.Location = new System.Drawing.Point(671, 522);
            this.CancelEventButton.Name = "CancelEventButton";
            this.CancelEventButton.Size = new System.Drawing.Size(128, 36);
            this.CancelEventButton.TabIndex = 13;
            this.CancelEventButton.Text = "Cancel current operation";
            this.CancelEventButton.UseVisualStyleBackColor = true;
            this.CancelEventButton.Click += new System.EventHandler(this.CancelLiveEventCreationButton_Click);
            // 
            // CopyToClipboardButton
            // 
            this.CopyToClipboardButton.Location = new System.Drawing.Point(671, 337);
            this.CopyToClipboardButton.Name = "CopyToClipboardButton";
            this.CopyToClipboardButton.Size = new System.Drawing.Size(170, 23);
            this.CopyToClipboardButton.TabIndex = 14;
            this.CopyToClipboardButton.Text = "Copy to clipboard";
            this.CopyToClipboardButton.UseVisualStyleBackColor = true;
            this.CopyToClipboardButton.Click += new System.EventHandler(this.CopyToClipboardButton_Click);
            // 
            // AccountsList_ComboBox
            // 
            this.AccountsList_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountsList_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AccountsList_ComboBox.FormattingEnabled = true;
            this.AccountsList_ComboBox.Location = new System.Drawing.Point(673, 40);
            this.AccountsList_ComboBox.Name = "AccountsList_ComboBox";
            this.AccountsList_ComboBox.Size = new System.Drawing.Size(171, 21);
            this.AccountsList_ComboBox.TabIndex = 15;
            this.AccountsList_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Accounts_List_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(670, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Youtube accounts";
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "Broadcasts Schedule";
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // SelectedBroadcastSettingsLink
            // 
            this.SelectedBroadcastSettingsLink.AutoSize = true;
            this.SelectedBroadcastSettingsLink.Enabled = false;
            this.SelectedBroadcastSettingsLink.Location = new System.Drawing.Point(671, 369);
            this.SelectedBroadcastSettingsLink.Name = "SelectedBroadcastSettingsLink";
            this.SelectedBroadcastSettingsLink.Size = new System.Drawing.Size(138, 13);
            this.SelectedBroadcastSettingsLink.TabIndex = 17;
            this.SelectedBroadcastSettingsLink.TabStop = true;
            this.SelectedBroadcastSettingsLink.Text = "Selected broadcast settings";
            this.SelectedBroadcastSettingsLink.VisitedLinkColor = System.Drawing.Color.Blue;
            this.SelectedBroadcastSettingsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.BroadcastSettingsLink_LinkClicked);
            // 
            // CancelAuth
            // 
            this.CancelAuth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelAuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelAuth.Location = new System.Drawing.Point(796, 64);
            this.CancelAuth.Margin = new System.Windows.Forms.Padding(0);
            this.CancelAuth.Name = "CancelAuth";
            this.CancelAuth.Size = new System.Drawing.Size(48, 20);
            this.CancelAuth.TabIndex = 18;
            this.CancelAuth.Text = "Cancel";
            this.CancelAuth.UseVisualStyleBackColor = true;
            this.CancelAuth.Visible = false;
            this.CancelAuth.Click += new System.EventHandler(this.CancelAuth_Click);
            // 
            // CurrentStatus
            // 
            this.CurrentStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentStatusLabel});
            this.CurrentStatus.Location = new System.Drawing.Point(0, 566);
            this.CurrentStatus.Name = "CurrentStatus";
            this.CurrentStatus.Size = new System.Drawing.Size(853, 22);
            this.CurrentStatus.TabIndex = 19;
            this.CurrentStatus.Text = "statusStrip1";
            // 
            // CurrentStatusLabel
            // 
            this.CurrentStatusLabel.Name = "CurrentStatusLabel";
            this.CurrentStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ListOfStreams_Button
            // 
            this.ListOfStreams_Button.BackColor = System.Drawing.Color.LimeGreen;
            this.ListOfStreams_Button.FlatAppearance.BorderSize = 0;
            this.ListOfStreams_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ListOfStreams_Button.Location = new System.Drawing.Point(671, 391);
            this.ListOfStreams_Button.Margin = new System.Windows.Forms.Padding(0);
            this.ListOfStreams_Button.Name = "ListOfStreams_Button";
            this.ListOfStreams_Button.Size = new System.Drawing.Size(128, 36);
            this.ListOfStreams_Button.TabIndex = 20;
            this.ListOfStreams_Button.Text = "Create list of streams";
            this.ListOfStreams_Button.UseVisualStyleBackColor = true;
            this.ListOfStreams_Button.Click += new System.EventHandler(this.ListOfStreams_Button_Click);
            // 
            // ScheduledBroadcasts_GridView
            // 
            this.ScheduledBroadcasts_GridView.AllowUserToAddRows = false;
            this.ScheduledBroadcasts_GridView.AllowUserToDeleteRows = false;
            this.ScheduledBroadcasts_GridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ScheduledBroadcasts_GridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ScheduledBroadcasts_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScheduledBroadcasts_GridView.Location = new System.Drawing.Point(6, 6);
            this.ScheduledBroadcasts_GridView.Name = "ScheduledBroadcasts_GridView";
            this.ScheduledBroadcasts_GridView.ReadOnly = true;
            this.ScheduledBroadcasts_GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ScheduledBroadcasts_GridView.Size = new System.Drawing.Size(633, 437);
            this.ScheduledBroadcasts_GridView.TabIndex = 21;
            // 
            // CurrentStreams_GridView
            // 
            this.CurrentStreams_GridView.AllowUserToAddRows = false;
            this.CurrentStreams_GridView.AllowUserToDeleteRows = false;
            this.CurrentStreams_GridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CurrentStreams_GridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CurrentStreams_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CurrentStreams_GridView.Location = new System.Drawing.Point(6, 6);
            this.CurrentStreams_GridView.Name = "CurrentStreams_GridView";
            this.CurrentStreams_GridView.ReadOnly = true;
            this.CurrentStreams_GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CurrentStreams_GridView.Size = new System.Drawing.Size(633, 437);
            this.CurrentStreams_GridView.TabIndex = 23;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.LecturesPage);
            this.tabControl.Controls.Add(this.ScheduledBroadcastsPage);
            this.tabControl.Controls.Add(this.BroadcastsOnlinePage);
            this.tabControl.Location = new System.Drawing.Point(12, 84);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(653, 475);
            this.tabControl.TabIndex = 25;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            tabControl.HandleCreated += new System.EventHandler(this.TabControl_HandleCreated);
            // 
            // LecturesPage
            // 
            this.LecturesPage.Controls.Add(this.Lectures_GridView);
            this.LecturesPage.Location = new System.Drawing.Point(4, 22);
            this.LecturesPage.Name = "LecturesPage";
            this.LecturesPage.Padding = new System.Windows.Forms.Padding(3);
            this.LecturesPage.Size = new System.Drawing.Size(645, 449);
            this.LecturesPage.TabIndex = 0;
            this.LecturesPage.Text = "Lectures";
            this.LecturesPage.UseVisualStyleBackColor = true;
            // 
            // ScheduledBroadcastsPage
            // 
            this.ScheduledBroadcastsPage.Controls.Add(this.ScheduledBroadcasts_GridView);
            this.ScheduledBroadcastsPage.Location = new System.Drawing.Point(4, 22);
            this.ScheduledBroadcastsPage.Name = "ScheduledBroadcastsPage";
            this.ScheduledBroadcastsPage.Padding = new System.Windows.Forms.Padding(3);
            this.ScheduledBroadcastsPage.Size = new System.Drawing.Size(645, 449);
            this.ScheduledBroadcastsPage.TabIndex = 1;
            this.ScheduledBroadcastsPage.Text = "Scheduled broadcasts";
            this.ScheduledBroadcastsPage.UseVisualStyleBackColor = true;
            // 
            // BroadcastsOnlinePage
            // 
            this.BroadcastsOnlinePage.Controls.Add(this.CurrentStreams_GridView);
            this.BroadcastsOnlinePage.Location = new System.Drawing.Point(4, 22);
            this.BroadcastsOnlinePage.Name = "BroadcastsOnlinePage";
            this.BroadcastsOnlinePage.Padding = new System.Windows.Forms.Padding(3);
            this.BroadcastsOnlinePage.Size = new System.Drawing.Size(645, 449);
            this.BroadcastsOnlinePage.TabIndex = 2;
            this.BroadcastsOnlinePage.Text = "Broadcasts Online";
            this.BroadcastsOnlinePage.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            // 
            // BroadcastsScheduleClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 588);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.ListOfStreams_Button);
            this.Controls.Add(this.CurrentStatus);
            this.Controls.Add(this.CancelAuth);
            this.Controls.Add(this.SelectedBroadcastSettingsLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AccountsList_ComboBox);
            this.Controls.Add(this.CopyToClipboardButton);
            this.Controls.Add(this.CancelEventButton);
            this.Controls.Add(this.EndEventButton);
            this.Controls.Add(this.StartEventButton);
            this.Controls.Add(this.EMailsList_ListView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CoursesList_ComboBox);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "BroadcastsScheduleClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Broadcasts Schedule";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BroadcastsScheduleClass_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BroadcastsScheduleClass_FormClosed);
            this.Load += new System.EventHandler(this.BroadcastsScheduleClass_Load);
            this.Resize += new System.EventHandler(this.BroadcastsScheduleClass_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Lectures_GridView)).EndInit();
            this.CurrentStatus.ResumeLayout(false);
            this.CurrentStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduledBroadcasts_GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentStreams_GridView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.LecturesPage.ResumeLayout(false);
            this.ScheduledBroadcastsPage.ResumeLayout(false);
            this.BroadcastsOnlinePage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.ComboBox CoursesList_ComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox EMailsList_ListView;
        private System.Windows.Forms.DataGridView Lectures_GridView;
        private AbortableBackgroundWorker backgroundWorker;
        private System.Windows.Forms.Button StartEventButton;
        private System.Windows.Forms.Button EndEventButton;
        private System.Windows.Forms.Button CancelEventButton;
        private System.Windows.Forms.ToolStripMenuItem editCoursesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editLecturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editEmailsToolStripMenuItem;
        private System.Windows.Forms.Button CopyToClipboardButton;
        private System.Windows.Forms.ComboBox AccountsList_ComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem editTablesIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editAccountsToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.LinkLabel SelectedBroadcastSettingsLink;
        private System.Windows.Forms.Button CancelAuth;
        private System.Windows.Forms.StatusStrip CurrentStatus;
        private System.Windows.Forms.ToolStripStatusLabel CurrentStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem changeGoogleSpreadSheetsAccountToolStripMenuItem;
        private System.Windows.Forms.Button ListOfStreams_Button;
        private System.Windows.Forms.DataGridView ScheduledBroadcasts_GridView;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.DataGridView CurrentStreams_GridView;
        private AbortableBackgroundWorker abortableBackgroundWorker1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage LecturesPage;
        private System.Windows.Forms.TabPage ScheduledBroadcastsPage;
        private System.Windows.Forms.TabPage BroadcastsOnlinePage;
    }
}

