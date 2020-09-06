namespace Scival.WebWatcher
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnImportLevel1 = new System.Windows.Forms.Button();
            this.btnRtnDel = new System.Windows.Forms.Button();
            this.btnGrpURL = new System.Windows.Forms.Button();
            this.btnRtnRprt = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 543);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(880, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(208, 17);
            this.toolStripStatusLabel1.Text = "© 2011 Aptara, Inc. All rights reserved.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-12, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(149, 83);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnImportLevel1
            // 
            this.btnImportLevel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnImportLevel1.BackgroundImage")));
            this.btnImportLevel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnImportLevel1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportLevel1.Location = new System.Drawing.Point(41, 31);
            this.btnImportLevel1.Name = "btnImportLevel1";
            this.btnImportLevel1.Size = new System.Drawing.Size(220, 57);
            this.btnImportLevel1.TabIndex = 2;
            this.btnImportLevel1.Text = "Import Level-I";
            this.btnImportLevel1.UseVisualStyleBackColor = true;
            this.btnImportLevel1.Click += new System.EventHandler(this.btnImportLevel1_Click);
            // 
            // btnRtnDel
            // 
            this.btnRtnDel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRtnDel.BackgroundImage")));
            this.btnRtnDel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRtnDel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRtnDel.Location = new System.Drawing.Point(41, 109);
            this.btnRtnDel.Name = "btnRtnDel";
            this.btnRtnDel.Size = new System.Drawing.Size(220, 57);
            this.btnRtnDel.TabIndex = 3;
            this.btnRtnDel.Text = "Retain/Delete";
            this.btnRtnDel.UseVisualStyleBackColor = true;
            this.btnRtnDel.Click += new System.EventHandler(this.btnRtnDel_Click);
            // 
            // btnGrpURL
            // 
            this.btnGrpURL.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGrpURL.BackgroundImage")));
            this.btnGrpURL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnGrpURL.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrpURL.Location = new System.Drawing.Point(41, 190);
            this.btnGrpURL.Name = "btnGrpURL";
            this.btnGrpURL.Size = new System.Drawing.Size(220, 57);
            this.btnGrpURL.TabIndex = 4;
            this.btnGrpURL.Text = "Group URLs";
            this.btnGrpURL.UseVisualStyleBackColor = true;
            this.btnGrpURL.Click += new System.EventHandler(this.btnGrpURL_Click);
            // 
            // btnRtnRprt
            // 
            this.btnRtnRprt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRtnRprt.BackgroundImage")));
            this.btnRtnRprt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRtnRprt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRtnRprt.Location = new System.Drawing.Point(41, 275);
            this.btnRtnRprt.Name = "btnRtnRprt";
            this.btnRtnRprt.Size = new System.Drawing.Size(220, 57);
            this.btnRtnRprt.TabIndex = 5;
            this.btnRtnRprt.Text = "Retain Report";
            this.btnRtnRprt.UseVisualStyleBackColor = true;
            this.btnRtnRprt.Click += new System.EventHandler(this.btnRtnRprt_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnImportLevel1);
            this.groupBox1.Controls.Add(this.btnRtnRprt);
            this.groupBox1.Controls.Add(this.btnRtnDel);
            this.groupBox1.Controls.Add(this.btnGrpURL);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(142, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 369);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Web Watcher Level-1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(481, 106);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 369);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Web Watcher Level-2";
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(45, 115);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(220, 57);
            this.button2.TabIndex = 3;
            this.button2.Text = "Export Level-2 Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(45, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(220, 57);
            this.button1.TabIndex = 2;
            this.button1.Text = "Import Level-2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(880, 565);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            
            this.Name = "DashBoard";
            this.Text = "DashBoard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DashBoard_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnImportLevel1;
        private System.Windows.Forms.Button btnRtnDel;
        private System.Windows.Forms.Button btnGrpURL;
        private System.Windows.Forms.Button btnRtnRprt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}