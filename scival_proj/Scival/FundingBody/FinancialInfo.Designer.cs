namespace Scival.FundingBody
{
    partial class FinancialInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinancialInfo));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.dtGridTex = new System.Windows.Forms.DataGridView();
            this.Texids = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text_Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ddlIds = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtTexDetail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.datePick = new System.Windows.Forms.DateTimePicker();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.grdFiscal = new System.Windows.Forms.DataGridView();
            this.FiscalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridTex)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFiscal)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.dtGridTex);
            this.groupBox1.Controls.Add(this.ddlIds);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtTexDetail);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(15, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 246);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tex Ids";
            // 
            // btnUpdate
            // 
           // this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.Location = new System.Drawing.Point(233, 105);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 41;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            //this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.Location = new System.Drawing.Point(335, 106);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(62, 81);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 39;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // button2
            // 
            //this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.Location = new System.Drawing.Point(335, 77);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Add &URL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dtGridTex
            // 
            this.dtGridTex.AllowUserToAddRows = false;
            this.dtGridTex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtGridTex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridTex.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Texids,
            this.Text_Details});
            this.dtGridTex.Location = new System.Drawing.Point(65, 134);
            this.dtGridTex.Name = "dtGridTex";
            this.dtGridTex.ReadOnly = true;
            this.dtGridTex.RowHeadersVisible = false;
            this.dtGridTex.Size = new System.Drawing.Size(438, 93);
            this.dtGridTex.TabIndex = 7;
            this.dtGridTex.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridTex_CellDoubleClick);
            this.dtGridTex.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtGridTex_KeyUp);
            // 
            // Texids
            // 
            this.Texids.DataPropertyName = "TYPE";
            this.Texids.HeaderText = "Tex IDs";
            this.Texids.Name = "Texids";
            this.Texids.ReadOnly = true;
            this.Texids.Width = 220;
            // 
            // Text_Details
            // 
            this.Text_Details.DataPropertyName = "TAXIDS_TEXT";
            this.Text_Details.HeaderText = "Tex Detail";
            this.Text_Details.Name = "Text_Details";
            this.Text_Details.ReadOnly = true;
            this.Text_Details.Width = 210;
            // 
            // ddlIds
            // 
            this.ddlIds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlIds.FormattingEnabled = true;
            this.ddlIds.Location = new System.Drawing.Point(180, 19);
            this.ddlIds.Name = "ddlIds";
            this.ddlIds.Size = new System.Drawing.Size(269, 21);
            this.ddlIds.TabIndex = 6;
            this.ddlIds.MouseHover += new System.EventHandler(this.ddlIds_MouseHover);

            //pankaj 8 july 2019
            this.ddlIds.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlIds_MouseWheel);
            // 
            // button1
            // 
            //this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.Location = new System.Drawing.Point(233, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtTexDetail
            // 
            this.txtTexDetail.Location = new System.Drawing.Point(180, 46);
            this.txtTexDetail.Name = "txtTexDetail";
            this.txtTexDetail.Size = new System.Drawing.Size(269, 20);
            this.txtTexDetail.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tex Details *";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tex ID *";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.datePick);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.grdFiscal);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(15, 278);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(554, 206);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fiscal Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(73, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "(*) are manadatory fields.";
            // 
            // datePick
            // 
            this.datePick.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePick.Location = new System.Drawing.Point(180, 19);
            this.datePick.Name = "datePick";
            this.datePick.Size = new System.Drawing.Size(269, 20);
            this.datePick.TabIndex = 12;
            // 
            // button3
            // 
            //this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.Location = new System.Drawing.Point(319, 51);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Add &URL";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            //this.button4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button4.BackgroundImage")));
            this.button4.Location = new System.Drawing.Point(217, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "Add";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // grdFiscal
            // 
            this.grdFiscal.AllowUserToAddRows = false;
            this.grdFiscal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFiscal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FiscalDate});
            this.grdFiscal.Location = new System.Drawing.Point(65, 90);
            this.grdFiscal.Name = "grdFiscal";
            this.grdFiscal.ReadOnly = true;
            this.grdFiscal.RowHeadersVisible = false;
            this.grdFiscal.Size = new System.Drawing.Size(435, 89);
            this.grdFiscal.TabIndex = 11;
            this.grdFiscal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdFiscal_KeyUp);
            // 
            // FiscalDate
            // 
            this.FiscalDate.DataPropertyName = "FISCALCLOSEDATE_COLUMN";
            this.FiscalDate.HeaderText = "Fiscal Date";
            this.FiscalDate.Name = "FiscalDate";
            this.FiscalDate.ReadOnly = true;
            this.FiscalDate.Width = 430;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Fiscal Date *";
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(15, 491);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(554, 157);
            this.pnlURL.TabIndex = 2;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(185, 262);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 52;
            this.lblMsg.Visible = false;
            // 
            // FinancialInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FinancialInfo";
            this.Size = new System.Drawing.Size(608, 669);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridTex)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFiscal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtTexDetail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlIds;
        private System.Windows.Forms.DataGridView dtGridTex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Texids;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text_Details;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView grdFiscal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn FiscalDate;
        private System.Windows.Forms.DateTimePicker datePick;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.Label lblMsg;
    }
}
