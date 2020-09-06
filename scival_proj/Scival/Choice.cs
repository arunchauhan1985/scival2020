using System;
using System.Windows.Forms;

namespace Scival
{
    public partial class Choice : BaseForm
    {
        public Choice()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (rbTaskboard.Checked)
            {
                TaskBoard TB = new TaskBoard();
                TB.Show();
            }
            else if (rbWebWatcher.Checked)
            {
                Scival.WebWatcher.DashBoard TB = new Scival.WebWatcher.DashBoard();
                TB.Show();
            }

            this.Dispose();
        }

        private void Choice_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
