namespace Scival.FundingBody
{
    partial class Classification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Classification));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dgvSubLevel = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sub_level_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewLinkColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ddlASJC = new System.Windows.Forms.ListBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnupdate = new System.Windows.Forms.Button();
            this.btnAddurl = new System.Windows.Forms.Button();
            this.grdClass = new System.Windows.Forms.DataGridView();
            this.TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FREQUENCY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CLASSIFICATION_TEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.ddlType = new System.Windows.Forms.ComboBox();
            this.txtFreqncy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlASJC1 = new System.Windows.Forms.ComboBox();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdClass)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.lblSearch);
            this.groupBox1.Controls.Add(this.dgvSubLevel);
            this.groupBox1.Controls.Add(this.ddlASJC);
            this.groupBox1.Controls.Add(this.lblMsg);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.btncancel);
            this.groupBox1.Controls.Add(this.btnupdate);
            this.groupBox1.Controls.Add(this.btnAddurl);
            this.groupBox1.Controls.Add(this.grdClass);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.ddlType);
            this.groupBox1.Controls.Add(this.txtFreqncy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ddlASJC1);
            this.groupBox1.Location = new System.Drawing.Point(17, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(669, 722);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Classification";
            // 
            // txtSearch
            // 
            this.txtSearch.AcceptsReturn = true;
            this.txtSearch.Location = new System.Drawing.Point(104, 196);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 20);
            this.txtSearch.TabIndex = 53;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(15, 199);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 13);
            this.lblSearch.TabIndex = 52;
            this.lblSearch.Text = "Search:";
            // 
            // dgvSubLevel
            // 
            this.dgvSubLevel.AllowUserToAddRows = false;
            this.dgvSubLevel.AllowUserToDeleteRows = false;
            this.dgvSubLevel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubLevel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.description,
            this.sub_level_description,
            this.DataGridViewLinkColumn});
            this.dgvSubLevel.Location = new System.Drawing.Point(9, 226);
            this.dgvSubLevel.Name = "dgvSubLevel";
            this.dgvSubLevel.ReadOnly = true;
            this.dgvSubLevel.RowHeadersVisible = false;
            this.dgvSubLevel.Size = new System.Drawing.Size(641, 207);
            this.dgvSubLevel.TabIndex = 51;
            this.dgvSubLevel.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubLevel_CellContentClick);
            // 
            // Code
            // 
            this.Code.DataPropertyName = "Code";
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 50;
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            this.description.HeaderText = "CLASSIFICATION";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Width = 225;
            // 
            // sub_level_description
            // 
            this.sub_level_description.DataPropertyName = "sub_level_description";
            this.sub_level_description.HeaderText = "SUB LEVEL";
            this.sub_level_description.Name = "sub_level_description";
            this.sub_level_description.ReadOnly = true;
            this.sub_level_description.Width = 250;
            // 
            // DataGridViewLinkColumn
            // 
            this.DataGridViewLinkColumn.HeaderText = "Action";
            this.DataGridViewLinkColumn.Name = "DataGridViewLinkColumn";
            this.DataGridViewLinkColumn.ReadOnly = true;
            this.DataGridViewLinkColumn.Text = "Select";
            this.DataGridViewLinkColumn.UseColumnTextForLinkValue = true;
            // 
            // ddlASJC
            // 
            this.ddlASJC.FormattingEnabled = true;
            this.ddlASJC.Location = new System.Drawing.Point(173, 87);
            this.ddlASJC.Name = "ddlASJC";
            this.ddlASJC.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ddlASJC.Size = new System.Drawing.Size(260, 95);
            this.ddlASJC.TabIndex = 50;
            this.ddlASJC.SelectedIndexChanged += new System.EventHandler(this.ddlASJC_SelectedIndexChanged);
            this.ddlASJC.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlASJC_MouseWheel);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(185, 478);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 49;
            this.lblMsg.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(54, 450);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // btncancel
            // 
            //this.btncancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncancel.BackgroundImage")));
            this.btncancel.Location = new System.Drawing.Point(421, 445);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 9;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Visible = false;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnupdate
            // 
            //this.btnupdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnupdate.BackgroundImage")));
            this.btnupdate.Location = new System.Drawing.Point(340, 445);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(75, 23);
            this.btnupdate.TabIndex = 8;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Visible = false;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // btnAddurl
            // 
            //this.btnAddurl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddurl.BackgroundImage")));
            this.btnAddurl.Location = new System.Drawing.Point(261, 445);
            this.btnAddurl.Name = "btnAddurl";
            this.btnAddurl.Size = new System.Drawing.Size(75, 23);
            this.btnAddurl.TabIndex = 7;
            this.btnAddurl.Text = "Add &URL";
            this.btnAddurl.UseVisualStyleBackColor = true;
            this.btnAddurl.Click += new System.EventHandler(this.btnAddurl_Click);
            // 
            // grdClass
            // 
            this.grdClass.AllowUserToAddRows = false;
            this.grdClass.AllowUserToDeleteRows = false;
            this.grdClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TYPE,
            this.FREQUENCY,
            this.CLASSIFICATION_TEXT});
            this.grdClass.Location = new System.Drawing.Point(21, 527);
            this.grdClass.Name = "grdClass";
            this.grdClass.ReadOnly = true;
            this.grdClass.RowHeadersVisible = false;
            this.grdClass.Size = new System.Drawing.Size(487, 183);
            this.grdClass.TabIndex = 0;
            this.grdClass.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdClass_CellDoubleClick);
            this.grdClass.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdClass_KeyUp);
            // 
            // TYPE
            // 
            this.TYPE.DataPropertyName = "TYPE";
            this.TYPE.HeaderText = "TYPE";
            this.TYPE.Name = "TYPE";
            this.TYPE.ReadOnly = true;
            this.TYPE.Width = 125;
            // 
            // FREQUENCY
            // 
            this.FREQUENCY.DataPropertyName = "FREQUENCY";
            this.FREQUENCY.HeaderText = "FREQUENCY";
            this.FREQUENCY.Name = "FREQUENCY";
            this.FREQUENCY.ReadOnly = true;
            this.FREQUENCY.Width = 125;
            // 
            // CLASSIFICATION_TEXT
            // 
            this.CLASSIFICATION_TEXT.DataPropertyName = "CLASSIFICATION_TEXT";
            this.CLASSIFICATION_TEXT.HeaderText = "CLASSIFICATION_TEXT";
            this.CLASSIFICATION_TEXT.Name = "CLASSIFICATION_TEXT";
            this.CLASSIFICATION_TEXT.ReadOnly = true;
            this.CLASSIFICATION_TEXT.Width = 225;
            // 
            // btnSave
            // 
            //this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Location = new System.Drawing.Point(180, 445);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ddlType
            // 
            this.ddlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlType.FormattingEnabled = true;
            this.ddlType.Location = new System.Drawing.Point(173, 24);
            this.ddlType.Name = "ddlType";
            this.ddlType.Size = new System.Drawing.Size(260, 21);
            this.ddlType.TabIndex = 1;
            this.ddlType.SelectedIndexChanged += new System.EventHandler(this.ddlType_SelectedIndexChanged);
            this.ddlType.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlType_MouseWheel);
            // 
            // txtFreqncy
            // 
            this.txtFreqncy.AcceptsReturn = true;
            this.txtFreqncy.Location = new System.Drawing.Point(173, 55);
            this.txtFreqncy.Name = "txtFreqncy";
            this.txtFreqncy.Size = new System.Drawing.Size(260, 20);
            this.txtFreqncy.TabIndex = 5;
            this.txtFreqncy.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type *";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Frequancy *";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Classification *";
            // 
            // ddlASJC1
            // 
            this.ddlASJC1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlASJC1.FormattingEnabled = true;
            this.ddlASJC1.Location = new System.Drawing.Point(9, 500);
            this.ddlASJC1.Name = "ddlASJC1";
            this.ddlASJC1.Size = new System.Drawing.Size(87, 21);
            this.ddlASJC1.TabIndex = 3;
            this.ddlASJC1.Visible = false;
            this.ddlASJC1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlASJC1_MouseWheel);
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(17, 747);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(668, 157);
            this.pnlURL.TabIndex = 1;
            // 
            // Classification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox1);
            this.Name = "Classification";
            this.Size = new System.Drawing.Size(701, 914);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdClass)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn FREQUENCY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CLASSIFICATION_TEXT;
        private System.Windows.Forms.Button btnAddurl;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtFreqncy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddlASJC1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.ListBox ddlASJC;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.DataGridView dgvSubLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn sub_level_description;
        private System.Windows.Forms.DataGridViewLinkColumn DataGridViewLinkColumn;
    }
}
