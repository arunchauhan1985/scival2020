namespace Scival.WebWatcher
{
    partial class Grouping
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Grouping));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnGroup = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlBatch = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstrighjt = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstLeft = new System.Windows.Forms.ListBox();
            this.grdURL = new System.Windows.Forms.DataGridView();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduleid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BATCH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlFunding = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnexit = new System.Windows.Forms.Button();
            this.btntoggle = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnHome = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdURL)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(1, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1344, 546);
            this.panel1.TabIndex = 12;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.splitContainer1.Panel1.Controls.Add(this.webBrowser1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.LightGray;
            this.splitContainer1.Panel2.Controls.Add(this.btnGroup);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.ddlBatch);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.grdURL);
            this.splitContainer1.Panel2.Controls.Add(this.radioButton2);
            this.splitContainer1.Panel2.Controls.Add(this.radioButton1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.ddlFunding);
            this.splitContainer1.Size = new System.Drawing.Size(1026, 546);
            this.splitContainer1.SplitterDistance = 341;
            this.splitContainer1.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(-1, -1);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(342, 545);
            this.webBrowser1.TabIndex = 0;
            // 
            // btnGroup
            // 
            this.btnGroup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGroup.BackgroundImage")));
            this.btnGroup.Location = new System.Drawing.Point(556, 432);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(105, 23);
            this.btnGroup.TabIndex = 37;
            this.btnGroup.Text = "Group";
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Batch";
            // 
            // ddlBatch
            // 
            this.ddlBatch.FormattingEnabled = true;
            this.ddlBatch.Location = new System.Drawing.Point(119, 40);
            this.ddlBatch.Name = "ddlBatch";
            this.ddlBatch.Size = new System.Drawing.Size(268, 21);
            this.ddlBatch.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Silver;
            this.groupBox2.Controls.Add(this.lstrighjt);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox2.Location = new System.Drawing.Point(349, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(319, 341);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Grouped URLs";
            // 
            // lstrighjt
            // 
            this.lstrighjt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstrighjt.FormattingEnabled = true;
            this.lstrighjt.ItemHeight = 18;
            this.lstrighjt.Location = new System.Drawing.Point(8, 23);
            this.lstrighjt.Name = "lstrighjt";
            this.lstrighjt.Size = new System.Drawing.Size(305, 310);
            this.lstrighjt.TabIndex = 5;
            this.lstrighjt.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstrighjt_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.lstLeft);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Location = new System.Drawing.Point(19, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 341);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "URLs for Group";
            // 
            // lstLeft
            // 
            this.lstLeft.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLeft.FormattingEnabled = true;
            this.lstLeft.ItemHeight = 18;
            this.lstLeft.Location = new System.Drawing.Point(6, 23);
            this.lstLeft.Name = "lstLeft";
            this.lstLeft.Size = new System.Drawing.Size(312, 310);
            this.lstLeft.TabIndex = 5;
            this.lstLeft.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstLeft_MouseDoubleClick);
            this.lstLeft.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstLeft_DrawItem);
            this.lstLeft.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstLeft_MouseClick);
            this.lstLeft.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstLeft_KeyUp);
            // 
            // grdURL
            // 
            this.grdURL.AllowUserToAddRows = false;
            this.grdURL.AllowUserToDeleteRows = false;
            this.grdURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.grdURL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdURL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.URL,
            this.ID,
            this.moduleid,
            this.BATCH});
            this.grdURL.Location = new System.Drawing.Point(19, 465);
            this.grdURL.Name = "grdURL";
            this.grdURL.ReadOnly = true;
            this.grdURL.RowHeadersVisible = false;
            this.grdURL.Size = new System.Drawing.Size(649, 66);
            this.grdURL.TabIndex = 4;
            this.grdURL.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdURL_CellDoubleClick);
            this.grdURL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdURL_KeyUp);
            // 
            // URL
            // 
            this.URL.DataPropertyName = "URL";
            this.URL.HeaderText = "URL";
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            this.URL.Width = 530;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "id";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // moduleid
            // 
            this.moduleid.DataPropertyName = "moduleid";
            this.moduleid.HeaderText = "Module Id";
            this.moduleid.Name = "moduleid";
            this.moduleid.ReadOnly = true;
            this.moduleid.Visible = false;
            // 
            // BATCH
            // 
            this.BATCH.DataPropertyName = "BATCH";
            this.BATCH.HeaderText = "BATCH";
            this.BATCH.Name = "BATCH";
            this.BATCH.ReadOnly = true;
            this.BATCH.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(529, 15);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(79, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Opportunity";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(442, 15);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(55, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Award";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "FundingBody";
            // 
            // ddlFunding
            // 
            this.ddlFunding.FormattingEnabled = true;
            this.ddlFunding.Location = new System.Drawing.Point(119, 13);
            this.ddlFunding.Name = "ddlFunding";
            this.ddlFunding.Size = new System.Drawing.Size(268, 21);
            this.ddlFunding.TabIndex = 0;
            this.ddlFunding.SelectedIndexChanged += new System.EventHandler(this.ddlFunding_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 622);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1028, 22);
            this.statusStrip1.TabIndex = 27;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(208, 17);
            this.toolStripStatusLabel1.Text = "© 2011 Aptara, Inc. All rights reserved.";
            // 
            // btnexit
            // 
            this.btnexit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnexit.BackgroundImage")));
            this.btnexit.Location = new System.Drawing.Point(955, 46);
            this.btnexit.Name = "btnexit";
            this.btnexit.Size = new System.Drawing.Size(61, 23);
            this.btnexit.TabIndex = 22;
            this.btnexit.Text = "SignOut";
            this.btnexit.UseVisualStyleBackColor = true;
            this.btnexit.Visible = false;
            this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
            // 
            // btntoggle
            // 
            this.btntoggle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btntoggle.BackgroundImage")));
            this.btntoggle.Location = new System.Drawing.Point(387, 41);
            this.btntoggle.Name = "btntoggle";
            this.btntoggle.Size = new System.Drawing.Size(75, 23);
            this.btntoggle.TabIndex = 29;
            this.btntoggle.Text = "&Toggle";
            this.btntoggle.UseVisualStyleBackColor = true;
            this.btntoggle.Click += new System.EventHandler(this.btntoggle_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(467, 9);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(64, 24);
            this.menuStrip2.TabIndex = 34;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // toggleToolStripMenuItem
            // 
            this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
            this.toggleToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Tab)));
            this.toggleToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.toggleToolStripMenuItem.Text = "Toggle";
            this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleToolStripMenuItem_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHome.BackgroundImage")));
            this.btnHome.Location = new System.Drawing.Point(880, 46);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(61, 23);
            this.btnHome.TabIndex = 35;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Visible = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // Grouping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1028, 644);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.btnexit);
            this.Controls.Add(this.btntoggle);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            
            this.Name = "Grouping";
            this.Text = "Retain And Delete";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Awards_FormClosing);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdURL)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnexit;
        private System.Windows.Forms.Button btntoggle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlFunding;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.DataGridView grdURL;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.ListBox lstLeft;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstrighjt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlBatch;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn URL;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduleid;
        private System.Windows.Forms.DataGridViewTextBoxColumn BATCH;
    }
}