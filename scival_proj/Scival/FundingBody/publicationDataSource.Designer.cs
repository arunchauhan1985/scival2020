namespace Scival.FundingBody
{
    partial class publicationDataSource
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(publicationDataSource));
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.lbl_comment = new System.Windows.Forms.Label();
            this.txt_Comment = new System.Windows.Forms.TextBox();
            this.lbl_captureEnd = new System.Windows.Forms.Label();
            this.lbl_captureStart = new System.Windows.Forms.Label();
            this.lbl_Freq = new System.Windows.Forms.Label();
            this.ddl_Frequency = new System.Windows.Forms.ComboBox();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.ddlLangContextName = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.ddlStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.grdoppottunities = new System.Windows.Forms.DataGridView();
            this.OPP_SOURCE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.O_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FREQUENCY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CAPTURESTART = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CAPTUREEND = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COMMENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LASTVISITED = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewLinkColumn();
            this.label14 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.grpItem = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdoppottunities)).BeginInit();
            this.grpItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Location = new System.Drawing.Point(170, 229);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(156, 20);
            this.dateTimePicker3.TabIndex = 71;
            this.dateTimePicker3.Value = new System.DateTime(1800, 1, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(170, 190);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(156, 20);
            this.dateTimePicker2.TabIndex = 70;
            this.dateTimePicker2.Value = new System.DateTime(1800, 1, 1, 0, 0, 0, 0);
            // 
            // lbl_comment
            // 
            this.lbl_comment.AutoSize = true;
            this.lbl_comment.Location = new System.Drawing.Point(91, 285);
            this.lbl_comment.Name = "lbl_comment";
            this.lbl_comment.Size = new System.Drawing.Size(51, 13);
            this.lbl_comment.TabIndex = 69;
            this.lbl_comment.Text = "Comment";
            // 
            // txt_Comment
            // 
            this.txt_Comment.Location = new System.Drawing.Point(170, 271);
            this.txt_Comment.Multiline = true;
            this.txt_Comment.Name = "txt_Comment";
            this.txt_Comment.Size = new System.Drawing.Size(318, 49);
            this.txt_Comment.TabIndex = 9;
            // 
            // lbl_captureEnd
            // 
            this.lbl_captureEnd.AutoSize = true;
            this.lbl_captureEnd.Location = new System.Drawing.Point(91, 236);
            this.lbl_captureEnd.Name = "lbl_captureEnd";
            this.lbl_captureEnd.Size = new System.Drawing.Size(63, 13);
            this.lbl_captureEnd.TabIndex = 67;
            this.lbl_captureEnd.Text = "CaptureEnd";
            // 
            // lbl_captureStart
            // 
            this.lbl_captureStart.AutoSize = true;
            this.lbl_captureStart.Location = new System.Drawing.Point(91, 197);
            this.lbl_captureStart.Name = "lbl_captureStart";
            this.lbl_captureStart.Size = new System.Drawing.Size(66, 13);
            this.lbl_captureStart.TabIndex = 66;
            this.lbl_captureStart.Text = "CaptureStart";
            // 
            // lbl_Freq
            // 
            this.lbl_Freq.AutoSize = true;
            this.lbl_Freq.Location = new System.Drawing.Point(93, 150);
            this.lbl_Freq.Name = "lbl_Freq";
            this.lbl_Freq.Size = new System.Drawing.Size(57, 13);
            this.lbl_Freq.TabIndex = 63;
            this.lbl_Freq.Text = "Frequency";
            // 
            // ddl_Frequency
            // 
            this.ddl_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Frequency.FormattingEnabled = true;
            this.ddl_Frequency.Items.AddRange(new object[] {
            "SIGNAL-BASED",
            "DAILY",
            "WEEKLY",
            "BI-WEEKLY",
            "MONTHLY",
            "BI-ANNUALLY",
            "ANNUALLY"});
            this.ddl_Frequency.Location = new System.Drawing.Point(170, 147);
            this.ddl_Frequency.Name = "ddl_Frequency";
            this.ddl_Frequency.Size = new System.Drawing.Size(156, 21);
            this.ddl_Frequency.TabIndex = 6;
            // 
            // lbl_Name
            // 
            this.lbl_Name.AutoSize = true;
            this.lbl_Name.Location = new System.Drawing.Point(93, 45);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(42, 13);
            this.lbl_Name.TabIndex = 61;
            this.lbl_Name.Text = "NAME*";
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(170, 42);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(318, 20);
            this.txt_name.TabIndex = 1;
            // 
            // ddlLangContextName
            // 
            this.ddlLangContextName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLangContextName.FormattingEnabled = true;
            this.ddlLangContextName.Items.AddRange(new object[] {
            "Afrikaans",
            "Albanian",
            "Arabic",
            "Armenian",
            "Azerbaijani",
            "Indonesian",
            "Basque",
            "Bengali",
            "Bosnian",
            "Bulgarian",
            "Burmese",
            "Belarusian",
            "Catalan",
            "Chinese",
            "Croatian",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Esperanto",
            "Estonian",
            "Finnish",
            "French",
            "Irish Gaelic",
            "Gallegan",
            "Georgian",
            "German",
            "Greek",
            "Hebrew",
            "Hindi",
            "Hungarian",
            "Icelandic",
            "Italian",
            "Japanese",
            "Korean",
            "Latin",
            "Latvian",
            "Lithuanian",
            "Macedonian",
            "Malay",
            "Maori",
            "Mongolian",
            "Norwegian",
            "Persian",
            "Polish",
            "Polyglot",
            "Portuguese",
            "Pushto",
            "Romanian",
            "Russian",
            "Scottish Gaelic",
            "Serbian",
            "Sinhalese",
            "Slovak",
            "Slovene",
            "Spanish",
            "Swedish",
            "Tagalog",
            "Thai",
            "Turkish",
            "Ukrainian",
            "Urdu",
            "Uzbek",
            "Vietnamese"});
            this.ddlLangContextName.Location = new System.Drawing.Point(332, 107);
            this.ddlLangContextName.Name = "ddlLangContextName";
            this.ddlLangContextName.Size = new System.Drawing.Size(112, 21);
            this.ddlLangContextName.TabIndex = 5;
            this.ddlLangContextName.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(540, 108);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(156, 20);
            this.dateTimePicker1.TabIndex = 59;
            this.dateTimePicker1.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(471, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 58;
            this.label6.Text = "Last Visted";
            this.label6.Visible = false;
            // 
            // ddlStatus
            // 
            this.ddlStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlStatus.FormattingEnabled = true;
            this.ddlStatus.Items.AddRange(new object[] {
            "active",
            "inactive",
            "discontinued"});
            this.ddlStatus.Location = new System.Drawing.Point(170, 107);
            this.ddlStatus.Name = "ddlStatus";
            this.ddlStatus.Size = new System.Drawing.Size(156, 21);
            this.ddlStatus.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Status";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(174, 237);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 55;
            this.lblMsg.Visible = false;
            // 
            // grdoppottunities
            // 
            this.grdoppottunities.AllowUserToAddRows = false;
            this.grdoppottunities.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdoppottunities.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdoppottunities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdoppottunities.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OPP_SOURCE_ID,
            this.URL,
            this.O_Name,
            this.FREQUENCY,
            this.CAPTURESTART,
            this.CAPTUREEND,
            this.COMMENT,
            this.STATUS,
            this.LANG,
            this.LASTVISITED,
            this.Action});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdoppottunities.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdoppottunities.Location = new System.Drawing.Point(30, 373);
            this.grdoppottunities.Name = "grdoppottunities";
            this.grdoppottunities.ReadOnly = true;
            this.grdoppottunities.RowHeadersVisible = false;
            this.grdoppottunities.Size = new System.Drawing.Size(524, 134);
            this.grdoppottunities.TabIndex = 42;
            this.grdoppottunities.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdoppottunities_CellContentClick);
            this.grdoppottunities.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdoppottunities_CellDoubleClick);
            // 
            // OPP_SOURCE_ID
            // 
            this.OPP_SOURCE_ID.DataPropertyName = "PUB_SOURCE_ID";
            this.OPP_SOURCE_ID.HeaderText = "SOURCEID";
            this.OPP_SOURCE_ID.Name = "OPP_SOURCE_ID";
            this.OPP_SOURCE_ID.ReadOnly = true;
            this.OPP_SOURCE_ID.Width = 120;
            // 
            // URL
            // 
            this.URL.DataPropertyName = "URL";
            this.URL.HeaderText = "URL";
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            this.URL.Width = 120;
            // 
            // O_Name
            // 
            this.O_Name.DataPropertyName = "Name";
            this.O_Name.HeaderText = "Name";
            this.O_Name.Name = "O_Name";
            this.O_Name.ReadOnly = true;
            // 
            // FREQUENCY
            // 
            this.FREQUENCY.DataPropertyName = "FREQUENCY";
            this.FREQUENCY.HeaderText = "FREQUENCY";
            this.FREQUENCY.Name = "FREQUENCY";
            this.FREQUENCY.ReadOnly = true;
            // 
            // CAPTURESTART
            // 
            this.CAPTURESTART.DataPropertyName = "CAPTURESTART";
            this.CAPTURESTART.HeaderText = "CAPTURESTART";
            this.CAPTURESTART.Name = "CAPTURESTART";
            this.CAPTURESTART.ReadOnly = true;
            // 
            // CAPTUREEND
            // 
            this.CAPTUREEND.DataPropertyName = "CAPTUREEND";
            this.CAPTUREEND.HeaderText = "CAPTUREEND";
            this.CAPTUREEND.Name = "CAPTUREEND";
            this.CAPTUREEND.ReadOnly = true;
            // 
            // COMMENT
            // 
            this.COMMENT.DataPropertyName = "PUB_COMMENT";
            this.COMMENT.HeaderText = "COMMENT";
            this.COMMENT.Name = "COMMENT";
            this.COMMENT.ReadOnly = true;
            // 
            // STATUS
            // 
            this.STATUS.DataPropertyName = "STATUS";
            this.STATUS.HeaderText = "STATUS";
            this.STATUS.Name = "STATUS";
            this.STATUS.ReadOnly = true;
            this.STATUS.Width = 120;
            // 
            // LANG
            // 
            this.LANG.DataPropertyName = "LANG";
            this.LANG.HeaderText = "LANG";
            this.LANG.Name = "LANG";
            this.LANG.ReadOnly = true;
            this.LANG.Visible = false;
            // 
            // LASTVISITED
            // 
            this.LASTVISITED.DataPropertyName = "LASTVISITED";
            this.LASTVISITED.HeaderText = "LAST VISITED";
            this.LASTVISITED.Name = "LASTVISITED";
            this.LASTVISITED.ReadOnly = true;
            this.LASTVISITED.Visible = false;
            this.LASTVISITED.Width = 120;
            // 
            // Action
            // 
            this.Action.DataPropertyName = "lnkColumn";
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            this.Action.Text = "Delete";
            this.Action.UseColumnTextForLinkValue = true;
            this.Action.Width = 70;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 338);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 39;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(170, 68);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(318, 20);
            this.txtUrl.TabIndex = 2;
            // 
            // grpItem
            // 
            this.grpItem.Controls.Add(this.dateTimePicker3);
            this.grpItem.Controls.Add(this.dateTimePicker2);
            this.grpItem.Controls.Add(this.lbl_comment);
            this.grpItem.Controls.Add(this.txt_Comment);
            this.grpItem.Controls.Add(this.lbl_captureEnd);
            this.grpItem.Controls.Add(this.lbl_captureStart);
            this.grpItem.Controls.Add(this.lbl_Freq);
            this.grpItem.Controls.Add(this.ddl_Frequency);
            this.grpItem.Controls.Add(this.lbl_Name);
            this.grpItem.Controls.Add(this.txt_name);
            this.grpItem.Controls.Add(this.ddlLangContextName);
            this.grpItem.Controls.Add(this.dateTimePicker1);
            this.grpItem.Controls.Add(this.label6);
            this.grpItem.Controls.Add(this.ddlStatus);
            this.grpItem.Controls.Add(this.label2);
            this.grpItem.Controls.Add(this.lblMsg);
            this.grpItem.Controls.Add(this.btnUpdate);
            this.grpItem.Controls.Add(this.btnCancel);
            this.grpItem.Controls.Add(this.grdoppottunities);
            this.grpItem.Controls.Add(this.BtnAdd);
            this.grpItem.Controls.Add(this.label14);
            this.grpItem.Controls.Add(this.txtUrl);
            this.grpItem.Controls.Add(this.label1);
            this.grpItem.Location = new System.Drawing.Point(17, 16);
            this.grpItem.Name = "grpItem";
            this.grpItem.Size = new System.Drawing.Size(732, 526);
            this.grpItem.TabIndex = 3;
            this.grpItem.TabStop = false;
            this.grpItem.Text = "OPPORTUNITIES SOURCE";
            // 
            // btnUpdate
            // 
            //this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.Location = new System.Drawing.Point(282, 326);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 25);
            this.btnUpdate.TabIndex = 45;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click_1);
            // 
            // btnCancel
            // 
            //this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.Location = new System.Drawing.Point(380, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            // 
            // BtnAdd
            // 
            //this.BtnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnAdd.BackgroundImage")));
            this.BtnAdd.Location = new System.Drawing.Point(170, 327);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 24);
            this.BtnAdd.TabIndex = 10;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL*";
            // 
            // publicationDataSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpItem);
            this.Name = "publicationDataSource";
            this.Size = new System.Drawing.Size(768, 569);
            ((System.ComponentModel.ISupportInitialize)(this.grdoppottunities)).EndInit();
            this.grpItem.ResumeLayout(false);
            this.grpItem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label lbl_comment;
        private System.Windows.Forms.TextBox txt_Comment;
        private System.Windows.Forms.Label lbl_captureEnd;
        private System.Windows.Forms.Label lbl_captureStart;
        private System.Windows.Forms.Label lbl_Freq;
        private System.Windows.Forms.ComboBox ddl_Frequency;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.ComboBox ddlLangContextName;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ddlStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView grdoppottunities;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.GroupBox grpItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPP_SOURCE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn URL;
        private System.Windows.Forms.DataGridViewTextBoxColumn O_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn FREQUENCY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CAPTURESTART;
        private System.Windows.Forms.DataGridViewTextBoxColumn CAPTUREEND;
        private System.Windows.Forms.DataGridViewTextBoxColumn COMMENT;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn LANG;
        private System.Windows.Forms.DataGridViewTextBoxColumn LASTVISITED;
        private System.Windows.Forms.DataGridViewLinkColumn Action;
    }
}
