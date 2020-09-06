using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.FundingBody
{
    public partial class Remark_Pause_Logoff : BaseForm
    {        
        ErrorLog oErrorLog = new ErrorLog();
        public Remark_Pause_Logoff()
        {
            InitializeComponent();
            loadInitailValue();
        }

        private void loadInitailValue()
        {
            if (SharedObjects.PageIds == 6)
            {
                this.Text = "Pause & Logoff";
                lblremark.Text = "You are leaving this task. \nThis task can be allocated to other user or you will get the same task on next login.";
            }
            else if (SharedObjects.PageIds == 10)
            {
                this.Text = "Exit";
                lblremark.Text = "You have not made any changes/modifications \nso it will not be forwarded to QA.";
            }
        }
        private void btnsubmit_Click(object sender, EventArgs e)
        {
            if (rchTextRemark.Text == "" || rchTextRemark.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the Remark.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    string remarkText = rchTextRemark.Text.Trim();
                    Int64 userId = Convert.ToInt64(SharedObjects.User.USERID);
                    Int64 WFId = SharedObjects.WorkId;
                    Int64 TransId = SharedObjects.TransactionId;

                    DataSet dsResult = DashboardDataOperations.TimeSheetStop(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), remarkText);

                    if (SharedObjects.PageIds == 10)
                    {
                        SharedObjects.TaskBoard = null;
                        Application.OpenForms["FundingBody"].Dispose();

                        TaskBoard taskobj = new TaskBoard();
                        taskobj.Show();
                        this.Dispose();
                    }
                    else if (SharedObjects.PageIds == 6)
                    {
                        this.Dispose();
                        Application.OpenForms["FundingBody"].Dispose();
                        //Application.Restart();
                        Application.OpenForms["Login"].Show();
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
