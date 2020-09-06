namespace Scival.FundingBody
{
    partial class EstablishmentInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EstablishmentInfo));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.txtState = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.richtxtDesc = new System.Windows.Forms.RichTextBox();
            this.ddlState = new System.Windows.Forms.ComboBox();
            this.ddlCountry = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtcity = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTodate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.ddlLang_EstInfo = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ddlLang_EstInfo);
            this.groupBox1.Controls.Add(this.lblMsg);
            this.groupBox1.Controls.Add(this.txtState);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.btnsave);
            this.groupBox1.Controls.Add(this.richtxtDesc);
            this.groupBox1.Controls.Add(this.ddlState);
            this.groupBox1.Controls.Add(this.ddlCountry);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtcity);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTodate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 387);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Establishment Info";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(258, 363);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 51;
            this.lblMsg.Visible = false;
            // 
            // txtState
            // 
            this.txtState.Enabled = false;
            this.txtState.Location = new System.Drawing.Point(181, 152);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(258, 20);
            this.txtState.TabIndex = 43;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(109, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Other State";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(112, 345);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // button2
            // 
            //this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.Location = new System.Drawing.Point(361, 329);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Add &URL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnsave
            // 
            //this.btnsave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnsave.BackgroundImage")));
            this.btnsave.Location = new System.Drawing.Point(255, 329);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 10;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // richtxtDesc
            // 
            this.richtxtDesc.Location = new System.Drawing.Point(181, 187);
            this.richtxtDesc.Name = "richtxtDesc";
            this.richtxtDesc.Size = new System.Drawing.Size(258, 135);
            this.richtxtDesc.TabIndex = 9;
            this.richtxtDesc.Text = "";
            // 
            // ddlState
            // 
            this.ddlState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlState.FormattingEnabled = true;
            this.ddlState.Location = new System.Drawing.Point(181, 117);
            this.ddlState.Name = "ddlState";
            this.ddlState.Size = new System.Drawing.Size(258, 21);
            this.ddlState.TabIndex = 8;
            this.ddlState.SelectedIndexChanged += new System.EventHandler(this.ddlState_SelectedIndexChanged);



            // 
            // ddlCountry
            // 
            this.ddlCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCountry.FormattingEnabled = true;
            this.ddlCountry.Location = new System.Drawing.Point(181, 84);
            this.ddlCountry.Name = "ddlCountry";
            this.ddlCountry.Size = new System.Drawing.Size(258, 21);
            this.ddlCountry.TabIndex = 7;
            this.ddlCountry.SelectedIndexChanged += new System.EventHandler(this.ddlCountry_SelectedIndexChanged);

            //pankaj july 2019
            this.ddlCountry.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlCountry_MouseWheel);
            this.ddlState.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlState_MouseWheel);
            // 
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(112, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Description";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "State";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Country *";
            // 
            // txtcity
            // 
            this.txtcity.Location = new System.Drawing.Point(181, 52);
            this.txtcity.Name = "txtcity";
            this.txtcity.Size = new System.Drawing.Size(258, 20);
            this.txtcity.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "City";
            // 
            // txtTodate
            // 
            this.txtTodate.Location = new System.Drawing.Point(181, 24);
            this.txtTodate.MaxLength = 4;
            this.txtTodate.Name = "txtTodate";
            this.txtTodate.Size = new System.Drawing.Size(258, 20);
            this.txtTodate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Year *";
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(16, 429);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(554, 157);
            this.pnlURL.TabIndex = 1;
            // 
            // ddlLang_EstInfo
            // 
            this.ddlLang_EstInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLang_EstInfo.FormattingEnabled = true;
            this.ddlLang_EstInfo.Items.AddRange(new object[] {
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
            this.ddlLang_EstInfo.Location = new System.Drawing.Point(445, 187);
            this.ddlLang_EstInfo.Name = "ddlLang_EstInfo";
            this.ddlLang_EstInfo.Size = new System.Drawing.Size(88, 21);
            this.ddlLang_EstInfo.TabIndex = 74;
            // 
            // EstablishmentInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox1);
            this.Name = "EstablishmentInfo";
            this.Size = new System.Drawing.Size(608, 640);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richtxtDesc;
        private System.Windows.Forms.ComboBox ddlState;
        private System.Windows.Forms.ComboBox ddlCountry;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtcity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTodate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.ComboBox ddlLang_EstInfo;
    }
}
