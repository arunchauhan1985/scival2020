namespace Scival.Opportunity
{
    partial class ChangeHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeHistory));
            this.grpboxAdd = new System.Windows.Forms.GroupBox();
            this.txtPostDate_Cal = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChangeText = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.txtPostDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grpboxGrid = new System.Windows.Forms.GroupBox();
            this.grdChangeHistory = new System.Windows.Forms.DataGridView();
            this.lblMsg = new System.Windows.Forms.Label();
            this.change_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.change_text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.postDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpboxAdd.SuspendLayout();
            this.grpboxGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxAdd
            // 
            this.grpboxAdd.Controls.Add(this.txtPostDate_Cal);
            this.grpboxAdd.Controls.Add(this.label1);
            this.grpboxAdd.Controls.Add(this.txtChangeText);
            this.grpboxAdd.Controls.Add(this.label14);
            this.grpboxAdd.Controls.Add(this.btnCancel);
            this.grpboxAdd.Controls.Add(this.btnUpdate);
            this.grpboxAdd.Controls.Add(this.BtnAdd);
            this.grpboxAdd.Controls.Add(this.txtPostDate);
            this.grpboxAdd.Controls.Add(this.label3);
            this.grpboxAdd.Location = new System.Drawing.Point(21, 15);
            this.grpboxAdd.Name = "grpboxAdd";
            this.grpboxAdd.Size = new System.Drawing.Size(604, 191);
            this.grpboxAdd.TabIndex = 0;
            this.grpboxAdd.TabStop = false;
            this.grpboxAdd.Text = "Change History";
            // 
            // txtPostDate_Cal
            // 
            this.txtPostDate_Cal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtPostDate_Cal.Location = new System.Drawing.Point(386, 27);
            this.txtPostDate_Cal.Name = "txtPostDate_Cal";
            this.txtPostDate_Cal.Size = new System.Drawing.Size(98, 20);
            this.txtPostDate_Cal.TabIndex = 118;
            this.txtPostDate_Cal.ValueChanged += new System.EventHandler(this.txtPostDate_Cal_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "Change Text *";
            // 
            // txtChangeText
            // 
            this.txtChangeText.Location = new System.Drawing.Point(219, 54);
            this.txtChangeText.Multiline = true;
            this.txtChangeText.Name = "txtChangeText";
            this.txtChangeText.Size = new System.Drawing.Size(267, 62);
            this.txtChangeText.TabIndex = 45;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(81, 151);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.Location = new System.Drawing.Point(395, 146);
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
            this.btnUpdate.Location = new System.Drawing.Point(300, 146);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnAdd.BackgroundImage")));
            this.BtnAdd.Location = new System.Drawing.Point(219, 146);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 8;
            this.BtnAdd.Text = "Save";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtPostDate
            // 
            this.txtPostDate.Location = new System.Drawing.Point(219, 27);
            this.txtPostDate.Name = "txtPostDate";
            this.txtPostDate.Size = new System.Drawing.Size(156, 20);
            this.txtPostDate.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Post Date *";
            // 
            // grpboxGrid
            // 
            this.grpboxGrid.Controls.Add(this.grdChangeHistory);
            this.grpboxGrid.Location = new System.Drawing.Point(21, 217);
            this.grpboxGrid.Name = "grpboxGrid";
            this.grpboxGrid.Size = new System.Drawing.Size(604, 216);
            this.grpboxGrid.TabIndex = 1;
            this.grpboxGrid.TabStop = false;
            // 
            // grdChangeHistory
            // 
            this.grdChangeHistory.AllowUserToAddRows = false;
            this.grdChangeHistory.AllowUserToDeleteRows = false;
            this.grdChangeHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdChangeHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.change_Id,
            this.change_text,
            this.version,
            this.postDate,
            this.type});
            this.grdChangeHistory.Location = new System.Drawing.Point(30, 32);
            this.grdChangeHistory.Name = "grdChangeHistory";
            this.grdChangeHistory.ReadOnly = true;
            this.grdChangeHistory.RowHeadersVisible = false;
            this.grdChangeHistory.Size = new System.Drawing.Size(553, 152);
            this.grdChangeHistory.TabIndex = 0;
            this.grdChangeHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAbout_CellDoubleClick);
            this.grdChangeHistory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdAbout_KeyUp);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(244, 186);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 46;
            this.lblMsg.Visible = false;
            // 
            // change_Id
            // 
            this.change_Id.DataPropertyName = "change_Id";
            this.change_Id.HeaderText = "change_Id";
            this.change_Id.Name = "change_Id";
            this.change_Id.ReadOnly = true;
            this.change_Id.Visible = false;
            this.change_Id.Width = 230;
            // 
            // change_text
            // 
            this.change_text.DataPropertyName = "change_text";
            this.change_text.HeaderText = "Change Text";
            this.change_text.Name = "change_text";
            this.change_text.ReadOnly = true;
            this.change_text.Width = 200;
            // 
            // version
            // 
            this.version.DataPropertyName = "version";
            this.version.HeaderText = "version";
            this.version.Name = "version";
            this.version.ReadOnly = true;
            this.version.Width = 200;
            // 
            // postDate
            // 
            this.postDate.DataPropertyName = "postDate";
            this.postDate.HeaderText = "postDate";
            this.postDate.Name = "postDate";
            this.postDate.ReadOnly = true;
            // 
            // type
            // 
            this.type.DataPropertyName = "type";
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            // 
            // changeHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.grpboxGrid);
            this.Controls.Add(this.grpboxAdd);
            this.Name = "changeHistory";
            this.Size = new System.Drawing.Size(669, 664);
            this.grpboxAdd.ResumeLayout(false);
            this.grpboxAdd.PerformLayout();
            this.grpboxGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpboxAdd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpboxGrid;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TextBox txtPostDate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView grdChangeHistory;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TextBox txtChangeText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txtPostDate_Cal;
        private System.Windows.Forms.DataGridViewTextBoxColumn change_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn change_text;
        private System.Windows.Forms.DataGridViewTextBoxColumn version;
        private System.Windows.Forms.DataGridViewTextBoxColumn postDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
    }
}
