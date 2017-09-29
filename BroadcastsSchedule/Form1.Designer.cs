namespace BroadcastsSchedule
{
    partial class BroadcastsSchedule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BroadcastsSchedule));
            this.UpdateList = new System.Windows.Forms.Button();
            this.Courses_List = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EMails_List = new System.Windows.Forms.ListBox();
            this.LecturesGrid = new System.Windows.Forms.DataGridView();
            this.StartStream = new System.Windows.Forms.Button();
            this.CreateBroadcast = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LecturesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdateList
            // 
            this.UpdateList.Image = ((System.Drawing.Image)(resources.GetObject("UpdateList.Image")));
            this.UpdateList.Location = new System.Drawing.Point(139, 40);
            this.UpdateList.Name = "UpdateList";
            this.UpdateList.Size = new System.Drawing.Size(25, 23);
            this.UpdateList.TabIndex = 0;
            this.UpdateList.TabStop = false;
            this.UpdateList.UseVisualStyleBackColor = true;
            this.UpdateList.Click += new System.EventHandler(this.UpdateList_Click);
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1021, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
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
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(406, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Available EMails";
            // 
            // EMails_List
            // 
            this.EMails_List.FormattingEnabled = true;
            this.EMails_List.Location = new System.Drawing.Point(409, 100);
            this.EMails_List.Name = "EMails_List";
            this.EMails_List.Size = new System.Drawing.Size(223, 134);
            this.EMails_List.TabIndex = 7;
            // 
            // LecturesGrid
            // 
            this.LecturesGrid.AllowUserToAddRows = false;
            this.LecturesGrid.AllowUserToDeleteRows = false;
            this.LecturesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LecturesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.LecturesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LecturesGrid.Location = new System.Drawing.Point(12, 100);
            this.LecturesGrid.Name = "LecturesGrid";
            this.LecturesGrid.Size = new System.Drawing.Size(391, 303);
            this.LecturesGrid.TabIndex = 8;
            // 
            // StartStream
            // 
            this.StartStream.Location = new System.Drawing.Point(472, 299);
            this.StartStream.Name = "StartStream";
            this.StartStream.Size = new System.Drawing.Size(108, 24);
            this.StartStream.TabIndex = 9;
            this.StartStream.Text = "Start Stream";
            this.StartStream.UseVisualStyleBackColor = true;
            this.StartStream.Click += new System.EventHandler(this.StartStream_Click);
            // 
            // CreateBroadcast
            // 
            this.CreateBroadcast.Location = new System.Drawing.Point(472, 270);
            this.CreateBroadcast.Name = "CreateBroadcast";
            this.CreateBroadcast.Size = new System.Drawing.Size(108, 23);
            this.CreateBroadcast.TabIndex = 10;
            this.CreateBroadcast.Text = "Create Broadcast";
            this.CreateBroadcast.UseVisualStyleBackColor = true;
            this.CreateBroadcast.Click += new System.EventHandler(this.CreateBroadcast_Click);
            // 
            // BroadcastsSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 407);
            this.Controls.Add(this.CreateBroadcast);
            this.Controls.Add(this.StartStream);
            this.Controls.Add(this.LecturesGrid);
            this.Controls.Add(this.EMails_List);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Courses_List);
            this.Controls.Add(this.UpdateList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BroadcastsSchedule";
            this.Text = "Broadcasts Schedule";
            this.Load += new System.EventHandler(this.BroadcastsSchedule_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LecturesGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateList;
        private System.Windows.Forms.ComboBox Courses_List;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox EMails_List;
        private System.Windows.Forms.DataGridView LecturesGrid;
        private System.Windows.Forms.Button StartStream;
        private System.Windows.Forms.Button CreateBroadcast;
    }
}

