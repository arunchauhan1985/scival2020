namespace Scival.Award
{
    partial class RelatedProgram
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RelatedProgram));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdClass = new System.Windows.Forms.DataGridView();
            this.Hierarchy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RelType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RELATEDPROGRAM_TEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label14 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnupdat = new System.Windows.Forms.Button();
            this.btnaddurl = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtProText = new System.Windows.Forms.TextBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.ddlreltype = new System.Windows.Forms.ComboBox();
            this.ddlHerchy = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClass)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdClass);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.btncancel);
            this.groupBox1.Controls.Add(this.btnupdat);
            this.groupBox1.Controls.Add(this.btnaddurl);
            this.groupBox1.Controls.Add(this.btnsave);
            this.groupBox1.Controls.Add(this.txtProText);
            this.groupBox1.Controls.Add(this.txtId);
            this.groupBox1.Controls.Add(this.ddlreltype);
            this.groupBox1.Controls.Add(this.ddlHerchy);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(23, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(568, 428);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Related Program";
            // 
            // grdClass
            // 
            this.grdClass.AllowUserToAddRows = false;
            this.grdClass.AllowUserToDeleteRows = false;
            this.grdClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Hierarchy,
            this.RelType,
            this.ID,
            this.RELATEDPROGRAM_TEXT});
            this.grdClass.Location = new System.Drawing.Point(16, 186);
            this.grdClass.Name = "grdClass";
            this.grdClass.ReadOnly = true;
            this.grdClass.RowHeadersVisible = false;
            this.grdClass.Size = new System.Drawing.Size(513, 210);
            this.grdClass.TabIndex = 42;
            this.grdClass.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdClass_CellDoubleClick);
            this.grdClass.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdClass_KeyUp);
            // 
            // Hierarchy
            // 
            this.Hierarchy.DataPropertyName = "Hierarchy";
            this.Hierarchy.HeaderText = "Hierarchy";
            this.Hierarchy.Name = "Hierarchy";
            this.Hierarchy.ReadOnly = true;
            this.Hierarchy.Width = 125;
            // 
            // RelType
            // 
            this.RelType.DataPropertyName = "RelType";
            this.RelType.HeaderText = "Rel Type";
            this.RelType.Name = "RelType";
            this.RelType.ReadOnly = true;
            this.RelType.Width = 125;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 125;
            // 
            // RELATEDPROGRAM_TEXT
            // 
            this.RELATEDPROGRAM_TEXT.DataPropertyName = "RELATEDPROGRAM_TEXT";
            this.RELATEDPROGRAM_TEXT.HeaderText = "Program Text";
            this.RELATEDPROGRAM_TEXT.Name = "RELATEDPROGRAM_TEXT";
            this.RELATEDPROGRAM_TEXT.ReadOnly = true;
            this.RELATEDPROGRAM_TEXT.Width = 125;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(82, 140);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(123, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "(*) are manadatory fields.";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncancel.BackgroundImage")));
            this.btncancel.Location = new System.Drawing.Point(454, 135);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 11;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Visible = false;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnupdat
            // 
            this.btnupdat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnupdat.BackgroundImage")));
            this.btnupdat.Location = new System.Drawing.Point(375, 135);
            this.btnupdat.Name = "btnupdat";
            this.btnupdat.Size = new System.Drawing.Size(75, 23);
            this.btnupdat.TabIndex = 10;
            this.btnupdat.Text = "Update";
            this.btnupdat.UseVisualStyleBackColor = true;
            this.btnupdat.Visible = false;
            this.btnupdat.Click += new System.EventHandler(this.btnupdat_Click);
            // 
            // btnaddurl
            // 
            this.btnaddurl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnaddurl.BackgroundImage")));
            this.btnaddurl.Location = new System.Drawing.Point(294, 135);
            this.btnaddurl.Name = "btnaddurl";
            this.btnaddurl.Size = new System.Drawing.Size(75, 23);
            this.btnaddurl.TabIndex = 9;
            this.btnaddurl.Text = "Add &URL";
            this.btnaddurl.UseVisualStyleBackColor = true;
            this.btnaddurl.Click += new System.EventHandler(this.btnaddurl_Click);
            // 
            // btnsave
            // 
            this.btnsave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnsave.BackgroundImage")));
            this.btnsave.Location = new System.Drawing.Point(214, 135);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 8;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtProText
            // 
            this.txtProText.Location = new System.Drawing.Point(217, 95);
            this.txtProText.Name = "txtProText";
            this.txtProText.Size = new System.Drawing.Size(260, 20);
            this.txtProText.TabIndex = 7;
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(217, 71);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(260, 20);
            this.txtId.TabIndex = 6;
            // 
            // ddlreltype
            // 
            this.ddlreltype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlreltype.FormattingEnabled = true;
            this.ddlreltype.Location = new System.Drawing.Point(217, 46);
            this.ddlreltype.Name = "ddlreltype";
            this.ddlreltype.Size = new System.Drawing.Size(260, 21);
            this.ddlreltype.TabIndex = 5;
            this.ddlreltype.SelectedIndexChanged += new System.EventHandler(this.ddlreltype_SelectedIndexChanged);
            // 
            // ddlHerchy
            // 
            this.ddlHerchy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlHerchy.FormattingEnabled = true;
            this.ddlHerchy.Location = new System.Drawing.Point(217, 22);
            this.ddlHerchy.Name = "ddlHerchy";
            this.ddlHerchy.Size = new System.Drawing.Size(260, 21);
            this.ddlHerchy.TabIndex = 4;
            this.ddlHerchy.SelectedIndexChanged += new System.EventHandler(this.ddlHerchy_SelectedIndexChanged);
            //pankaj july 2019
            this.ddlHerchy.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlHerchy_MouseWheel);
            this.ddlreltype.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ddlreltype_MouseWheel);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Program Text *";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "ID  *";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "RelType  *";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hierarchy *";
            // 
            // pnlURL
            // 
            this.pnlURL.Location = new System.Drawing.Point(23, 472);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(550, 210);
            this.pnlURL.TabIndex = 1;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(241, 189);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 33;
            this.lblMsg.Visible = false;
            // 
            // RelatedProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.groupBox1);
            this.Name = "RelatedProgram";
            this.Size = new System.Drawing.Size(614, 696);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnupdat;
        private System.Windows.Forms.Button btnaddurl;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtProText;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.ComboBox ddlreltype;
        private System.Windows.Forms.ComboBox ddlHerchy;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView grdClass;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hierarchy;
        private System.Windows.Forms.DataGridViewTextBoxColumn RelType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RELATEDPROGRAM_TEXT;
        private System.Windows.Forms.Label lblMsg;

    }
}
