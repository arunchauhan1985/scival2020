namespace Scival.Award
{
    partial class AwardLocation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AwardLocation));
            this.pnlURL = new System.Windows.Forms.Panel();
            this.grdAddress = new System.Windows.Forms.DataGridView();
            this.grpcontact = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txttestcountry = new System.Windows.Forms.TextBox();
            this.txtOtherState = new System.Windows.Forms.TextBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblotherstate = new System.Windows.Forms.Label();
            this.ddlState = new System.Windows.Forms.ComboBox();
            this.btnupdate = new System.Windows.Forms.Button();
            this.DDLCOUNTRY = new System.Windows.Forms.ComboBox();
            this.txtSubmit = new System.Windows.Forms.Button();
            this.TextPostalCode = new System.Windows.Forms.TextBox();
            this.btnaddurl = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.TextRoom = new System.Windows.Forms.TextBox();
            this.TextCity = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.TextStreet = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdAddress)).BeginInit();
            this.grpcontact.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlURL
            // 
            this.pnlURL.Enabled = false;
            this.pnlURL.Location = new System.Drawing.Point(66, 529);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(632, 159);
            this.pnlURL.TabIndex = 25;
            this.pnlURL.Visible = false;
            // 
            // grdAddress
            // 
            this.grdAddress.AllowUserToAddRows = false;
            this.grdAddress.AllowUserToDeleteRows = false;
            this.grdAddress.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdAddress.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAddress.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn27,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn26});
            this.grdAddress.Location = new System.Drawing.Point(66, 393);
            this.grdAddress.Name = "grdAddress";
            this.grdAddress.ReadOnly = true;
            this.grdAddress.RowHeadersVisible = false;
            this.grdAddress.Size = new System.Drawing.Size(632, 88);
            this.grdAddress.TabIndex = 24;
            this.grdAddress.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdContact_CellContentDoubleClick);
            this.grdAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdContact_KeyUp);
            // 
            // grpcontact
            // 
            this.grpcontact.Controls.Add(this.label1);
            this.grpcontact.Controls.Add(this.txttestcountry);
            this.grpcontact.Controls.Add(this.txtOtherState);
            this.grpcontact.Controls.Add(this.lblMsg);
            this.grpcontact.Controls.Add(this.lblotherstate);
            this.grpcontact.Controls.Add(this.ddlState);
            this.grpcontact.Controls.Add(this.btnupdate);
            this.grpcontact.Controls.Add(this.DDLCOUNTRY);
            this.grpcontact.Controls.Add(this.txtSubmit);
            this.grpcontact.Controls.Add(this.TextPostalCode);
            this.grpcontact.Controls.Add(this.btnaddurl);
            this.grpcontact.Controls.Add(this.label18);
            this.grpcontact.Controls.Add(this.btncancel);
            this.grpcontact.Controls.Add(this.label11);
            this.grpcontact.Controls.Add(this.label17);
            this.grpcontact.Controls.Add(this.label12);
            this.grpcontact.Controls.Add(this.TextRoom);
            this.grpcontact.Controls.Add(this.TextCity);
            this.grpcontact.Controls.Add(this.label16);
            this.grpcontact.Controls.Add(this.label13);
            this.grpcontact.Controls.Add(this.TextStreet);
            this.grpcontact.Location = new System.Drawing.Point(66, 28);
            this.grpcontact.Name = "grpcontact";
            this.grpcontact.Size = new System.Drawing.Size(632, 343);
            this.grpcontact.TabIndex = 23;
            this.grpcontact.TabStop = false;
            this.grpcontact.Text = "Award Location";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "other State";
            // 
            // txttestcountry
            // 
            this.txttestcountry.Location = new System.Drawing.Point(149, 216);
            this.txttestcountry.Name = "txttestcountry";
            this.txttestcountry.Size = new System.Drawing.Size(336, 20);
            this.txttestcountry.TabIndex = 7;
            this.txttestcountry.Visible = false;
            // 
            // txtOtherState
            // 
            this.txtOtherState.Location = new System.Drawing.Point(149, 192);
            this.txtOtherState.Name = "txtOtherState";
            this.txtOtherState.Size = new System.Drawing.Size(336, 20);
            this.txtOtherState.TabIndex = 6;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(240, 269);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(41, 13);
            this.lblMsg.TabIndex = 43;
            this.lblMsg.Text = "ghjhgj";
            this.lblMsg.Visible = false;
            // 
            // lblotherstate
            // 
            this.lblotherstate.AutoSize = true;
            this.lblotherstate.Location = new System.Drawing.Point(58, 216);
            this.lblotherstate.Name = "lblotherstate";
            this.lblotherstate.Size = new System.Drawing.Size(67, 13);
            this.lblotherstate.TabIndex = 14;
            this.lblotherstate.Text = "Test Country";
            this.lblotherstate.Visible = false;
            // 
            // ddlState
            // 
            this.ddlState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlState.FormattingEnabled = true;
            this.ddlState.Location = new System.Drawing.Point(149, 162);
            this.ddlState.Name = "ddlState";
            this.ddlState.Size = new System.Drawing.Size(336, 21);
            this.ddlState.TabIndex = 5;

            this.DDLCOUNTRY.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.DDLCOUNTRY_MouseWheel);
            this.ddlState.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlState_MouseWheel);
            // 
            // btnupdate
            // 
            this.btnupdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnupdate.BackgroundImage")));
            this.btnupdate.Location = new System.Drawing.Point(144, 297);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(75, 23);
            this.btnupdate.TabIndex = 9;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Visible = false;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // DDLCOUNTRY
            // 
            this.DDLCOUNTRY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDLCOUNTRY.FormattingEnabled = true;
            this.DDLCOUNTRY.Location = new System.Drawing.Point(149, 132);
            this.DDLCOUNTRY.Name = "DDLCOUNTRY";
            this.DDLCOUNTRY.Size = new System.Drawing.Size(336, 21);
            this.DDLCOUNTRY.TabIndex = 4;
            this.DDLCOUNTRY.SelectedIndexChanged += new System.EventHandler(this.DDLCOUNTRY_SelectedIndexChanged);
            // 
            // txtSubmit
            // 
            this.txtSubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSubmit.BackgroundImage")));
            this.txtSubmit.Location = new System.Drawing.Point(305, 297);
            this.txtSubmit.Name = "txtSubmit";
            this.txtSubmit.Size = new System.Drawing.Size(75, 23);
            this.txtSubmit.TabIndex = 11;
            this.txtSubmit.Text = "Save";
            this.txtSubmit.UseVisualStyleBackColor = true;
            this.txtSubmit.Click += new System.EventHandler(this.txtSubmit_Click);
            // 
            // TextPostalCode
            // 
            this.TextPostalCode.AcceptsReturn = true;
            this.TextPostalCode.Location = new System.Drawing.Point(149, 243);
            this.TextPostalCode.Name = "TextPostalCode";
            this.TextPostalCode.Size = new System.Drawing.Size(336, 20);
            this.TextPostalCode.TabIndex = 8;
            // 
            // btnaddurl
            // 
            this.btnaddurl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnaddurl.BackgroundImage")));
            this.btnaddurl.Location = new System.Drawing.Point(386, 297);
            this.btnaddurl.Name = "btnaddurl";
            this.btnaddurl.Size = new System.Drawing.Size(75, 23);
            this.btnaddurl.TabIndex = 10;
            this.btnaddurl.Text = "Add &URL";
            this.btnaddurl.UseVisualStyleBackColor = true;
            this.btnaddurl.Visible = false;
            this.btnaddurl.Click += new System.EventHandler(this.btnaddurl_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(61, 243);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "Postal Code";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncancel.BackgroundImage")));
            this.btncancel.Location = new System.Drawing.Point(225, 297);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 12;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Visible = false;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(61, 165);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "State";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(61, 37);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Room";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(61, 135);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Country";
            // 
            // TextRoom
            // 
            this.TextRoom.Location = new System.Drawing.Point(149, 37);
            this.TextRoom.Name = "TextRoom";
            this.TextRoom.Size = new System.Drawing.Size(336, 20);
            this.TextRoom.TabIndex = 1;
            // 
            // TextCity
            // 
            this.TextCity.Location = new System.Drawing.Point(149, 98);
            this.TextCity.Name = "TextCity";
            this.TextCity.Size = new System.Drawing.Size(336, 20);
            this.TextCity.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(61, 69);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Street";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(61, 98);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "City";
            // 
            // TextStreet
            // 
            this.TextStreet.AcceptsReturn = true;
            this.TextStreet.Location = new System.Drawing.Point(149, 69);
            this.TextStreet.Name = "TextStreet";
            this.TextStreet.Size = new System.Drawing.Size(336, 20);
            this.TextStreet.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "room";
            this.dataGridViewTextBoxColumn11.HeaderText = "Room";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "street";
            this.dataGridViewTextBoxColumn12.HeaderText = "Street";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "city";
            this.dataGridViewTextBoxColumn13.HeaderText = "City";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "statename";
            this.dataGridViewTextBoxColumn14.HeaderText = "State";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn27
            // 
            this.dataGridViewTextBoxColumn27.DataPropertyName = "COUNTRYTEST";
            this.dataGridViewTextBoxColumn27.HeaderText = "Test Country";
            this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
            this.dataGridViewTextBoxColumn27.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "countryname";
            this.dataGridViewTextBoxColumn15.HeaderText = "Country";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn26
            // 
            this.dataGridViewTextBoxColumn26.DataPropertyName = "postalCode";
            this.dataGridViewTextBoxColumn26.HeaderText = "PostalCode";
            this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
            this.dataGridViewTextBoxColumn26.ReadOnly = true;
            // 
            // AwardLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.grdAddress);
            this.Controls.Add(this.grpcontact);
            this.Name = "AwardLocation";
            this.Size = new System.Drawing.Size(1184, 729);
            ((System.ComponentModel.ISupportInitialize)(this.grdAddress)).EndInit();
            this.grpcontact.ResumeLayout(false);
            this.grpcontact.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.DataGridView grdAddress;
        private System.Windows.Forms.GroupBox grpcontact;
        private System.Windows.Forms.TextBox txtOtherState;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblotherstate;
        private System.Windows.Forms.ComboBox ddlState;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.ComboBox DDLCOUNTRY;
        private System.Windows.Forms.Button txtSubmit;
        private System.Windows.Forms.TextBox TextPostalCode;
        private System.Windows.Forms.Button btnaddurl;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TextRoom;
        private System.Windows.Forms.TextBox TextCity;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TextStreet;
        private System.Windows.Forms.TextBox txttestcountry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
    }
}
