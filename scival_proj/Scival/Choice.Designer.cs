namespace Scival
{
    partial class Choice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Choice));
            this.label1 = new System.Windows.Forms.Label();
            this.rbTaskboard = new System.Windows.Forms.RadioButton();
            this.rbWebWatcher = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select One of the given option.";
            // 
            // rbTaskboard
            // 
            this.rbTaskboard.AutoSize = true;
            this.rbTaskboard.Checked = true;
            this.rbTaskboard.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbTaskboard.Location = new System.Drawing.Point(39, 63);
            this.rbTaskboard.Name = "rbTaskboard";
            this.rbTaskboard.Size = new System.Drawing.Size(86, 18);
            this.rbTaskboard.TabIndex = 1;
            this.rbTaskboard.TabStop = true;
            this.rbTaskboard.Text = "Task Board";
            this.rbTaskboard.UseVisualStyleBackColor = true;
            // 
            // rbWebWatcher
            // 
            this.rbWebWatcher.AutoSize = true;
            this.rbWebWatcher.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbWebWatcher.Location = new System.Drawing.Point(143, 63);
            this.rbWebWatcher.Name = "rbWebWatcher";
            this.rbWebWatcher.Size = new System.Drawing.Size(98, 18);
            this.rbWebWatcher.TabIndex = 2;
            this.rbWebWatcher.Text = "Web-Watcher";
            this.rbWebWatcher.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Scival.Properties.Resources.button1_BackgroundImage;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(100, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Go";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Choice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(285, 150);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rbWebWatcher);
            this.Controls.Add(this.rbTaskboard);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Choice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choice";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Choice_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbTaskboard;
        private System.Windows.Forms.RadioButton rbWebWatcher;
        private System.Windows.Forms.Button button1;
    }
}