namespace Scival
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.BTexit = new System.Windows.Forms.Button();
            this.Passwordtxt = new System.Windows.Forms.TextBox();
            this.UserNametxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BT_Login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTexit
            // 
            this.BTexit.BackgroundImage = global::Scival.Properties.Resources.BTexit_Image;
            this.BTexit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTexit.Location = new System.Drawing.Point(109, 206);
            this.BTexit.Margin = new System.Windows.Forms.Padding(1);
            this.BTexit.Name = "BTexit";
            this.BTexit.Size = new System.Drawing.Size(59, 26);
            this.BTexit.TabIndex = 4;
            this.BTexit.UseVisualStyleBackColor = true;
            this.BTexit.Click += new System.EventHandler(this.BTexit_Click);
            // 
            // Passwordtxt
            // 
            this.Passwordtxt.AcceptsReturn = true;
            this.Passwordtxt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Passwordtxt.Location = new System.Drawing.Point(44, 171);
            this.Passwordtxt.Name = "Passwordtxt";
            this.Passwordtxt.PasswordChar = '*';
            this.Passwordtxt.Size = new System.Drawing.Size(161, 20);
            this.Passwordtxt.TabIndex = 2;
            // 
            // UserNametxt
            // 
            this.UserNametxt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.UserNametxt.Location = new System.Drawing.Point(44, 112);
            this.UserNametxt.Name = "UserNametxt";
            this.UserNametxt.Size = new System.Drawing.Size(161, 20);
            this.UserNametxt.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))));
            this.label2.Location = new System.Drawing.Point(41, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))));
            this.label1.Location = new System.Drawing.Point(41, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Name";
            // 
            // BT_Login
            // 
            this.BT_Login.BackgroundImage = global::Scival.Properties.Resources.BT_Login_Image;
            this.BT_Login.Location = new System.Drawing.Point(44, 206);
            this.BT_Login.Margin = new System.Windows.Forms.Padding(1);
            this.BT_Login.Name = "BT_Login";
            this.BT_Login.Size = new System.Drawing.Size(59, 26);
            this.BT_Login.TabIndex = 3;
            this.BT_Login.UseVisualStyleBackColor = true;
            this.BT_Login.Click += new System.EventHandler(this.BT_Login_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Scival.Properties.Resources.scival_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.BTexit;
            this.ClientSize = new System.Drawing.Size(508, 294);
            this.ControlBox = false;
            this.Controls.Add(this.BTexit);
            this.Controls.Add(this.Passwordtxt);
            this.Controls.Add(this.UserNametxt);
            this.Controls.Add(this.BT_Login);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_Login;
        private System.Windows.Forms.TextBox Passwordtxt;
        private System.Windows.Forms.TextBox UserNametxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BTexit;
    }
}