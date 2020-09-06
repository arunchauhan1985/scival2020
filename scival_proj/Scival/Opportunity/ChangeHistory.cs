using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class ChangeHistory : UserControl
    {
        List<change> changes;

        Opportunity opportunity;
        Replace replace = new Replace();
        ErrorLog errorLog = new ErrorLog();

        int rowIndex = 0;
        static int gridRowId = 0;
        static string changeType = "";
        static string oppStatus = "";

        public ChangeHistory(Opportunity opp)
        {
            InitializeComponent();
            opportunity = opp;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";
        }

        private void LoadInitialValue()
        {
            try
            {
                lblMsg.Visible = false;
                BindGrid();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        public void BindGrid()
        {
            changes = OpportunityDataOperations.GetChangeHistory(SharedObjects.WorkId);

            oppStatus = OpportunityDataOperations.GetOpportunityStatus(SharedObjects.WorkId);

            if (changes.Count > 0)
            {
                foreach (change change in changes)
                {
                    string UpdateFunding_CHANGE_TEXT = replace.Return_WieredChar_Original(change.CHANGE_TEXT);

                    if (UpdateFunding_CHANGE_TEXT != "")
                        change.CHANGE_TEXT = UpdateFunding_CHANGE_TEXT;
                }

                grdChangeHistory.AutoGenerateColumns = false;
                grdChangeHistory.DataSource = changes;
                BtnAdd.Visible = false;
                btnUpdate.Visible = true;
            }
            else
            {
                BtnAdd.Visible = true;
                btnUpdate.Visible = false;
                norecord();
            }
        }

        private void norecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("change_text");
                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdChangeHistory.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string url = txtChangeText.Text.TrimStart().TrimEnd();

            if (url.Contains("http://") || url.Contains("https://") || url.Contains("www."))
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (txtChangeText.Text != "")
                {
                    string _result = errorLog.htlmtag(txtChangeText.Text.Trim(), "Change Description");

                    if (!_result.Equals(""))
                    {
                        MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (txtPostDate.Text == "")
                {
                    MessageBox.Show("Post Date can not be blank.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtChangeText.Text == "")
                {
                    MessageBox.Show("Change Text can not be blank.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                OpportunityDataOperations.SaveAndUpdateChangeHistory(SharedObjects.WorkId, null, oppStatus, Convert.ToDateTime(txtPostDate.Text), replace.EntityToUnicode(txtChangeText.Text.TrimStart().TrimEnd()), 0, 0);

                txtPostDate.Text = "";
                txtChangeText.Text = "";

                if (SharedObjects.TRAN_TYPE_ID == 1)
                    opportunity.GetProcess_update("Changehistory");
                else
                    opportunity.GetProcess();

                BindGrid();

                BtnAdd.Visible = false;
                btnUpdate.Visible = true;

                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            txtPostDate.Text = SharedObjects.CurrentUrl.ToString();
        }

        private void grdAbout_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (changes.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowIndex = e.RowIndex;

                        try
                        {
                            txtPostDate.Text = Convert.ToString(changes[rowIndex].POSTDATE.Value);
                            txtChangeText.Text = changes[rowIndex].CHANGE_TEXT;
                            gridRowId = Convert.ToInt32(changes[rowIndex].CHANGE_ID);
                            changeType = changes[rowIndex].TYPE;
                            BtnAdd.Visible = false;
                            btnUpdate.Visible = true;
                            btnCancel.Visible = true;
                        }
                        catch { }
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for edit.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void grdAbout_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (changes.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            OpportunityDataOperations.SaveAndUpdateChangeHistory(SharedObjects.WorkId, null, "new", null, "", 3, changes[grdChangeHistory.SelectedCells[0].RowIndex].CHANGE_ID);
                            BindGrid();
                            MessageBox.Show("Record inserted successfully.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        opportunity.GetProcess();
                        MessageBox.Show("Please add Post Date.....!!!!", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for delete.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url = txtChangeText.Text.TrimStart().TrimEnd();

            if (url.Contains("http://") || url.Contains("https://") || url.Contains("www."))
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (txtChangeText.Text != "")
                {
                    string _result = errorLog.htlmtag(txtChangeText.Text.Trim(), "Change Description");

                    if (!_result.Equals(""))
                    {
                        MessageBox.Show(_result, "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (txtPostDate.Text == "")
                {
                    MessageBox.Show("Post Date can not be blank.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtChangeText.Text == "")
                {
                    MessageBox.Show("Change Text can not be blank.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                OpportunityDataOperations.SaveAndUpdateChangeHistory(SharedObjects.WorkId, null, changeType, Convert.ToDateTime(txtPostDate.Text), replace.EntityToUnicode(txtChangeText.Text.TrimStart().TrimEnd()), 2, gridRowId);

                txtPostDate.Text = "";
                txtChangeText.Text = "";

                if (SharedObjects.TRAN_TYPE_ID == 1)
                    opportunity.GetProcess_update("Changehistory");
                else
                    opportunity.GetProcess();

                BindGrid();

                BtnAdd.Visible = false;
                btnUpdate.Visible = true;

                lblMsg.Visible = true;
                lblMsg.Text = "OpportunityDataOperations";

                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            BindGrid();
            txtPostDate.Text = "";
            txtChangeText.Text = "";
            opportunity.GetProcess();
        }

        private void txtPostDate_Cal_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtPostDate.Text = txtPostDate_Cal.Text.Trim();
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }
    }
}
