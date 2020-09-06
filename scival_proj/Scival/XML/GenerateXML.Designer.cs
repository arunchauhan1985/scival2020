namespace Scival.XML
{
    partial class GenerateXML
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateXML));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCurrentLimits = new System.Windows.Forms.Label();
            this.lblCurrentLimit = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtgenrationLimit = new System.Windows.Forms.TextBox();
            this.btnFixLimit = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.TreeView1 = new System.Windows.Forms.TreeView();
            this.btnAWUpdate = new System.Windows.Forms.Button();
            this.btnAWDel = new System.Windows.Forms.Button();
            this.btnAWNew = new System.Windows.Forms.Button();
            this.btnOPPUpdate = new System.Windows.Forms.Button();
            this.btnOPPDel = new System.Windows.Forms.Button();
            this.btnOPPNew = new System.Windows.Forms.Button();
            this.btnFBUpdate = new System.Windows.Forms.Button();
            this.btnFBDel = new System.Windows.Forms.Button();
            this.btnFBNew = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnsignout = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlbtn = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlbtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.lblCurrentLimits);
            this.panel1.Controls.Add(this.lblCurrentLimit);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtgenrationLimit);
            this.panel1.Controls.Add(this.btnFixLimit);
            this.panel1.Controls.Add(this.pnlContent);
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1044, 579);
            this.panel1.TabIndex = 0;
            // 
            // lblCurrentLimits
            // 
            this.lblCurrentLimits.AutoSize = true;
            this.lblCurrentLimits.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrentLimits.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCurrentLimits.Location = new System.Drawing.Point(199, 25);
            this.lblCurrentLimits.Name = "lblCurrentLimits";
            this.lblCurrentLimits.Size = new System.Drawing.Size(47, 15);
            this.lblCurrentLimits.TabIndex = 41;
            this.lblCurrentLimits.Text = "label2";
            // 
            // lblCurrentLimit
            // 
            this.lblCurrentLimit.AutoSize = true;
            this.lblCurrentLimit.Location = new System.Drawing.Point(139, 15);
            this.lblCurrentLimit.Name = "lblCurrentLimit";
            this.lblCurrentLimit.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentLimit.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(3, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 15);
            this.label1.TabIndex = 37;
            this.label1.Text = "XML Genration Current Limit";
            // 
            // txtgenrationLimit
            // 
            this.txtgenrationLimit.Location = new System.Drawing.Point(249, 21);
            this.txtgenrationLimit.Name = "txtgenrationLimit";
            this.txtgenrationLimit.Size = new System.Drawing.Size(67, 20);
            this.txtgenrationLimit.TabIndex = 38;
            // 
            // btnFixLimit
            // 
            this.btnFixLimit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFixLimit.Location = new System.Drawing.Point(320, 19);
            this.btnFixLimit.Name = "btnFixLimit";
            this.btnFixLimit.Size = new System.Drawing.Size(76, 23);
            this.btnFixLimit.TabIndex = 39;
            this.btnFixLimit.Text = "Fix Limit";
            this.btnFixLimit.UseVisualStyleBackColor = true;
            this.btnFixLimit.Click += new System.EventHandler(this.btnFixLimit_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.Controls.Add(this.TreeView1);
            this.pnlContent.Controls.Add(this.btnAWUpdate);
            this.pnlContent.Controls.Add(this.btnAWDel);
            this.pnlContent.Controls.Add(this.btnAWNew);
            this.pnlContent.Controls.Add(this.btnOPPUpdate);
            this.pnlContent.Controls.Add(this.btnOPPDel);
            this.pnlContent.Controls.Add(this.btnOPPNew);
            this.pnlContent.Controls.Add(this.btnFBUpdate);
            this.pnlContent.Controls.Add(this.btnFBDel);
            this.pnlContent.Controls.Add(this.btnFBNew);
            this.pnlContent.Controls.Add(this.panel3);
            this.pnlContent.Location = new System.Drawing.Point(3, 45);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1038, 467);
            this.pnlContent.TabIndex = 0;
            // 
            // TreeView1
            // 
            this.TreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeView1.Location = new System.Drawing.Point(395, 2);
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Size = new System.Drawing.Size(642, 460);
            this.TreeView1.TabIndex = 24;
            this.TreeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView1_NodeMouseClick);
            // 
            // btnAWUpdate
            // 
            this.btnAWUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAWUpdate.BackgroundImage")));
            this.btnAWUpdate.Location = new System.Drawing.Point(214, 366);
            this.btnAWUpdate.Name = "btnAWUpdate";
            this.btnAWUpdate.Size = new System.Drawing.Size(179, 50);
            this.btnAWUpdate.TabIndex = 33;
            this.btnAWUpdate.UseVisualStyleBackColor = true;
            this.btnAWUpdate.Click += new System.EventHandler(this.btnAWUpdate_Click);
            // 
            // btnAWDel
            // 
            this.btnAWDel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAWDel.BackgroundImage")));
            this.btnAWDel.Location = new System.Drawing.Point(214, 414);
            this.btnAWDel.Name = "btnAWDel";
            this.btnAWDel.Size = new System.Drawing.Size(179, 51);
            this.btnAWDel.TabIndex = 32;
            this.btnAWDel.UseVisualStyleBackColor = true;
            this.btnAWDel.Click += new System.EventHandler(this.btnAWDel_Click);
            // 
            // btnAWNew
            // 
            this.btnAWNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAWNew.BackgroundImage")));
            this.btnAWNew.Location = new System.Drawing.Point(214, 314);
            this.btnAWNew.Name = "btnAWNew";
            this.btnAWNew.Size = new System.Drawing.Size(179, 54);
            this.btnAWNew.TabIndex = 31;
            this.btnAWNew.UseVisualStyleBackColor = true;
            this.btnAWNew.Click += new System.EventHandler(this.btnAWNew_Click);
            // 
            // btnOPPUpdate
            // 
            this.btnOPPUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOPPUpdate.BackgroundImage")));
            this.btnOPPUpdate.Location = new System.Drawing.Point(214, 207);
            this.btnOPPUpdate.Name = "btnOPPUpdate";
            this.btnOPPUpdate.Size = new System.Drawing.Size(179, 50);
            this.btnOPPUpdate.TabIndex = 30;
            this.btnOPPUpdate.UseVisualStyleBackColor = true;
            this.btnOPPUpdate.Click += new System.EventHandler(this.btnOPPUpdate_Click);
            // 
            // btnOPPDel
            // 
            this.btnOPPDel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOPPDel.BackgroundImage")));
            this.btnOPPDel.Location = new System.Drawing.Point(214, 255);
            this.btnOPPDel.Name = "btnOPPDel";
            this.btnOPPDel.Size = new System.Drawing.Size(179, 51);
            this.btnOPPDel.TabIndex = 29;
            this.btnOPPDel.UseVisualStyleBackColor = true;
            this.btnOPPDel.Click += new System.EventHandler(this.btnOPPDel_Click);
            // 
            // btnOPPNew
            // 
            this.btnOPPNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOPPNew.BackgroundImage")));
            this.btnOPPNew.Location = new System.Drawing.Point(214, 155);
            this.btnOPPNew.Name = "btnOPPNew";
            this.btnOPPNew.Size = new System.Drawing.Size(179, 54);
            this.btnOPPNew.TabIndex = 28;
            this.btnOPPNew.UseVisualStyleBackColor = true;
            this.btnOPPNew.Click += new System.EventHandler(this.btnOPPNew_Click);
            // 
            // btnFBUpdate
            // 
            this.btnFBUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFBUpdate.BackgroundImage")));
            this.btnFBUpdate.Location = new System.Drawing.Point(214, 51);
            this.btnFBUpdate.Name = "btnFBUpdate";
            this.btnFBUpdate.Size = new System.Drawing.Size(179, 50);
            this.btnFBUpdate.TabIndex = 27;
            this.btnFBUpdate.UseVisualStyleBackColor = true;
            this.btnFBUpdate.Click += new System.EventHandler(this.btnFBUpdate_Click);
            // 
            // btnFBDel
            // 
            this.btnFBDel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFBDel.BackgroundImage")));
            this.btnFBDel.Location = new System.Drawing.Point(214, 99);
            this.btnFBDel.Name = "btnFBDel";
            this.btnFBDel.Size = new System.Drawing.Size(179, 51);
            this.btnFBDel.TabIndex = 26;
            this.btnFBDel.UseVisualStyleBackColor = true;
            this.btnFBDel.Click += new System.EventHandler(this.btnFBDel_Click);
            // 
            // btnFBNew
            // 
            this.btnFBNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFBNew.BackgroundImage")));
            this.btnFBNew.Location = new System.Drawing.Point(214, -1);
            this.btnFBNew.Name = "btnFBNew";
            this.btnFBNew.Size = new System.Drawing.Size(179, 54);
            this.btnFBNew.TabIndex = 25;
            this.btnFBNew.UseVisualStyleBackColor = true;
            this.btnFBNew.Click += new System.EventHandler(this.btnFBNew_Click);
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.Location = new System.Drawing.Point(2, -1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(214, 468);
            this.panel3.TabIndex = 23;
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHome.BackgroundImage")));
            this.btnHome.Location = new System.Drawing.Point(26, 4);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(55, 23);
            this.btnHome.TabIndex = 1;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnsignout
            // 
            this.btnsignout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnsignout.BackgroundImage")));
            this.btnsignout.Location = new System.Drawing.Point(87, 4);
            this.btnsignout.Name = "btnsignout";
            this.btnsignout.Size = new System.Drawing.Size(59, 23);
            this.btnsignout.TabIndex = 2;
            this.btnsignout.Text = "SignOut";
            this.btnsignout.UseVisualStyleBackColor = true;
            this.btnsignout.Click += new System.EventHandler(this.btnsignout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 632);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1041, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(208, 17);
            this.toolStripStatusLabel1.Text = "© 2011 Aptara, Inc. All rights reserved.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(184, 32);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // pnlbtn
            // 
            this.pnlbtn.Controls.Add(this.btnHome);
            this.pnlbtn.Controls.Add(this.btnsignout);
            this.pnlbtn.Location = new System.Drawing.Point(865, 12);
            this.pnlbtn.Name = "pnlbtn";
            this.pnlbtn.Size = new System.Drawing.Size(164, 32);
            this.pnlbtn.TabIndex = 5;
            // 
            // GenerateXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 654);
            this.Controls.Add(this.pnlbtn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            
            this.Name = "GenerateXML";
            this.Text = "GenerateXML";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenerateXML_FormClosing);
            this.Load += new System.EventHandler(this.GenerateXML_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlbtn.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnsignout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TreeView TreeView1;
        private System.Windows.Forms.Button btnAWUpdate;
        private System.Windows.Forms.Button btnAWDel;
        private System.Windows.Forms.Button btnAWNew;
        private System.Windows.Forms.Button btnOPPUpdate;
        private System.Windows.Forms.Button btnOPPDel;
        private System.Windows.Forms.Button btnOPPNew;
        private System.Windows.Forms.Button btnFBUpdate;
        private System.Windows.Forms.Button btnFBDel;
        private System.Windows.Forms.Button btnFBNew;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtgenrationLimit;
        private System.Windows.Forms.Button btnFixLimit;
        private System.Windows.Forms.Label lblCurrentLimit;
        private System.Windows.Forms.Label lblCurrentLimits;
    }
}