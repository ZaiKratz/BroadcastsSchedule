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
            this.Courses_List = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCoursesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editLecturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editEmailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTablesIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editAccountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EMails_List = new System.Windows.Forms.ListBox();
            this.LecturesGrid = new System.Windows.Forms.DataGridView();
            this.StartEventButton = new System.Windows.Forms.Button();
            this.EndEventButton = new System.Windows.Forms.Button();
            this.CancelEventButton = new System.Windows.Forms.Button();
            this.CopyToClipboardButton = new System.Windows.Forms.Button();
            this.Accounts_List = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.BroadcastSettingsLink = new System.Windows.Forms.LinkLabel();
            this.CancelAuth = new System.Windows.Forms.Button();
            this.CurrentStatus = new System.Windows.Forms.StatusStrip();
            this.CurrentStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new BroadcastsSchedule.AbortableBackgroundWorker();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LecturesGrid)).BeginInit();
            this.CurrentStatus.SuspendLayout();
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
            // Courses_List
            // 
            this.Courses_List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Courses_List.FormattingEnabled = true;
            this.Courses_List.Location = new System.Drawing.Point(12, 41);
            this.Courses_List.Name = "Courses_List";
            this.Courses_List.Size = new System.Drawing.Size(121, 21);
            this.Courses_List.TabIndex = 1;
            this.Courses_List.SelectedIndexChanged += new System.EventHandler(this.CoursesList_SelectedIndexChanged);
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
            this.editToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(864, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
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
            this.editEmailsToolStripMenuItem.Click += new System.EventHandler(this.editEmailsToolStripMenuItem_Click);
            // 
            // editTablesIDToolStripMenuItem
            // 
            this.editTablesIDToolStripMenuItem.Name = "editTablesIDToolStripMenuItem";
            this.editTablesIDToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.editTablesIDToolStripMenuItem.Text = "Edit IDs";
            this.editTablesIDToolStripMenuItem.Click += new System.EventHandler(this.editTablesIDToolStripMenuItem_Click);
            // 
            // editAccountsToolStripMenuItem
            // 
            this.editAccountsToolStripMenuItem.Name = "editAccountsToolStripMenuItem";
            this.editAccountsToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.editAccountsToolStripMenuItem.Text = "Edit Accounts";
            this.editAccountsToolStripMenuItem.Click += new System.EventHandler(this.editAccountsToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lectures";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(538, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Available EMails";
            // 
            // EMails_List
            // 
            this.EMails_List.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EMails_List.FormattingEnabled = true;
            this.EMails_List.Location = new System.Drawing.Point(537, 100);
            this.EMails_List.Name = "EMails_List";
            this.EMails_List.Size = new System.Drawing.Size(315, 134);
            this.EMails_List.TabIndex = 7;
            // 
            // LecturesGrid
            // 
            this.LecturesGrid.AllowUserToAddRows = false;
            this.LecturesGrid.AllowUserToDeleteRows = false;
            this.LecturesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LecturesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LecturesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.LecturesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LecturesGrid.Location = new System.Drawing.Point(12, 100);
            this.LecturesGrid.Name = "LecturesGrid";
            this.LecturesGrid.Size = new System.Drawing.Size(516, 304);
            this.LecturesGrid.TabIndex = 8;
            // 
            // StartEventButton
            // 
            this.StartEventButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartEventButton.BackColor = System.Drawing.Color.LimeGreen;
            this.StartEventButton.FlatAppearance.BorderSize = 0;
            this.StartEventButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartEventButton.Location = new System.Drawing.Point(537, 340);
            this.StartEventButton.Name = "StartEventButton";
            this.StartEventButton.Size = new System.Drawing.Size(90, 28);
            this.StartEventButton.TabIndex = 10;
            this.StartEventButton.Text = "Start Stream";
            this.StartEventButton.UseVisualStyleBackColor = true;
            this.StartEventButton.Click += new System.EventHandler(this.StartLiveEvent_Click);
            // 
            // EndEventButton
            // 
            this.EndEventButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EndEventButton.FlatAppearance.BorderSize = 0;
            this.EndEventButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EndEventButton.Location = new System.Drawing.Point(537, 376);
            this.EndEventButton.Name = "EndEventButton";
            this.EndEventButton.Size = new System.Drawing.Size(90, 28);
            this.EndEventButton.TabIndex = 12;
            this.EndEventButton.Text = "End Stream";
            this.EndEventButton.UseVisualStyleBackColor = true;
            this.EndEventButton.Click += new System.EventHandler(this.EndLiveEventButton_Click);
            // 
            // CancelEventButton
            // 
            this.CancelEventButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelEventButton.FlatAppearance.BorderSize = 0;
            this.CancelEventButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelEventButton.Location = new System.Drawing.Point(633, 340);
            this.CancelEventButton.Name = "CancelEventButton";
            this.CancelEventButton.Size = new System.Drawing.Size(75, 28);
            this.CancelEventButton.TabIndex = 13;
            this.CancelEventButton.Text = "Cancel";
            this.CancelEventButton.UseVisualStyleBackColor = true;
            this.CancelEventButton.Click += new System.EventHandler(this.CancelLiveEventCreationButton_Click);
            // 
            // CopyToClipboardButton
            // 
            this.CopyToClipboardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyToClipboardButton.Location = new System.Drawing.Point(537, 237);
            this.CopyToClipboardButton.Name = "CopyToClipboardButton";
            this.CopyToClipboardButton.Size = new System.Drawing.Size(97, 23);
            this.CopyToClipboardButton.TabIndex = 14;
            this.CopyToClipboardButton.Text = "Copy to clipboard";
            this.CopyToClipboardButton.UseVisualStyleBackColor = true;
            this.CopyToClipboardButton.Click += new System.EventHandler(this.CopyToClipboardButton_Click);
            // 
            // Accounts_List
            // 
            this.Accounts_List.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Accounts_List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Accounts_List.FormattingEnabled = true;
            this.Accounts_List.Location = new System.Drawing.Point(681, 40);
            this.Accounts_List.Name = "Accounts_List";
            this.Accounts_List.Size = new System.Drawing.Size(171, 21);
            this.Accounts_List.TabIndex = 15;
            this.Accounts_List.SelectedIndexChanged += new System.EventHandler(this.Accounts_List_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(678, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Youtube Accounts";
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "Broadcasts Schedule";
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // BroadcastSettingsLink
            // 
            this.BroadcastSettingsLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BroadcastSettingsLink.AutoSize = true;
            this.BroadcastSettingsLink.Enabled = false;
            this.BroadcastSettingsLink.Location = new System.Drawing.Point(756, 237);
            this.BroadcastSettingsLink.Name = "BroadcastSettingsLink";
            this.BroadcastSettingsLink.Size = new System.Drawing.Size(96, 13);
            this.BroadcastSettingsLink.TabIndex = 17;
            this.BroadcastSettingsLink.TabStop = true;
            this.BroadcastSettingsLink.Text = "Broadcast Settings";
            this.BroadcastSettingsLink.VisitedLinkColor = System.Drawing.Color.Blue;
            this.BroadcastSettingsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.BroadcastSettingsLink_LinkClicked);
            // 
            // CancelAuth
            // 
            this.CancelAuth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelAuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelAuth.Location = new System.Drawing.Point(804, 64);
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
            this.CurrentStatus.Location = new System.Drawing.Point(0, 407);
            this.CurrentStatus.Name = "CurrentStatus";
            this.CurrentStatus.Size = new System.Drawing.Size(864, 22);
            this.CurrentStatus.TabIndex = 19;
            this.CurrentStatus.Text = "statusStrip1";
            // 
            // CurrentStatusLabel
            // 
            this.CurrentStatusLabel.Name = "CurrentStatusLabel";
            this.CurrentStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // changeGoogleSpreadSheetsAccountToolStripMenuItem
            // 
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Name = "changeGoogleSpreadSheetsAccountToolStripMenuItem";
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Text = "Change Google Spreadsheet Account";
            this.changeGoogleSpreadSheetsAccountToolStripMenuItem.Click += new System.EventHandler(this.changeGoogleSpreadSheetsAccountToolStripMenuItem_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            // 
            // BroadcastsScheduleClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 429);
            this.Controls.Add(this.CurrentStatus);
            this.Controls.Add(this.CancelAuth);
            this.Controls.Add(this.BroadcastSettingsLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Accounts_List);
            this.Controls.Add(this.CopyToClipboardButton);
            this.Controls.Add(this.CancelEventButton);
            this.Controls.Add(this.EndEventButton);
            this.Controls.Add(this.StartEventButton);
            this.Controls.Add(this.LecturesGrid);
            this.Controls.Add(this.EMails_List);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Courses_List);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(880, 445);
            this.Name = "BroadcastsScheduleClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Broadcasts Schedule";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BroadcastsScheduleClass_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BroadcastsScheduleClass_FormClosed);
            this.Load += new System.EventHandler(this.BroadcastsScheduleClass_Load);
            this.Resize += new System.EventHandler(this.BroadcastsScheduleClass_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LecturesGrid)).EndInit();
            this.CurrentStatus.ResumeLayout(false);
            this.CurrentStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.ComboBox Courses_List;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox EMails_List;
        private System.Windows.Forms.DataGridView LecturesGrid;
        private AbortableBackgroundWorker backgroundWorker;
        private System.Windows.Forms.Button StartEventButton;
        private System.Windows.Forms.Button EndEventButton;
        private System.Windows.Forms.Button CancelEventButton;
        private System.Windows.Forms.ToolStripMenuItem editCoursesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editLecturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editEmailsToolStripMenuItem;
        private System.Windows.Forms.Button CopyToClipboardButton;
        private System.Windows.Forms.ComboBox Accounts_List;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem editTablesIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editAccountsToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.LinkLabel BroadcastSettingsLink;
        private System.Windows.Forms.Button CancelAuth;
        private System.Windows.Forms.StatusStrip CurrentStatus;
        private System.Windows.Forms.ToolStripStatusLabel CurrentStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem changeGoogleSpreadSheetsAccountToolStripMenuItem;
    }
}

