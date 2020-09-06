using System;
using System.Windows.Forms;
using MySqlDal;

namespace Scival
{
    public partial class Login : BaseForm
    {
        ErrorLog oErrorLog = new ErrorLog();

        public Login()
        {
            InitializeComponent();

            UserNametxt.Focus();
        }

        private void BTexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BT_Login_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(UserNametxt.Text))
                {
                    MessageBox.Show("Please enter User Name.");
                    return;
                }

                if (string.IsNullOrEmpty(Passwordtxt.Text))
                {
                    MessageBox.Show("Please enter Password");
                    return;
                }

                string validationMessage = string.Empty;
                sci_usermaster user = null;
                CommonDataOperation.ValidateLogin(UserNametxt.Text, Passwordtxt.Text, out validationMessage, out user);

                if (user == null)
                {
                    MessageBox.Show(validationMessage);
                    UserNametxt.Text = string.Empty;
                    Passwordtxt.Text = string.Empty;
                    return;
                }

                SharedObjects.User = user;

                CommonDataOperation.UpdateExpireAlert();

                //var userTasksCountForAdmin = TaskDataOperation.GetModuleWiseUserTasksCountByModuleName(user.USERID, user.NAME, "Admin");

                //if (userTasksCountForAdmin > 0)
                //{
                //    MessageBox.Show("You have admin privileges. \nPlease use following URL to login : \nhttp://scival.aptaracorp.com", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Application.Exit();
                //}

                //var userTasksCountForExpiry = TaskDataOperation.GetModuleWiseUserTasksCountByModuleName(user.USERID, user.NAME, "Expiry");

                //if (userTasksCountForExpiry > 0)
                //    SharedObjects.ExpireAlertCount = CommonDataOperation.GetExpireAlertsCount();

                bool checkWebWatcher = false;

                var userTasksCountForWebWatcher = TaskDataOperation.GetModuleWiseUserTasksCountByModuleName(user.USERID, user.NAME, "Web Watcher");

                if (userTasksCountForWebWatcher > 0)
                    checkWebWatcher = true;

                UserNametxt.Text = "";
                Passwordtxt.Text = "";

                if (checkWebWatcher)
                {
                    var userTasksCount = TaskDataOperation.GetModuleWiseUserTasksCount(user.USERID, user.NAME);

                    if (userTasksCount > 1)
                    {
                        Choice choice = new Choice();
                        choice.Show();
                    }
                    else
                    {
                        Scival.WebWatcher.DashBoard dashBoard = new Scival.WebWatcher.DashBoard();
                        dashBoard.Show();
                    }
                }
                else
                {
                    TaskBoard taskBoard = new TaskBoard();
                    taskBoard.Show();
                }

                this.Hide();

            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
