namespace Scival.Award
{
    partial class PageURL
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
            this.grdPageURL = new System.Windows.Forms.DataGridView();
            this.url = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdPageURL)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdPageURL
            // 
            this.grdPageURL.AllowUserToAddRows = false;
            this.grdPageURL.AllowUserToDeleteRows = false;
            this.grdPageURL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPageURL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.url});
            this.grdPageURL.Location = new System.Drawing.Point(28, 31);
            this.grdPageURL.Name = "grdPageURL";
            this.grdPageURL.ReadOnly = true;
            this.grdPageURL.RowHeadersVisible = false;
            this.grdPageURL.Size = new System.Drawing.Size(485, 98);
            this.grdPageURL.TabIndex = 0;
            // 
            // url
            // 
            this.url.DataPropertyName = "URL";
            this.url.HeaderText = "Page URL";
            this.url.Name = "url";
            this.url.ReadOnly = true;
            this.url.Width = 475;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdPageURL);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 151);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Page URL";
            // 
            // PageURL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PageURL";
            this.Size = new System.Drawing.Size(559, 172);
            ((System.ComponentModel.ISupportInitialize)(this.grdPageURL)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdPageURL;
        private System.Windows.Forms.DataGridViewTextBoxColumn url;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
