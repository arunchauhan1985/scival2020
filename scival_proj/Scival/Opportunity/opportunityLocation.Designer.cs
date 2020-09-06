namespace Scival.Opportunity
{
    partial class opportunityLocation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(opportunityLocation));
            this.txtOtherState = new System.Windows.Forms.TextBox();
            this.lblotherstate = new System.Windows.Forms.Label();
            this.ddlState = new System.Windows.Forms.ComboBox();
            this.DDLCOUNTRY = new System.Windows.Forms.ComboBox();
            this.TextPostalCode = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.TextCity = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.TextStreet = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.TextRoom = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtSubmit = new System.Windows.Forms.Button();
            this.grpcontact = new System.Windows.Forms.GroupBox();
            this.btnDeleteGrouping = new System.Windows.Forms.Button();
            this.chk_individual_list = new System.Windows.Forms.CheckedListBox();
            this.ddlcountrylistdtl = new System.Windows.Forms.ListBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnupdate = new System.Windows.Forms.Button();
            this.btnaddurl = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.grdAddress = new System.Windows.Forms.DataGridView();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.postalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpcontact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOtherState
            // 
            this.txtOtherState.Enabled = false;
            this.txtOtherState.Location = new System.Drawing.Point(149, 192);
            this.txtOtherState.Name = "txtOtherState";
            this.txtOtherState.Size = new System.Drawing.Size(336, 20);
            this.txtOtherState.TabIndex = 15;
            this.txtOtherState.Visible = false;
            // 
            // lblotherstate
            // 
            this.lblotherstate.AutoSize = true;
            this.lblotherstate.Location = new System.Drawing.Point(61, 195);
            this.lblotherstate.Name = "lblotherstate";
            this.lblotherstate.Size = new System.Drawing.Size(61, 13);
            this.lblotherstate.TabIndex = 14;
            this.lblotherstate.Text = "Other State";
            this.lblotherstate.Visible = false;
            // 
            // ddlState
            // 
            this.ddlState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlState.FormattingEnabled = true;
            this.ddlState.Location = new System.Drawing.Point(149, 162);
            this.ddlState.Name = "ddlState";
            this.ddlState.Size = new System.Drawing.Size(336, 21);
            this.ddlState.TabIndex = 13;
            this.ddlState.SelectedIndexChanged += new System.EventHandler(this.ddlState_SelectedIndexChanged);
            this.ddlState.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlState_MouseWheel);
            // 
            // DDLCOUNTRY
            // 
            this.DDLCOUNTRY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDLCOUNTRY.Enabled = false;
            this.DDLCOUNTRY.FormattingEnabled = true;
            this.DDLCOUNTRY.Location = new System.Drawing.Point(149, 135);
            this.DDLCOUNTRY.Name = "DDLCOUNTRY";
            this.DDLCOUNTRY.Size = new System.Drawing.Size(336, 21);
            this.DDLCOUNTRY.TabIndex = 12;
            this.DDLCOUNTRY.SelectedIndexChanged += new System.EventHandler(this.DDLCOUNTRY_SelectedIndexChanged);
            this.DDLCOUNTRY.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.DDLCOUNTRY_MouseWheel);
            // 
            // TextPostalCode
            // 
            this.TextPostalCode.AcceptsReturn = true;
            this.TextPostalCode.Location = new System.Drawing.Point(149, 222);
            this.TextPostalCode.Name = "TextPostalCode";
            this.TextPostalCode.Size = new System.Drawing.Size(336, 20);
            this.TextPostalCode.TabIndex = 11;
            this.TextPostalCode.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(61, 225);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "Postal Code";
            this.label18.Visible = false;
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
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(61, 139);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Country";
            // 
            // TextCity
            // 
            this.TextCity.Location = new System.Drawing.Point(149, 110);
            this.TextCity.Name = "TextCity";
            this.TextCity.Size = new System.Drawing.Size(336, 20);
            this.TextCity.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(61, 113);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "City";
            // 
            // TextStreet
            // 
            this.TextStreet.AcceptsReturn = true;
            this.TextStreet.Location = new System.Drawing.Point(149, 84);
            this.TextStreet.Name = "TextStreet";
            this.TextStreet.Size = new System.Drawing.Size(336, 20);
            this.TextStreet.TabIndex = 3;
            this.TextStreet.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(61, 87);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Street";
            this.label16.Visible = false;
            // 
            // TextRoom
            // 
            this.TextRoom.Location = new System.Drawing.Point(149, 58);
            this.TextRoom.Name = "TextRoom";
            this.TextRoom.Size = new System.Drawing.Size(336, 20);
            this.TextRoom.TabIndex = 1;
            this.TextRoom.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(61, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Room";
            this.label17.Visible = false;
            // 
            // txtSubmit
            // 
            this.txtSubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSubmit.BackgroundImage")));
            this.txtSubmit.Location = new System.Drawing.Point(305, 297);
            this.txtSubmit.Name = "txtSubmit";
            this.txtSubmit.Size = new System.Drawing.Size(75, 23);
            this.txtSubmit.TabIndex = 12;
            this.txtSubmit.Text = "Save";
            this.txtSubmit.UseVisualStyleBackColor = true;
            this.txtSubmit.Click += new System.EventHandler(this.txtSubmit_Click);
            // 
            // grpcontact
            // 
            this.grpcontact.Controls.Add(this.btnDeleteGrouping);
            this.grpcontact.Controls.Add(this.chk_individual_list);
            this.grpcontact.Controls.Add(this.ddlcountrylistdtl);
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
            this.grpcontact.Location = new System.Drawing.Point(21, 28);
            this.grpcontact.Name = "grpcontact";
            this.grpcontact.Size = new System.Drawing.Size(750, 331);
            this.grpcontact.TabIndex = 14;
            this.grpcontact.TabStop = false;
            this.grpcontact.Text = "Opportunity Location";
            this.grpcontact.Enter += new System.EventHandler(this.grpcontact_Enter);
            // 
            // btnDeleteGrouping
            // 
            this.btnDeleteGrouping.AccessibleDescription = "ddlElClassText";
            this.btnDeleteGrouping.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteGrouping.BackgroundImage")));
            this.btnDeleteGrouping.Location = new System.Drawing.Point(505, 19);
            this.btnDeleteGrouping.Name = "btnDeleteGrouping";
            this.btnDeleteGrouping.Size = new System.Drawing.Size(93, 23);
            this.btnDeleteGrouping.TabIndex = 79;
            this.btnDeleteGrouping.Text = "Delete Grouping";
            this.btnDeleteGrouping.UseVisualStyleBackColor = true;
            this.btnDeleteGrouping.Click += new System.EventHandler(this.btnDeleteGrouping_Click);
            // 
            // chk_individual_list
            // 
            this.chk_individual_list.FormattingEnabled = true;
            this.chk_individual_list.Location = new System.Drawing.Point(505, 58);
            this.chk_individual_list.Name = "chk_individual_list";
            this.chk_individual_list.Size = new System.Drawing.Size(232, 124);
            this.chk_individual_list.TabIndex = 56;
            this.chk_individual_list.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chk_individual_list_ItemCheck);
            // 
            // ddlcountrylistdtl
            // 
            this.ddlcountrylistdtl.BackColor = System.Drawing.Color.PowderBlue;
            this.ddlcountrylistdtl.Enabled = false;
            this.ddlcountrylistdtl.FormattingEnabled = true;
            this.ddlcountrylistdtl.Location = new System.Drawing.Point(505, 192);
            this.ddlcountrylistdtl.Name = "ddlcountrylistdtl";
            this.ddlcountrylistdtl.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ddlcountrylistdtl.Size = new System.Drawing.Size(232, 121);
            this.ddlcountrylistdtl.TabIndex = 55;
            this.ddlcountrylistdtl.SelectedIndexChanged += new System.EventHandler(this.ddlcountrylistdtl_SelectedIndexChanged);
            this.ddlcountrylistdtl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlcountrylistdtl_MouseWheel);
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
            // btnupdate
            // 
            this.btnupdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnupdate.BackgroundImage")));
            this.btnupdate.Location = new System.Drawing.Point(144, 297);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(75, 23);
            this.btnupdate.TabIndex = 17;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Visible = false;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // btnaddurl
            // 
            this.btnaddurl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnaddurl.BackgroundImage")));
            this.btnaddurl.Location = new System.Drawing.Point(386, 297);
            this.btnaddurl.Name = "btnaddurl";
            this.btnaddurl.Size = new System.Drawing.Size(75, 23);
            this.btnaddurl.TabIndex = 15;
            this.btnaddurl.Text = "Add &URL";
            this.btnaddurl.UseVisualStyleBackColor = true;
            this.btnaddurl.Click += new System.EventHandler(this.btnaddurl_Click);
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncancel.BackgroundImage")));
            this.btncancel.Location = new System.Drawing.Point(225, 297);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 16;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Visible = false;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
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
            this.dataGridViewTextBoxColumn15,
            this.postalCode});
            this.grdAddress.Location = new System.Drawing.Point(21, 381);
            this.grdAddress.Name = "grdAddress";
            this.grdAddress.ReadOnly = true;
            this.grdAddress.RowHeadersVisible = false;
            this.grdAddress.Size = new System.Drawing.Size(692, 116);
            this.grdAddress.TabIndex = 21;
            this.grdAddress.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdContact_CellDoubleClick);
            this.grdAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdContact_KeyUp);
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(21, 517);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(692, 159);
            this.pnlURL.TabIndex = 22;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "room";
            this.dataGridViewTextBoxColumn11.HeaderText = "Room";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "street";
            this.dataGridViewTextBoxColumn12.HeaderText = "Street";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Visible = false;
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
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "countryname";
            this.dataGridViewTextBoxColumn15.HeaderText = "Country";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // postalCode
            // 
            this.postalCode.DataPropertyName = "postalCode";
            this.postalCode.HeaderText = "PostalCode";
            this.postalCode.Name = "postalCode";
            this.postalCode.ReadOnly = true;
            this.postalCode.Visible = false;
            // 
            // opportunityLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.grdAddress);
            this.Controls.Add(this.grpcontact);
            this.Name = "opportunityLocation";
            this.Size = new System.Drawing.Size(774, 718);
            this.grpcontact.ResumeLayout(false);
            this.grpcontact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TextPostalCode;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TextCity;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TextStreet;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox TextRoom;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox ddlState;
        private System.Windows.Forms.ComboBox DDLCOUNTRY;
        private System.Windows.Forms.Button txtSubmit;
        private System.Windows.Forms.GroupBox grpcontact;
        private System.Windows.Forms.Button btnaddurl;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.DataGridView grdAddress;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.TextBox txtOtherState;
        private System.Windows.Forms.Label lblotherstate;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.CheckedListBox chk_individual_list;
        private System.Windows.Forms.ListBox ddlcountrylistdtl;
        private System.Windows.Forms.Button btnDeleteGrouping;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn postalCode;
    }
}
