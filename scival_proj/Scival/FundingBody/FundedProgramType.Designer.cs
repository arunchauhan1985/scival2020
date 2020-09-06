namespace Scival.FundingBody
{
    partial class FundedProgramType
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FundedProgramType));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ddlFnProType = new System.Windows.Forms.ListBox();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnupdate = new System.Windows.Forms.Button();
            this.btnaddUrl = new System.Windows.Forms.Button();
            this.grdFDPRoType = new System.Windows.Forms.DataGridView();
            this.FUNDEDPROGRAMSTYPE_TEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnsave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFDPRoType)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMsg);
            this.groupBox1.Controls.Add(this.ddlFnProType);
            this.groupBox1.Controls.Add(this.btncancel);
            this.groupBox1.Controls.Add(this.btnupdate);
            this.groupBox1.Controls.Add(this.btnaddUrl);
            this.groupBox1.Controls.Add(this.grdFDPRoType);
            this.groupBox1.Controls.Add(this.btnsave);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(22, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 500);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = " MessageBox.Show(Convert.ToString(dsresult.Tables[\"ERRORCODE\"].Rows[0][1]), \"Sciv" +
                "al\", MessageBoxButtons.OK, MessageBoxIcon.Information);";
            this.groupBox1.Text = "Funding Program Type";
            // 
            // ddlFnProType
            // 
            this.ddlFnProType.FormattingEnabled = true;
            this.ddlFnProType.Location = new System.Drawing.Point(232, 19);
            this.ddlFnProType.Name = "ddlFnProType";
            this.ddlFnProType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ddlFnProType.Size = new System.Drawing.Size(249, 121);
            this.ddlFnProType.TabIndex = 6;
            // 
            // btncancel
            // 
            //this.btncancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncancel.BackgroundImage")));
            this.btncancel.Location = new System.Drawing.Point(360, 176);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 5;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Visible = false;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnupdate
            // 
            //this.btnupdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnupdate.BackgroundImage")));
            this.btnupdate.Location = new System.Drawing.Point(260, 176);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(75, 23);
            this.btnupdate.TabIndex = 4;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Visible = false;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // btnaddUrl
            // 
            //this.btnaddUrl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnaddUrl.BackgroundImage")));
            this.btnaddUrl.Location = new System.Drawing.Point(360, 147);
            this.btnaddUrl.Name = "btnaddUrl";
            this.btnaddUrl.Size = new System.Drawing.Size(75, 23);
            this.btnaddUrl.TabIndex = 3;
            this.btnaddUrl.Text = "Add &URL";
            this.btnaddUrl.UseVisualStyleBackColor = true;
            this.btnaddUrl.Click += new System.EventHandler(this.btnaddUrl_Click);
            // 
            // grdFDPRoType
            // 
            this.grdFDPRoType.AllowUserToAddRows = false;
            this.grdFDPRoType.AllowUserToDeleteRows = false;
            this.grdFDPRoType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFDPRoType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FUNDEDPROGRAMSTYPE_TEXT});
            this.grdFDPRoType.Location = new System.Drawing.Point(28, 234);
            this.grdFDPRoType.Name = "grdFDPRoType";
            this.grdFDPRoType.ReadOnly = true;
            this.grdFDPRoType.RowHeadersVisible = false;
            this.grdFDPRoType.Size = new System.Drawing.Size(486, 260);
            this.grdFDPRoType.TabIndex = 0;
            this.grdFDPRoType.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFDPRoType_CellDoubleClick);
            this.grdFDPRoType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdFDPRoType_KeyUp);
            // 
            // FUNDEDPROGRAMSTYPE_TEXT
            // 
            this.FUNDEDPROGRAMSTYPE_TEXT.DataPropertyName = "FUNDEDPROGRAMSTYPE_TEXT";
            this.FUNDEDPROGRAMSTYPE_TEXT.HeaderText = "FUNDED PROGRAMS TYPE";
            this.FUNDEDPROGRAMSTYPE_TEXT.Name = "FUNDEDPROGRAMSTYPE_TEXT";
            this.FUNDEDPROGRAMSTYPE_TEXT.ReadOnly = true;
            this.FUNDEDPROGRAMSTYPE_TEXT.Width = 475;
            // 
            // btnsave
            // 
            //this.btnsave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnsave.BackgroundImage")));
            this.btnsave.Location = new System.Drawing.Point(260, 147);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 2;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Funding Programs Types *";
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(22, 520);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(554, 157);
            this.pnlURL.TabIndex = 1;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(135, 210);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 53;
            this.lblMsg.Visible = false;
            // 
            // FundedProgramType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox1);
            this.Name = "FundedProgramType";
            this.Size = new System.Drawing.Size(614, 702);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFDPRoType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdFDPRoType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNDEDPROGRAMSTYPE_TEXT;
        private System.Windows.Forms.Button btnaddUrl;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.ListBox ddlFnProType;
        private System.Windows.Forms.Label lblMsg;
    }
}
