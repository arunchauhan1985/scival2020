namespace Scival.Award
{
    partial class RelaredOrg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RelaredOrg));
            this.ddlHerchy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ddlCuurency = new System.Windows.Forms.ComboBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnaddurl = new System.Windows.Forms.Button();
            this.grdFundingBody = new System.Windows.Forms.DataGridView();
            this.Fname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRel = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.ddlreltype = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grdRelOrg = new System.Windows.Forms.DataGridView();
            this.Hierarchy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RELTYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FUNDINGBODYNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyvalue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFundingBody)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRelOrg)).BeginInit();
            this.SuspendLayout();
            // 
            // ddlHerchy
            // 
            this.ddlHerchy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlHerchy.FormattingEnabled = true;
            this.ddlHerchy.Location = new System.Drawing.Point(224, 18);
            this.ddlHerchy.Name = "ddlHerchy";
            this.ddlHerchy.Size = new System.Drawing.Size(192, 21);
            this.ddlHerchy.TabIndex = 1;
            // 
            //pankaj july 2019
            this.ddlCuurency.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlCuurency_MouseWheel);
            this.ddlHerchy.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlHerchy_MouseWheel);
            this.ddlreltype.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlreltype_MouseWheel);
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Change Hierarchy *";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUpdate);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.ddlCuurency);
            this.groupBox2.Controls.Add(this.txtAmount);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnReset);
            this.groupBox2.Controls.Add(this.ddlHerchy);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnaddurl);
            this.groupBox2.Controls.Add(this.grdFundingBody);
            this.groupBox2.Controls.Add(this.btnRel);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.ddlreltype);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(17, 181);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(575, 403);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add Related Orgs";
            // 
            // ddlCuurency
            // 
            this.ddlCuurency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCuurency.FormattingEnabled = true;
            this.ddlCuurency.Location = new System.Drawing.Point(224, 73);
            this.ddlCuurency.Name = "ddlCuurency";
            this.ddlCuurency.Size = new System.Drawing.Size(192, 21);
            this.ddlCuurency.TabIndex = 50;
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(224, 106);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(192, 20);
            this.txtAmount.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(107, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "currency";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 47;
            this.label4.Text = "Amount";
            // 
            // btnReset
            // 
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.Location = new System.Drawing.Point(501, 167);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(68, 23);
            this.btnReset.TabIndex = 46;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.Location = new System.Drawing.Point(435, 167);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(68, 23);
            this.btnSearch.TabIndex = 45;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(224, 174);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(192, 20);
            this.txtSearch.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Search";
            // 
            // btnaddurl
            // 
            this.btnaddurl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnaddurl.BackgroundImage")));
            this.btnaddurl.Location = new System.Drawing.Point(501, 104);
            this.btnaddurl.Name = "btnaddurl";
            this.btnaddurl.Size = new System.Drawing.Size(68, 23);
            this.btnaddurl.TabIndex = 42;
            this.btnaddurl.Text = "Add &URL";
            this.btnaddurl.UseVisualStyleBackColor = true;
            this.btnaddurl.Click += new System.EventHandler(this.btnaddurl_Click);
            // 
            // grdFundingBody
            // 
            this.grdFundingBody.AllowUserToAddRows = false;
            this.grdFundingBody.AllowUserToDeleteRows = false;
            this.grdFundingBody.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFundingBody.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Fname});
            this.grdFundingBody.Location = new System.Drawing.Point(52, 230);
            this.grdFundingBody.Name = "grdFundingBody";
            this.grdFundingBody.ReadOnly = true;
            this.grdFundingBody.RowHeadersVisible = false;
            this.grdFundingBody.Size = new System.Drawing.Size(512, 153);
            this.grdFundingBody.TabIndex = 41;
            // 
            // Fname
            // 
            this.Fname.DataPropertyName = "FUNDINGBODYNAME";
            this.Fname.HeaderText = "FUNDINGBODYNAME";
            this.Fname.Name = "Fname";
            this.Fname.ReadOnly = true;
            this.Fname.Width = 490;
            // 
            // btnRel
            // 
            this.btnRel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRel.BackgroundImage")));
            this.btnRel.Location = new System.Drawing.Point(435, 103);
            this.btnRel.Name = "btnRel";
            this.btnRel.Size = new System.Drawing.Size(68, 23);
            this.btnRel.TabIndex = 3;
            this.btnRel.Text = "Submit";
            this.btnRel.UseVisualStyleBackColor = true;
            this.btnRel.Click += new System.EventHandler(this.btnRel_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(80, 154);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // ddlreltype
            // 
            this.ddlreltype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlreltype.FormattingEnabled = true;
            this.ddlreltype.Location = new System.Drawing.Point(224, 45);
            this.ddlreltype.Name = "ddlreltype";
            this.ddlreltype.Size = new System.Drawing.Size(192, 21);
            this.ddlreltype.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rel Type *";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.grdRelOrg);
            this.groupBox3.Location = new System.Drawing.Point(17, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(575, 163);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Related Orgs";
            // 
            // grdRelOrg
            // 
            this.grdRelOrg.AllowUserToAddRows = false;
            this.grdRelOrg.AllowUserToDeleteRows = false;
            this.grdRelOrg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRelOrg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Hierarchy,
            this.RELTYPE,
            this.FUNDINGBODYNAME,
            this.Amount,
            this.currencyvalue});
            this.grdRelOrg.Location = new System.Drawing.Point(42, 19);
            this.grdRelOrg.Name = "grdRelOrg";
            this.grdRelOrg.ReadOnly = true;
            this.grdRelOrg.RowHeadersVisible = false;
            this.grdRelOrg.Size = new System.Drawing.Size(514, 120);
            this.grdRelOrg.TabIndex = 0;
            this.grdRelOrg.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRelOrg_CellDoubleClick);
            this.grdRelOrg.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdRelOrg_KeyUp);
            // 
            // Hierarchy
            // 
            this.Hierarchy.DataPropertyName = "Hierarchy";
            this.Hierarchy.HeaderText = "Hierarchy";
            this.Hierarchy.Name = "Hierarchy";
            this.Hierarchy.ReadOnly = true;
            // 
            // RELTYPE
            // 
            this.RELTYPE.DataPropertyName = "RELTYPE";
            this.RELTYPE.HeaderText = "REL TYPE";
            this.RELTYPE.Name = "RELTYPE";
            this.RELTYPE.ReadOnly = true;
            this.RELTYPE.Width = 150;
            // 
            // FUNDINGBODYNAME
            // 
            this.FUNDINGBODYNAME.DataPropertyName = "FUNDINGBODYNAME";
            this.FUNDINGBODYNAME.HeaderText = "FUNDINGBODY NAME";
            this.FUNDINGBODYNAME.Name = "FUNDINGBODYNAME";
            this.FUNDINGBODYNAME.ReadOnly = true;
            this.FUNDINGBODYNAME.Width = 250;
            // 
            // Amount
            // 
            this.Amount.DataPropertyName = "Amount";
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // currencyvalue
            // 
            this.currencyvalue.DataPropertyName = "Currency";
            this.currencyvalue.HeaderText = "Currency";
            this.currencyvalue.Name = "currencyvalue";
            this.currencyvalue.ReadOnly = true;
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(17, 600);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(575, 157);
            this.pnlURL.TabIndex = 3;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(262, 254);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 32;
            this.lblMsg.Visible = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.Location = new System.Drawing.Point(435, 61);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(68, 25);
            this.btnUpdate.TabIndex = 47;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.Location = new System.Drawing.Point(501, 61);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 25);
            this.btnCancel.TabIndex = 46;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // RelaredOrg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "RelaredOrg";
            this.Size = new System.Drawing.Size(632, 773);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFundingBody)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRelOrg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlHerchy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ddlreltype;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView grdFundingBody;
        private System.Windows.Forms.Button btnRel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView grdRelOrg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fname;
        private System.Windows.Forms.Button btnaddurl;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddlCuurency;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hierarchy;
        private System.Windows.Forms.DataGridViewTextBoxColumn RELTYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNDINGBODYNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyvalue;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
    }
}
