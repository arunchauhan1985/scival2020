namespace Scival.FundingBody
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnHierachy = new System.Windows.Forms.Button();
            this.ddlHerchy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnaddurl = new System.Windows.Forms.Button();
            this.grdFundingBody = new System.Windows.Forms.DataGridView();
            this.Fname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FUNDINGBODYID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRel = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.ddlreltype = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grdRelOrg = new System.Windows.Forms.DataGridView();
            this.RELTYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FUNDINGBODYNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FundingBody_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblVMsg = new System.Windows.Forms.Label();
            this.btnVReset = new System.Windows.Forms.Button();
            this.btnVSearch = new System.Windows.Forms.Button();
            this.txtVSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnVAddurl = new System.Windows.Forms.Button();
            this.btnVRel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ddlVreltype = new System.Windows.Forms.ComboBox();
            this.lblVRelType = new System.Windows.Forms.Label();
            this.grdVendorFundingBody = new System.Windows.Forms.DataGridView();
            this.vendorid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFundingBody)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRelOrg)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVendorFundingBody)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnHierachy);
            this.groupBox1.Controls.Add(this.ddlHerchy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(17, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(575, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hierarchy";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnHierachy
            // 
            this.btnHierachy.Location = new System.Drawing.Point(436, 17);
            this.btnHierachy.Name = "btnHierachy";
            this.btnHierachy.Size = new System.Drawing.Size(68, 23);
            this.btnHierachy.TabIndex = 2;
            this.btnHierachy.Text = "Submit";
            this.btnHierachy.UseVisualStyleBackColor = true;
            this.btnHierachy.Click += new System.EventHandler(this.btnHierachy_Click);
            // 
            // ddlHerchy
            // 
            this.ddlHerchy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlHerchy.FormattingEnabled = true;
            this.ddlHerchy.Location = new System.Drawing.Point(210, 19);
            this.ddlHerchy.Name = "ddlHerchy";
            this.ddlHerchy.Size = new System.Drawing.Size(192, 21);
            this.ddlHerchy.TabIndex = 1;
            this.ddlHerchy.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlHerchy_MouseWheel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Change Hierarchy";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblMsg);
            this.groupBox2.Controls.Add(this.btnReset);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnaddurl);
            this.groupBox2.Controls.Add(this.grdFundingBody);
            this.groupBox2.Controls.Add(this.btnRel);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.ddlreltype);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(17, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(575, 346);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add Related Orgs";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(235, 52);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 56;
            this.lblMsg.Visible = false;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(496, 75);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(68, 23);
            this.btnReset.TabIndex = 46;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(422, 75);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(68, 23);
            this.btnSearch.TabIndex = 45;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(224, 76);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(192, 20);
            this.txtSearch.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Search";
            // 
            // btnaddurl
            // 
            this.btnaddurl.Location = new System.Drawing.Point(496, 17);
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
            this.Fname,
            this.FUNDINGBODYID});
            this.grdFundingBody.Location = new System.Drawing.Point(42, 113);
            this.grdFundingBody.Name = "grdFundingBody";
            this.grdFundingBody.ReadOnly = true;
            this.grdFundingBody.RowHeadersVisible = false;
            this.grdFundingBody.Size = new System.Drawing.Size(512, 227);
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
            // FUNDINGBODYID
            // 
            this.FUNDINGBODYID.DataPropertyName = "ORGDBID";
            this.FUNDINGBODYID.HeaderText = "FUNDINGBODYID";
            this.FUNDINGBODYID.Name = "FUNDINGBODYID";
            this.FUNDINGBODYID.ReadOnly = true;
            // 
            // btnRel
            // 
            this.btnRel.Location = new System.Drawing.Point(422, 17);
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
            this.label14.Location = new System.Drawing.Point(102, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // ddlreltype
            // 
            this.ddlreltype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlreltype.FormattingEnabled = true;
            this.ddlreltype.Location = new System.Drawing.Point(224, 19);
            this.ddlreltype.Name = "ddlreltype";
            this.ddlreltype.Size = new System.Drawing.Size(192, 21);
            this.ddlreltype.TabIndex = 4;
            this.ddlreltype.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlreltype_MouseWheel);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rel Type *";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.grdRelOrg);
            this.groupBox3.Location = new System.Drawing.Point(17, 75);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(575, 127);
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
            this.RELTYPE,
            this.FUNDINGBODYNAME,
            this.FundingBody_ID});
            this.grdRelOrg.Location = new System.Drawing.Point(42, 19);
            this.grdRelOrg.Name = "grdRelOrg";
            this.grdRelOrg.ReadOnly = true;
            this.grdRelOrg.RowHeadersVisible = false;
            this.grdRelOrg.Size = new System.Drawing.Size(514, 96);
            this.grdRelOrg.TabIndex = 0;
            this.grdRelOrg.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdRelOrg_KeyUp);
            // 
            // RELTYPE
            // 
            this.RELTYPE.DataPropertyName = "RELTYPE";
            this.RELTYPE.HeaderText = "REL TYPE";
            this.RELTYPE.Name = "RELTYPE";
            this.RELTYPE.ReadOnly = true;
            // 
            // FUNDINGBODYNAME
            // 
            this.FUNDINGBODYNAME.DataPropertyName = "FUNDINGBODYNAME";
            this.FUNDINGBODYNAME.HeaderText = "FUNDINGBODY NAME";
            this.FUNDINGBODYNAME.Name = "FUNDINGBODYNAME";
            this.FUNDINGBODYNAME.ReadOnly = true;
            this.FUNDINGBODYNAME.Width = 315;
            // 
            // FundingBody_ID
            // 
            this.FundingBody_ID.DataPropertyName = "ORGDBID";
            this.FundingBody_ID.HeaderText = "FundingBody_ID";
            this.FundingBody_ID.Name = "FundingBody_ID";
            this.FundingBody_ID.ReadOnly = true;
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(17, 903);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(575, 157);
            this.pnlURL.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblVMsg);
            this.groupBox4.Controls.Add(this.btnVReset);
            this.groupBox4.Controls.Add(this.btnVSearch);
            this.groupBox4.Controls.Add(this.txtVSearch);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.btnVAddurl);
            this.groupBox4.Controls.Add(this.btnVRel);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.ddlVreltype);
            this.groupBox4.Controls.Add(this.lblVRelType);
            this.groupBox4.Controls.Add(this.grdVendorFundingBody);
            this.groupBox4.Location = new System.Drawing.Point(17, 560);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(575, 300);
            this.groupBox4.TabIndex = 57;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Add Related Vendor\'s Orgs";
            // 
            // lblVMsg
            // 
            this.lblVMsg.AutoSize = true;
            this.lblVMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVMsg.ForeColor = System.Drawing.Color.Red;
            this.lblVMsg.Location = new System.Drawing.Point(189, 51);
            this.lblVMsg.Name = "lblVMsg";
            this.lblVMsg.Size = new System.Drawing.Size(0, 13);
            this.lblVMsg.TabIndex = 66;
            this.lblVMsg.Visible = false;
            // 
            // btnVReset
            // 
            this.btnVReset.Location = new System.Drawing.Point(450, 74);
            this.btnVReset.Name = "btnVReset";
            this.btnVReset.Size = new System.Drawing.Size(68, 23);
            this.btnVReset.TabIndex = 65;
            this.btnVReset.Text = "Reset";
            this.btnVReset.UseVisualStyleBackColor = true;
            this.btnVReset.Click += new System.EventHandler(this.btnVReset_Click);
            // 
            // btnVSearch
            // 
            this.btnVSearch.Location = new System.Drawing.Point(376, 74);
            this.btnVSearch.Name = "btnVSearch";
            this.btnVSearch.Size = new System.Drawing.Size(68, 23);
            this.btnVSearch.TabIndex = 64;
            this.btnVSearch.Text = "Search";
            this.btnVSearch.UseVisualStyleBackColor = true;
            this.btnVSearch.Click += new System.EventHandler(this.btnVSearch_Click);
            // 
            // txtVSearch
            // 
            this.txtVSearch.Location = new System.Drawing.Point(178, 75);
            this.txtVSearch.Name = "txtVSearch";
            this.txtVSearch.Size = new System.Drawing.Size(192, 20);
            this.txtVSearch.TabIndex = 63;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "Search";
            // 
            // btnVAddurl
            // 
            this.btnVAddurl.Location = new System.Drawing.Point(450, 16);
            this.btnVAddurl.Name = "btnVAddurl";
            this.btnVAddurl.Size = new System.Drawing.Size(68, 23);
            this.btnVAddurl.TabIndex = 61;
            this.btnVAddurl.Text = "Add &URL";
            this.btnVAddurl.UseVisualStyleBackColor = true;
            this.btnVAddurl.Click += new System.EventHandler(this.btnVAddurl_Click);
            // 
            // btnVRel
            // 
            this.btnVRel.Location = new System.Drawing.Point(376, 16);
            this.btnVRel.Name = "btnVRel";
            this.btnVRel.Size = new System.Drawing.Size(68, 23);
            this.btnVRel.TabIndex = 57;
            this.btnVRel.Text = "Submit";
            this.btnVRel.UseVisualStyleBackColor = true;
            this.btnVRel.Click += new System.EventHandler(this.btnVRel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 13);
            this.label6.TabIndex = 60;
            this.label6.Text = "(*) are manadatory fields.";
            // 
            // ddlVreltype
            // 
            this.ddlVreltype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlVreltype.FormattingEnabled = true;
            this.ddlVreltype.Location = new System.Drawing.Point(178, 18);
            this.ddlVreltype.Name = "ddlVreltype";
            this.ddlVreltype.Size = new System.Drawing.Size(192, 21);
            this.ddlVreltype.TabIndex = 59;
            this.ddlVreltype.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlVreltype_MouseWheel);
            // 
            // lblVRelType
            // 
            this.lblVRelType.AutoSize = true;
            this.lblVRelType.Location = new System.Drawing.Point(61, 21);
            this.lblVRelType.Name = "lblVRelType";
            this.lblVRelType.Size = new System.Drawing.Size(57, 13);
            this.lblVRelType.TabIndex = 58;
            this.lblVRelType.Text = "Rel Type *";
            // 
            // grdVendorFundingBody
            // 
            this.grdVendorFundingBody.AllowUserToAddRows = false;
            this.grdVendorFundingBody.AllowUserToDeleteRows = false;
            this.grdVendorFundingBody.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdVendorFundingBody.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vendorid,
            this.dataGridViewTextBoxColumn1});
            this.grdVendorFundingBody.Location = new System.Drawing.Point(10, 110);
            this.grdVendorFundingBody.Name = "grdVendorFundingBody";
            this.grdVendorFundingBody.ReadOnly = true;
            this.grdVendorFundingBody.RowHeadersVisible = false;
            this.grdVendorFundingBody.Size = new System.Drawing.Size(553, 183);
            this.grdVendorFundingBody.TabIndex = 41;
            // 
            // vendorid
            // 
            this.vendorid.DataPropertyName = "vendor_id";
            this.vendorid.HeaderText = "Vendor ID";
            this.vendorid.Name = "vendorid";
            this.vendorid.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "vendor_fundingbody_name";
            this.dataGridViewTextBoxColumn1.HeaderText = "FUNDINGBODYNAME";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 450;
            // 
            // RelaredOrg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "RelaredOrg";
            this.Size = new System.Drawing.Size(632, 1093);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFundingBody)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRelOrg)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVendorFundingBody)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnHierachy;
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
        private System.Windows.Forms.Button btnaddurl;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView grdVendorFundingBody;
        private System.Windows.Forms.Label lblVMsg;
        private System.Windows.Forms.Button btnVReset;
        private System.Windows.Forms.Button btnVSearch;
        private System.Windows.Forms.TextBox txtVSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnVAddurl;
        private System.Windows.Forms.Button btnVRel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ddlVreltype;
        private System.Windows.Forms.Label lblVRelType;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fname;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNDINGBODYID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RELTYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNDINGBODYNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn FundingBody_ID;
    }
}
