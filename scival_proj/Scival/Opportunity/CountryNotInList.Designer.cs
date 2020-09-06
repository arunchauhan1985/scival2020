namespace Scival.Opportunity
{
    partial class CountryNotInList
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rchTextRemark = new System.Windows.Forms.RichTextBox();
            this.Lblmsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rchTextRemark
            // 
            this.rchTextRemark.Location = new System.Drawing.Point(3, 39);
            this.rchTextRemark.Name = "rchTextRemark";
            this.rchTextRemark.Size = new System.Drawing.Size(437, 158);
            this.rchTextRemark.TabIndex = 2;
            this.rchTextRemark.Text = "";
            // 
            // Lblmsg
            // 
            this.Lblmsg.AutoSize = true;
            this.Lblmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lblmsg.Location = new System.Drawing.Point(7, 9);
            this.Lblmsg.Name = "Lblmsg";
            this.Lblmsg.Size = new System.Drawing.Size(129, 13);
            this.Lblmsg.TabIndex = 3;
            this.Lblmsg.Text = "Country Not Seleted :";
            // 
            // CountryNotInList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 198);
            this.Controls.Add(this.Lblmsg);
            this.Controls.Add(this.rchTextRemark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CountryNotInList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Country not in list";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rchTextRemark;
        private System.Windows.Forms.Label Lblmsg;
    }
}