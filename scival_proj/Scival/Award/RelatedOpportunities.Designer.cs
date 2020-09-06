namespace Scival.Award
{
    partial class RelatedOpportunities
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RelatedOpportunities));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnaddurl = new System.Windows.Forms.Button();
            this.grdOpportunity = new System.Windows.Forms.DataGridView();
            this.btnRel = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.ddlreltype = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grdRelOpportunity = new System.Windows.Forms.DataGridView();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.Fname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relation_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPPORTUNITYNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOpportunity)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRelOpportunity)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblMsg);
            this.groupBox2.Controls.Add(this.btnReset);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnaddurl);
            this.groupBox2.Controls.Add(this.grdOpportunity);
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
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(267, 48);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 47;
            this.lblMsg.Visible = false;
            // 
            // btnReset
            // 
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.Location = new System.Drawing.Point(496, 68);
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
            this.btnSearch.Location = new System.Drawing.Point(422, 68);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(68, 23);
            this.btnSearch.TabIndex = 45;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(224, 69);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(192, 20);
            this.txtSearch.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Search";
            // 
            // btnaddurl
            // 
            this.btnaddurl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnaddurl.BackgroundImage")));
            this.btnaddurl.Location = new System.Drawing.Point(496, 16);
            this.btnaddurl.Name = "btnaddurl";
            this.btnaddurl.Size = new System.Drawing.Size(68, 23);
            this.btnaddurl.TabIndex = 42;
            this.btnaddurl.Text = "Add &URL";
            this.btnaddurl.UseVisualStyleBackColor = true;
            this.btnaddurl.Click += new System.EventHandler(this.btnaddurl_Click);
            // 
            // grdOpportunity
            // 
            this.grdOpportunity.AllowUserToAddRows = false;
            this.grdOpportunity.AllowUserToDeleteRows = false;
            this.grdOpportunity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOpportunity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Fname});
            this.grdOpportunity.Location = new System.Drawing.Point(52, 111);
            this.grdOpportunity.Name = "grdOpportunity";
            this.grdOpportunity.ReadOnly = true;
            this.grdOpportunity.RowHeadersVisible = false;
            this.grdOpportunity.Size = new System.Drawing.Size(512, 245);
            this.grdOpportunity.TabIndex = 41;
            // 
            // btnRel
            // 
            this.btnRel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRel.BackgroundImage")));
            this.btnRel.Location = new System.Drawing.Point(422, 16);
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
            this.label14.Location = new System.Drawing.Point(107, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // ddlreltype
            // 
            this.ddlreltype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlreltype.FormattingEnabled = true;
            this.ddlreltype.Location = new System.Drawing.Point(224, 18);
            this.ddlreltype.Name = "ddlreltype";
            this.ddlreltype.Size = new System.Drawing.Size(192, 21);
            this.ddlreltype.TabIndex = 4;

            this.ddlreltype.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlreltype_MouseWheel);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rel Type *";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.grdRelOpportunity);
            this.groupBox3.Location = new System.Drawing.Point(17, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(575, 163);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Related Opportunities";
            // 
            // grdRelOpportunity
            // 
            this.grdRelOpportunity.AllowUserToAddRows = false;
            this.grdRelOpportunity.AllowUserToDeleteRows = false;
            this.grdRelOpportunity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRelOpportunity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.relation_name,
            this.OPPORTUNITYNAME});
            this.grdRelOpportunity.Location = new System.Drawing.Point(42, 19);
            this.grdRelOpportunity.Name = "grdRelOpportunity";
            this.grdRelOpportunity.ReadOnly = true;
            this.grdRelOpportunity.RowHeadersVisible = false;
            this.grdRelOpportunity.Size = new System.Drawing.Size(514, 120);
            this.grdRelOpportunity.TabIndex = 0;
            this.grdRelOpportunity.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdRelOpp_KeyUp);
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(17, 600);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(575, 157);
            this.pnlURL.TabIndex = 3;
            // 
            // Fname
            // 
            this.Fname.DataPropertyName = "OPPORTUNITYNAME";
            this.Fname.HeaderText = "OPPORTUNITYNAME";
            this.Fname.Name = "Fname";
            this.Fname.ReadOnly = true;
            this.Fname.Width = 490;
            // 
            // relation_name
            // 
            this.relation_name.DataPropertyName = "relation_name";
            this.relation_name.HeaderText = "REL TYPE";
            this.relation_name.Name = "relation_name";
            this.relation_name.ReadOnly = true;
            this.relation_name.Width = 150;
            // 
            // OPPORTUNITYNAME
            // 
            this.OPPORTUNITYNAME.DataPropertyName = "OPPORTUNITYNAME";
            this.OPPORTUNITYNAME.HeaderText = "OPPORTUNITY NAME";
            this.OPPORTUNITYNAME.Name = "OPPORTUNITYNAME";
            this.OPPORTUNITYNAME.ReadOnly = true;
            this.OPPORTUNITYNAME.Width = 350;
            // 
            // RelatedOpportunities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "RelatedOpportunities";
            this.Size = new System.Drawing.Size(632, 773);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOpportunity)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRelOpportunity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ddlreltype;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView grdOpportunity;
        private System.Windows.Forms.Button btnRel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView grdRelOpportunity;
        private System.Windows.Forms.Button btnaddurl;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fname;
        private System.Windows.Forms.DataGridViewTextBoxColumn relation_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPPORTUNITYNAME;
    }
}
