namespace Scival
{
    partial class DashBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashBoard));
            this.btnstart = new System.Windows.Forms.Button();
            this.btnback = new System.Windows.Forms.Button();
            this.FOAname = new System.Windows.Forms.Label();
            this.imgFlow = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.imgMode = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PredictionImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.FundingBodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstComment = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.imgFlow)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgMode)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnstart
            // 
            this.btnstart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnstart.Location = new System.Drawing.Point(804, 498);
            this.btnstart.Name = "btnstart";
            this.btnstart.Size = new System.Drawing.Size(75, 24);
            this.btnstart.TabIndex = 2;
            this.btnstart.Text = "Start";
            this.btnstart.UseVisualStyleBackColor = true;
            this.btnstart.Click += new System.EventHandler(this.btnstart_Click);
            // 
            // btnback
            // 
            this.btnback.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnback.Location = new System.Drawing.Point(722, 498);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(75, 24);
            this.btnback.TabIndex = 3;
            this.btnback.Text = "Back";
            this.btnback.UseVisualStyleBackColor = true;
            this.btnback.Click += new System.EventHandler(this.btnback_Click);
            // 
            // FOAname
            // 
            this.FOAname.AutoSize = true;
            this.FOAname.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FOAname.ForeColor = System.Drawing.Color.Black;
            this.FOAname.Location = new System.Drawing.Point(23, 12);
            this.FOAname.Name = "FOAname";
            this.FOAname.Size = new System.Drawing.Size(152, 25);
            this.FOAname.TabIndex = 4;
            this.FOAname.Text = "Module(Detail)";
            // 
            // imgFlow
            // 
            this.imgFlow.Location = new System.Drawing.Point(27, 10);
            this.imgFlow.Name = "imgFlow";
            this.imgFlow.Size = new System.Drawing.Size(911, 39);
            this.imgFlow.TabIndex = 5;
            this.imgFlow.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.imgMode);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.imgFlow);
            this.panel1.Controls.Add(this.btnstart);
            this.panel1.Controls.Add(this.btnback);
            this.panel1.Location = new System.Drawing.Point(1, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(969, 567);
            this.panel1.TabIndex = 6;
            // 
            // imgMode
            // 
            this.imgMode.Location = new System.Drawing.Point(33, 55);
            this.imgMode.Name = "imgMode";
            this.imgMode.Size = new System.Drawing.Size(234, 26);
            this.imgMode.TabIndex = 7;
            this.imgMode.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Location = new System.Drawing.Point(27, 64);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(911, 414);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dashboard Details";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(88, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 375);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PredictionImage,
            this.FundingBodyName});
            this.dataGridView1.Enabled = false;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlText;
            this.dataGridView1.Location = new System.Drawing.Point(6, 20);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
            this.dataGridView1.Size = new System.Drawing.Size(350, 349);
            this.dataGridView1.TabIndex = 0;
            // 
            // PredictionImage
            // 
            this.PredictionImage.HeaderText = "";
            this.PredictionImage.Name = "PredictionImage";
            this.PredictionImage.Width = 50;
            // 
            // FundingBodyName
            // 
            this.FundingBodyName.DataPropertyName = "TAB";
            this.FundingBodyName.HeaderText = "";
            this.FundingBodyName.Name = "FundingBodyName";
            this.FundingBodyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FundingBodyName.Width = 280;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstComment);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox2.Location = new System.Drawing.Point(467, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 330);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Comments";
            // 
            // lstComment
            // 
            this.lstComment.BackColor = System.Drawing.Color.DimGray;
            this.lstComment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstComment.FormattingEnabled = true;
            this.lstComment.Location = new System.Drawing.Point(6, 17);
            this.lstComment.Margin = new System.Windows.Forms.Padding(20, 10, 10, 10);
            this.lstComment.Name = "lstComment";
            this.lstComment.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstComment.Size = new System.Drawing.Size(373, 303);
            this.lstComment.TabIndex = 1;
            this.lstComment.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstComment_DrawItem);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 617);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(969, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(208, 17);
            this.toolStripStatusLabel1.Text = "© 2011 Aptara, Inc. All rights reserved.";
            // 
            // DashBoard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(969, 639);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.FOAname);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "DashBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DashBoard_FormClosed);
            this.Load += new System.EventHandler(this.DashBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgFlow)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgMode)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnstart;
        private System.Windows.Forms.Button btnback;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label FOAname;
        private System.Windows.Forms.DataGridViewImageColumn PredictionImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundingBodyName;
        private System.Windows.Forms.PictureBox imgFlow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ListBox lstComment;
        private System.Windows.Forms.PictureBox imgMode;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}