namespace Scival.Award
{
    partial class Amount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Amount));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpName = new System.Windows.Forms.GroupBox();
            this.Add = new System.Windows.Forms.Button();
            this.dtGridInstallment = new System.Windows.Forms.DataGridView();
            this.sequence_tran_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Currency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstallmentAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstallmentStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstallmentEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewLinkColumn2 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_start = new System.Windows.Forms.DateTimePicker();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.btnAddurl = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlCuurency = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.grpName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridInstallment)).BeginInit();
            this.SuspendLayout();
            // 
            // grpName
            // 
            this.grpName.Controls.Add(this.Add);
            this.grpName.Controls.Add(this.dtGridInstallment);
            this.grpName.Controls.Add(this.dateTimePickerEnd);
            this.grpName.Controls.Add(this.dateTimePicker_start);
            this.grpName.Controls.Add(this.txtTotalAmount);
            this.grpName.Controls.Add(this.label5);
            this.grpName.Controls.Add(this.label4);
            this.grpName.Controls.Add(this.label3);
            this.grpName.Controls.Add(this.lblMsg);
            this.grpName.Controls.Add(this.btndelete);
            this.grpName.Controls.Add(this.btnAddurl);
            this.grpName.Controls.Add(this.btnSave);
            this.grpName.Controls.Add(this.txtAmount);
            this.grpName.Controls.Add(this.label2);
            this.grpName.Controls.Add(this.ddlCuurency);
            this.grpName.Controls.Add(this.label1);
            this.grpName.Location = new System.Drawing.Point(26, 17);
            this.grpName.Name = "grpName";
            this.grpName.Size = new System.Drawing.Size(612, 421);
            this.grpName.TabIndex = 0;
            this.grpName.TabStop = false;
            // 
            // Add
            // 
            this.Add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Add.BackgroundImage")));
            this.Add.Location = new System.Drawing.Point(404, 174);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 86;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // dtGridInstallment
            // 
            this.dtGridInstallment.AllowUserToAddRows = false;
            this.dtGridInstallment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGridInstallment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGridInstallment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridInstallment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sequence_tran_id,
            this.Currency,
            this.InstallmentAmount,
            this.InstallmentStart,
            this.InstallmentEnd,
            this.dataGridViewLinkColumn2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtGridInstallment.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGridInstallment.Location = new System.Drawing.Point(101, 203);
            this.dtGridInstallment.Name = "dtGridInstallment";
            this.dtGridInstallment.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGridInstallment.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtGridInstallment.RowHeadersVisible = false;
            this.dtGridInstallment.Size = new System.Drawing.Size(493, 126);
            this.dtGridInstallment.TabIndex = 85;
            this.dtGridInstallment.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridInstallment_CellContentClick);
            // 
            // sequence_tran_id
            // 
            this.sequence_tran_id.DataPropertyName = "sequence_id";
            this.sequence_tran_id.HeaderText = "TID";
            this.sequence_tran_id.Name = "sequence_tran_id";
            this.sequence_tran_id.ReadOnly = true;
            this.sequence_tran_id.Visible = false;
            // 
            // Currency
            // 
            this.Currency.DataPropertyName = "Currency";
            this.Currency.HeaderText = "Currency";
            this.Currency.Name = "Currency";
            this.Currency.ReadOnly = true;
            this.Currency.Width = 60;
            // 
            // InstallmentAmount
            // 
            this.InstallmentAmount.DataPropertyName = "InstallmentAmount";
            this.InstallmentAmount.HeaderText = "Installment Amt";
            this.InstallmentAmount.Name = "InstallmentAmount";
            this.InstallmentAmount.ReadOnly = true;
            this.InstallmentAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InstallmentAmount.Width = 120;
            // 
            // InstallmentStart
            // 
            this.InstallmentStart.DataPropertyName = "InstallmentStart";
            this.InstallmentStart.HeaderText = "InstallmentStart";
            this.InstallmentStart.Name = "InstallmentStart";
            this.InstallmentStart.ReadOnly = true;
            this.InstallmentStart.Width = 120;
            // 
            // InstallmentEnd
            // 
            this.InstallmentEnd.DataPropertyName = "InstallmentEnd";
            this.InstallmentEnd.HeaderText = "InstallmentEnd";
            this.InstallmentEnd.Name = "InstallmentEnd";
            this.InstallmentEnd.ReadOnly = true;
            this.InstallmentEnd.Width = 120;
            // 
            // dataGridViewLinkColumn2
            // 
            this.dataGridViewLinkColumn2.DataPropertyName = "lnkColumn";
            this.dataGridViewLinkColumn2.HeaderText = "Action";
            this.dataGridViewLinkColumn2.Name = "dataGridViewLinkColumn2";
            this.dataGridViewLinkColumn2.ReadOnly = true;
            this.dataGridViewLinkColumn2.Text = "Delete";
            this.dataGridViewLinkColumn2.UseColumnTextForLinkValue = true;
            this.dataGridViewLinkColumn2.Width = 70;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "yyyy";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(278, 148);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(201, 20);
            this.dateTimePickerEnd.TabIndex = 15;
            this.dateTimePickerEnd.ValueChanged += new System.EventHandler(this.dateTimePickerEnd_ValueChanged);
            // 
            // dateTimePicker_start
            // 
            this.dateTimePicker_start.CustomFormat = "yyyy";
            this.dateTimePicker_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_start.Location = new System.Drawing.Point(278, 106);
            this.dateTimePicker_start.Name = "dateTimePicker_start";
            this.dateTimePicker_start.Size = new System.Drawing.Size(201, 20);
            this.dateTimePicker_start.TabIndex = 14;
            this.dateTimePicker_start.Value = new System.DateTime(2018, 9, 6, 13, 19, 1, 0);
            this.dateTimePicker_start.ValueChanged += new System.EventHandler(this.dateTimePicker_start_ValueChanged);
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(278, 351);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(201, 20);
            this.txtTotalAmount.TabIndex = 13;
            this.txtTotalAmount.Text = "0";
            this.txtTotalAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTotalAmount_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(116, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "TotalAmount";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(119, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "InstallmentEnd";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(116, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "InstallmentStart";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(195, 277);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 7;
            this.lblMsg.Visible = false;
            // 
            // btndelete
            // 
            this.btndelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btndelete.BackgroundImage")));
            this.btndelete.Location = new System.Drawing.Point(404, 392);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(75, 23);
            this.btndelete.TabIndex = 6;
            this.btndelete.Text = "Delete";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Visible = false;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // btnAddurl
            // 
            this.btnAddurl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddurl.BackgroundImage")));
            this.btnAddurl.Location = new System.Drawing.Point(278, 392);
            this.btnAddurl.Name = "btnAddurl";
            this.btnAddurl.Size = new System.Drawing.Size(75, 23);
            this.btnAddurl.TabIndex = 5;
            this.btnAddurl.Text = "Add &URL";
            this.btnAddurl.UseVisualStyleBackColor = true;
            this.btnAddurl.Click += new System.EventHandler(this.btnAddurl_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Location = new System.Drawing.Point(168, 392);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(278, 66);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(201, 20);
            this.txtAmount.TabIndex = 3;
            this.txtAmount.Text = "0";
            this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAmount_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Installment Amount";
            // 
            // ddlCuurency
            // 
            this.ddlCuurency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCuurency.FormattingEnabled = true;
            this.ddlCuurency.Location = new System.Drawing.Point(278, 28);
            this.ddlCuurency.Name = "ddlCuurency";
            this.ddlCuurency.Size = new System.Drawing.Size(201, 21);
            this.ddlCuurency.TabIndex = 1;

            this.ddlCuurency.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlCuurency_MouseWheel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Currency";
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(38, 475);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(612, 213);
            this.pnlURL.TabIndex = 1;
            // 
            // Amount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.grpName);
            this.Name = "Amount";
            this.Size = new System.Drawing.Size(692, 688);
            this.grpName.ResumeLayout(false);
            this.grpName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridInstallment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlCuurency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Button btnAddurl;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePicker_start;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.DataGridView dtGridInstallment;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequence_tran_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Currency;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstallmentAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstallmentStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstallmentEnd;
        private System.Windows.Forms.DataGridViewLinkColumn dataGridViewLinkColumn2;
    }
}
