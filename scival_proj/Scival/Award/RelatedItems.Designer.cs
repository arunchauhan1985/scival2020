namespace Scival.Award
{
    partial class RelatedItems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RelatedItems));
            this.grpItem = new System.Windows.Forms.GroupBox();
            this.ddlLangContextName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLinkText = new System.Windows.Forms.TextBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.grdAbout = new System.Windows.Forms.DataGridView();
            this.RelType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Linkurl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Linktext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.txtLinkUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlRelType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grpItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAbout)).BeginInit();
            this.SuspendLayout();
            // 
            // grpItem
            // 
            this.grpItem.Controls.Add(this.ddlLangContextName);
            this.grpItem.Controls.Add(this.label1);
            this.grpItem.Controls.Add(this.txtLinkText);
            this.grpItem.Controls.Add(this.lblMsg);
            this.grpItem.Controls.Add(this.btnUpdate);
            this.grpItem.Controls.Add(this.btnCancel);
            this.grpItem.Controls.Add(this.button1);
            this.grpItem.Controls.Add(this.grdAbout);
            this.grpItem.Controls.Add(this.BtnAdd);
            this.grpItem.Controls.Add(this.label14);
            this.grpItem.Controls.Add(this.txtLinkUrl);
            this.grpItem.Controls.Add(this.label3);
            this.grpItem.Controls.Add(this.txtDescr);
            this.grpItem.Controls.Add(this.label2);
            this.grpItem.Controls.Add(this.ddlRelType);
            this.grpItem.Controls.Add(this.label5);
            this.grpItem.Location = new System.Drawing.Point(27, 29);
            this.grpItem.Name = "grpItem";
            this.grpItem.Size = new System.Drawing.Size(594, 442);
            this.grpItem.TabIndex = 4;
            this.grpItem.TabStop = false;
            this.grpItem.Enter += new System.EventHandler(this.grpItem_Enter);
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
            this.ddlLangContextName.Location = new System.Drawing.Point(465, 83);
            this.ddlLangContextName.Name = "ddlLangContextName";
            this.ddlLangContextName.Size = new System.Drawing.Size(112, 21);
            this.ddlLangContextName.TabIndex = 58;
            this.ddlLangContextName.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlLangContextName_MouseWheel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "Link Text *";
            // 
            // txtLinkText
            // 
            this.txtLinkText.Location = new System.Drawing.Point(170, 160);
            this.txtLinkText.Name = "txtLinkText";
            this.txtLinkText.Size = new System.Drawing.Size(287, 20);
            this.txtLinkText.TabIndex = 56;
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
            // btnUpdate
            // 
            this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.Location = new System.Drawing.Point(271, 199);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 25);
            this.btnUpdate.TabIndex = 45;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.Location = new System.Drawing.Point(369, 199);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 44;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.Location = new System.Drawing.Point(466, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 19);
            this.button1.TabIndex = 43;
            this.button1.Text = "&+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grdAbout
            // 
            this.grdAbout.AllowUserToAddRows = false;
            this.grdAbout.AllowUserToDeleteRows = false;
            this.grdAbout.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAbout.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RelType,
            this.Description,
            this.Linkurl,
            this.Linktext,
            this.lang});
            this.grdAbout.Location = new System.Drawing.Point(6, 253);
            this.grdAbout.Name = "grdAbout";
            this.grdAbout.ReadOnly = true;
            this.grdAbout.RowHeadersVisible = false;
            this.grdAbout.Size = new System.Drawing.Size(524, 134);
            this.grdAbout.TabIndex = 42;
            this.grdAbout.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAbout_CellDoubleClick);
            this.grdAbout.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdAbout_KeyUp);
            // 
            // RelType
            // 
            this.RelType.DataPropertyName = "RELTYPE";
            this.RelType.HeaderText = "Rel Type";
            this.RelType.Name = "RelType";
            this.RelType.ReadOnly = true;
            this.RelType.Width = 120;
            // 
            // Description
            // 
            this.Description.DataPropertyName = "DESCRIPTION";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 150;
            // 
            // Linkurl
            // 
            this.Linkurl.DataPropertyName = "URL";
            this.Linkurl.HeaderText = "Link URL";
            this.Linkurl.Name = "Linkurl";
            this.Linkurl.ReadOnly = true;
            this.Linkurl.Width = 150;
            // 
            // Linktext
            // 
            this.Linktext.DataPropertyName = "LINK_TEXT";
            this.Linktext.HeaderText = "Link Text";
            this.Linktext.Name = "Linktext";
            this.Linktext.ReadOnly = true;
            this.Linktext.Width = 120;
            // 
            // lang
            // 
            this.lang.DataPropertyName = "lang";
            this.lang.HeaderText = "language";
            this.lang.Name = "lang";
            this.lang.ReadOnly = true;
            // 
            // BtnAdd
            // 
            this.BtnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnAdd.BackgroundImage")));
            this.BtnAdd.Location = new System.Drawing.Point(170, 200);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 24);
            this.BtnAdd.TabIndex = 40;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(41, 206);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 39;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // txtLinkUrl
            // 
            this.txtLinkUrl.Location = new System.Drawing.Point(172, 134);
            this.txtLinkUrl.Name = "txtLinkUrl";
            this.txtLinkUrl.Size = new System.Drawing.Size(287, 20);
            this.txtLinkUrl.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Link URL *";
            // 
            // txtDescr
            // 
            this.txtDescr.Location = new System.Drawing.Point(172, 65);
            this.txtDescr.Multiline = true;
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.Size = new System.Drawing.Size(287, 60);
            this.txtDescr.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description";
            // 
            // ddlRelType
            // 
            this.ddlRelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRelType.FormattingEnabled = true;
            this.ddlRelType.Location = new System.Drawing.Point(177, 38);
            this.ddlRelType.Name = "ddlRelType";
            this.ddlRelType.Size = new System.Drawing.Size(287, 21);
            this.ddlRelType.TabIndex = 1;
            this.ddlRelType.SelectedIndexChanged += new System.EventHandler(this.ddlRelType_SelectedIndexChanged);
            this.ddlRelType.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlRelType_MouseWheel);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(93, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Rel Type";
            // 
            // RelatedItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpItem);
            this.Name = "RelatedItems";
            this.Size = new System.Drawing.Size(709, 585);
            this.grpItem.ResumeLayout(false);
            this.grpItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAbout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpItem;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView grdAbout;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtLinkUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlRelType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLinkText;
        private System.Windows.Forms.ComboBox ddlLangContextName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RelType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linkurl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linktext;
        private System.Windows.Forms.DataGridViewTextBoxColumn lang;
    }
}
