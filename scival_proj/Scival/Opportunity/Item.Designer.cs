namespace Scival.Opportunity
{
    partial class Item
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Item));
            this.grpboxAdd = new System.Windows.Forms.GroupBox();
            this.txtdurationExpression = new System.Windows.Forms.TextBox();
            this.lbldurationExpression = new System.Windows.Forms.Label();
            this.ddlRelatedItemType = new System.Windows.Forms.ComboBox();
            this.lblReltype = new System.Windows.Forms.Label();
            this.ddlLangOppName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAddURL = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.txtLinkText = new System.Windows.Forms.TextBox();
            this.txtLinkUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpboxGrid = new System.Windows.Forms.GroupBox();
            this.grdAbout = new System.Windows.Forms.DataGridView();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LINK_TEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DESCRIPTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.grpboxAdd.SuspendLayout();
            this.grpboxGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAbout)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxAdd
            // 
            this.grpboxAdd.Controls.Add(this.txtdurationExpression);
            this.grpboxAdd.Controls.Add(this.lbldurationExpression);
            this.grpboxAdd.Controls.Add(this.ddlRelatedItemType);
            this.grpboxAdd.Controls.Add(this.lblReltype);
            this.grpboxAdd.Controls.Add(this.ddlLangOppName);
            this.grpboxAdd.Controls.Add(this.label1);
            this.grpboxAdd.Controls.Add(this.txtDescr);
            this.grpboxAdd.Controls.Add(this.button1);
            this.grpboxAdd.Controls.Add(this.label14);
            this.grpboxAdd.Controls.Add(this.btnCancel);
            this.grpboxAdd.Controls.Add(this.btnUpdate);
            this.grpboxAdd.Controls.Add(this.btnAddURL);
            this.grpboxAdd.Controls.Add(this.BtnAdd);
            this.grpboxAdd.Controls.Add(this.txtLinkText);
            this.grpboxAdd.Controls.Add(this.txtLinkUrl);
            this.grpboxAdd.Controls.Add(this.label4);
            this.grpboxAdd.Controls.Add(this.label3);
            this.grpboxAdd.Location = new System.Drawing.Point(21, 42);
            this.grpboxAdd.Name = "grpboxAdd";
            this.grpboxAdd.Size = new System.Drawing.Size(628, 258);
            this.grpboxAdd.TabIndex = 0;
            this.grpboxAdd.TabStop = false;
            // 
            // txtdurationExpression
            // 
            this.txtdurationExpression.Location = new System.Drawing.Point(219, 188);
            this.txtdurationExpression.Name = "txtdurationExpression";
            this.txtdurationExpression.Size = new System.Drawing.Size(267, 20);
            this.txtdurationExpression.TabIndex = 76;
            this.txtdurationExpression.Visible = false;
            // 
            // lbldurationExpression
            // 
            this.lbldurationExpression.AutoSize = true;
            this.lbldurationExpression.Location = new System.Drawing.Point(109, 195);
            this.lbldurationExpression.Name = "lbldurationExpression";
            this.lbldurationExpression.Size = new System.Drawing.Size(105, 13);
            this.lbldurationExpression.TabIndex = 75;
            this.lbldurationExpression.Text = "DurationExpression *";
            this.lbldurationExpression.Visible = false;
            // 
            // ddlRelatedItemType
            // 
            this.ddlRelatedItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRelatedItemType.FormattingEnabled = true;
            this.ddlRelatedItemType.Location = new System.Drawing.Point(219, 25);
            this.ddlRelatedItemType.Name = "ddlRelatedItemType";
            this.ddlRelatedItemType.Size = new System.Drawing.Size(267, 21);
            this.ddlRelatedItemType.TabIndex = 74;
            this.ddlRelatedItemType.Visible = false;
            this.ddlRelatedItemType.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlRelatedItemType_MouseWheel);
            // 
            // lblReltype
            // 
            this.lblReltype.AutoSize = true;
            this.lblReltype.Location = new System.Drawing.Point(106, 28);
            this.lblReltype.Name = "lblReltype";
            this.lblReltype.Size = new System.Drawing.Size(94, 13);
            this.lblReltype.TabIndex = 73;
            this.lblReltype.Text = "Related Item Type";
            this.lblReltype.Visible = false;
            // 
            // ddlLangOppName
            // 
            this.ddlLangOppName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLangOppName.FormattingEnabled = true;
            this.ddlLangOppName.Items.AddRange(new object[] {
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
            this.ddlLangOppName.Location = new System.Drawing.Point(492, 100);
            this.ddlLangOppName.Name = "ddlLangOppName";
            this.ddlLangOppName.Size = new System.Drawing.Size(88, 21);
            this.ddlLangOppName.TabIndex = 72;
            this.ddlLangOppName.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlLangOppName_MouseWheel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "Description";
            // 
            // txtDescr
            // 
            this.txtDescr.Location = new System.Drawing.Point(219, 119);
            this.txtDescr.Multiline = true;
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.Size = new System.Drawing.Size(267, 62);
            this.txtDescr.TabIndex = 45;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.Location = new System.Drawing.Point(492, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 20);
            this.button1.TabIndex = 44;
            this.button1.Text = "&+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(90, 171);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.Location = new System.Drawing.Point(471, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.Location = new System.Drawing.Point(381, 214);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAddURL
            // 
            this.btnAddURL.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddURL.BackgroundImage")));
            this.btnAddURL.Location = new System.Drawing.Point(300, 214);
            this.btnAddURL.Name = "btnAddURL";
            this.btnAddURL.Size = new System.Drawing.Size(75, 23);
            this.btnAddURL.TabIndex = 9;
            this.btnAddURL.Text = "Add &URL";
            this.btnAddURL.UseVisualStyleBackColor = true;
            this.btnAddURL.Click += new System.EventHandler(this.btnaddurl_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnAdd.BackgroundImage")));
            this.BtnAdd.Location = new System.Drawing.Point(219, 214);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 8;
            this.BtnAdd.Text = "Save";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtLinkText
            // 
            this.txtLinkText.Location = new System.Drawing.Point(219, 84);
            this.txtLinkText.Name = "txtLinkText";
            this.txtLinkText.Size = new System.Drawing.Size(267, 20);
            this.txtLinkText.TabIndex = 7;
            // 
            // txtLinkUrl
            // 
            this.txtLinkUrl.Location = new System.Drawing.Point(219, 55);
            this.txtLinkUrl.Name = "txtLinkUrl";
            this.txtLinkUrl.Size = new System.Drawing.Size(267, 20);
            this.txtLinkUrl.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(109, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Link Text *";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(108, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Link URL *";
            // 
            // grpboxGrid
            // 
            this.grpboxGrid.Controls.Add(this.grdAbout);
            this.grpboxGrid.Location = new System.Drawing.Point(21, 306);
            this.grpboxGrid.Name = "grpboxGrid";
            this.grpboxGrid.Size = new System.Drawing.Size(604, 216);
            this.grpboxGrid.TabIndex = 1;
            this.grpboxGrid.TabStop = false;
            // 
            // grdAbout
            // 
            this.grdAbout.AllowUserToAddRows = false;
            this.grdAbout.AllowUserToDeleteRows = false;
            this.grdAbout.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAbout.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.URL,
            this.LINK_TEXT,
            this.DESCRIPTION,
            this.lang});
            this.grdAbout.Location = new System.Drawing.Point(30, 32);
            this.grdAbout.Name = "grdAbout";
            this.grdAbout.ReadOnly = true;
            this.grdAbout.RowHeadersVisible = false;
            this.grdAbout.Size = new System.Drawing.Size(553, 152);
            this.grdAbout.TabIndex = 0;
            this.grdAbout.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAbout_CellDoubleClick);
            this.grdAbout.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdAbout_KeyUp);
            // 
            // URL
            // 
            this.URL.DataPropertyName = "URL";
            this.URL.HeaderText = "Link URL";
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            this.URL.Width = 230;
            // 
            // LINK_TEXT
            // 
            this.LINK_TEXT.DataPropertyName = "LINK_TEXT";
            this.LINK_TEXT.HeaderText = "Link Text";
            this.LINK_TEXT.Name = "LINK_TEXT";
            this.LINK_TEXT.ReadOnly = true;
            this.LINK_TEXT.Width = 200;
            // 
            // DESCRIPTION
            // 
            this.DESCRIPTION.DataPropertyName = "DESCRIPTION";
            this.DESCRIPTION.HeaderText = "DESCRIPTION";
            this.DESCRIPTION.Name = "DESCRIPTION";
            this.DESCRIPTION.ReadOnly = true;
            this.DESCRIPTION.Width = 200;
            // 
            // lang
            // 
            this.lang.DataPropertyName = "lang";
            this.lang.HeaderText = "language";
            this.lang.Name = "lang";
            this.lang.ReadOnly = true;
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(21, 528);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(604, 121);
            this.pnlURL.TabIndex = 2;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(118, 242);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 46;
            this.lblMsg.Visible = false;
            // 
            // item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.grpboxGrid);
            this.Controls.Add(this.grpboxAdd);
            this.Name = "item";
            this.Size = new System.Drawing.Size(669, 664);
            this.grpboxAdd.ResumeLayout(false);
            this.grpboxAdd.PerformLayout();
            this.grpboxGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAbout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpboxAdd;
        //private System.Windows.Forms.RichTextBox txtDescr;
        //private System.Windows.Forms.ComboBox ddlRelType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        //private System.Windows.Forms.Label label2;
        //private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpboxGrid;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAddURL;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TextBox txtLinkText;
        private System.Windows.Forms.TextBox txtLinkUrl;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView grdAbout;
        //private System.Windows.Forms.DataGridViewTextBoxColumn RELTYPE; // Remove Item relType as per Schema V2.0 Updated by Harish(@17-Dec-2014
        //private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIPTION;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlLangOppName;
        private System.Windows.Forms.DataGridViewTextBoxColumn URL;
        private System.Windows.Forms.DataGridViewTextBoxColumn LINK_TEXT;
        private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIPTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn lang;
        private System.Windows.Forms.Label lblReltype;
        private System.Windows.Forms.ComboBox ddlRelatedItemType;
        private System.Windows.Forms.TextBox txtdurationExpression;
        private System.Windows.Forms.Label lbldurationExpression;
    }
}
