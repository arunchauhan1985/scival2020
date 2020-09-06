using System;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class RemarkStop : BaseForm
    {
        ErrorLog oErrorLog = new ErrorLog();

        public RemarkStop()
        {
            InitializeComponent();
            loadInitailValue();
        }

        private void loadInitailValue()
        {
            if (SharedObjects.PageIds == 8)
            {
                this.Text = "Stop";
                lblremark.Text = "Your task is completed now.\nIt will forwarded to next step.";
            }
            else if (SharedObjects.PageIds == 10)
            {
                this.Text = "Stop-->New";
                String ext = "new Opportunity.";

                if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                {
                    this.Text = "Stop-->Next";
                    ext = "next Opportunity for quality check";
                }

                lblremark.Text = "Your task is completed now.\nIt will forwarded to next step.\nYou will continue with " + ext + ".";
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
                if (SharedObjects.PageIds == 10)
                {
                    if (SharedObjects.TaskId == 2 && SharedObjects.Cycle == 0)
                    {
                        try
                        {
                            string remarkText = rchTextRemark.Text.Trim();
                            Int64 userId = Convert.ToInt64(SharedObjects.User.USERID);
                            Int64 WFId = SharedObjects.WorkId;
                            Int64 TransId = SharedObjects.TransactionId;
                            DataSet dsResult = AwardDataOperations.TimeSheetStopContinueForQC(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), remarkText);

                            if (Convert.ToString(dsResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                if (dsResult.Tables["Result"].Rows.Count > 0)
                                {
                                    SharedObjects.WorkId = Convert.ToInt64(dsResult.Tables["Result"].Rows[0]["WORKFLOWID"]);
                                    this.Dispose();
                                }
                                else
                                {
                                    SharedObjects.TaskBoard = null;
                                    Application.OpenForms["Opportunity"].Dispose();
                                    TaskBoard taskobj = new TaskBoard();
                                    taskobj.Show();
                                    this.Dispose();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            oErrorLog.WriteErrorLog(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            string remarkText = rchTextRemark.Text.Trim();
                            Int64 userId = Convert.ToInt64(SharedObjects.User.USERID);
                            Int64 WFId = SharedObjects.WorkId;
                            Int64 TransId = SharedObjects.TransactionId;

                            DataSet dsResult = AwardDataOperations.TimeSheetStopContinue(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), remarkText);

                            if (Convert.ToString(dsResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                            {
                                SharedObjects.WorkId = Convert.ToInt64(dsResult.Tables["ERRORCODE"].Rows[0]["WFID"]);
                                this.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            oErrorLog.WriteErrorLog(ex);
                        }
                    }
                }
                else
                {
                    try
                    {
                        string remarkText = rchTextRemark.Text.Trim();
                        Int64 userId = Convert.ToInt64(SharedObjects.User.USERID);
                        Int64 WFId = SharedObjects.WorkId;
                        Int64 TransId = SharedObjects.TransactionId;

                        DataSet dsResult = AwardDataOperations.TimeSheetStop(WFId, userId, TransId, Convert.ToInt64(SharedObjects.PageIds), remarkText);

                        if (Convert.ToString(dsResult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            if (SharedObjects.PageIds == 8 || SharedObjects.PageIds == 10)
                            {
                                SharedObjects.TaskBoard = null;
                                Application.OpenForms["Opportunity"].Dispose();

                                TaskBoard taskobj = new TaskBoard();
                                taskobj.Show();

                                this.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        oErrorLog.WriteErrorLog(ex);
                    }
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void rchTextRemark_TextChanged(object sender, EventArgs e) { }
    }
}
