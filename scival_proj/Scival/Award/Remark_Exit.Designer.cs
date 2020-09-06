namespace Scival.Award
{
    partial class Remark_Exit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Remark_Exit));
            this.lblremark = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rchTextRemark = new System.Windows.Forms.RichTextBox();
            this.btnsubmit = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblremark
            // 
            this.lblremark.AutoSize = true;
            this.lblremark.Location = new System.Drawing.Point(21, 24);
            this.lblremark.Name = "lblremark";
            this.lblremark.Size = new System.Drawing.Size(0, 13);
            this.lblremark.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Remark";
            // 
            // rchTextRemark
            // 
            this.rchTextRemark.Location = new System.Drawing.Point(24, 99);
            this.rchTextRemark.Name = "rchTextRemark";
            this.rchTextRemark.Size = new System.Drawing.Size(433, 96);
            this.rchTextRemark.TabIndex = 2;
            this.rchTextRemark.Text = "";
            this.rchTextRemark.TextChanged += new System.EventHandler(this.rchTextRemark_TextChanged);
            // 
            // btnsubmit
            // 
            this.btnsubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnsubmit.BackgroundImage")));
            this.btnsubmit.Location = new System.Drawing.Point(301, 218);
            this.btnsubmit.Name = "btnsubmit";
            this.btnsubmit.Size = new System.Drawing.Size(75, 23);
            this.btnsubmit.TabIndex = 3;
            this.btnsubmit.Text = "Submit";
            this.btnsubmit.UseVisualStyleBackColor = true;
            this.btnsubmit.Click += new System.EventHandler(this.btnsubmit_Click);
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncancel.BackgroundImage")));
            this.btncancel.Location = new System.Drawing.Point(382, 218);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(75, 23);
            this.btncancel.TabIndex = 4;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // Remark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 253);
            this.ControlBox = false;
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnsubmit);
            this.Controls.Add(this.rchTextRemark);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblremark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Remark";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remark";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblremark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rchTextRemark;
        private System.Windows.Forms.Button btnsubmit;
        public System.Windows.Forms.Button btncancel;

    }
}