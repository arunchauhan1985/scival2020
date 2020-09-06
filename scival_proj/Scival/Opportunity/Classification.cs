using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySqlDal;

namespace Scival.Opportunity
{
    public partial class Classification : UserControl
    {
        Opportunity opportunity;
        ErrorLog errorLog = new ErrorLog();

        List<ClassificationList> classificationLists;
        List<sci_classificationstypetype> classificationsType;
        List<sci_asjcdescription> asjcDescriptions;
        List<CustomAsjcdescription> subClassifications;

        int rowIndex = 0;
        bool flagUpdate = false;

        public Classification(Opportunity opp)
        {
            InitializeComponent();
            opportunity = opp;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(opportunity);
            pnlURL.Controls.Add(objPage);
        }

        void ddlASJC_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlType_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        void ddlASJC1_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void LoadInitialValue()
        {
            try
            {
                lblMsg.Visible = false;

                classificationLists = OpportunityDataOperations.GetClassificationLists(SharedObjects.WorkId);

                if (classificationLists.Count > 0)
                {
                    grdClass.AutoGenerateColumns = false;
                    grdClass.DataSource = classificationLists;
                }
                else
                {
                    NoRecord();
                }

                classificationsType = OpportunityDataOperations.GetClassificationsTypes();
                ddlType.DataSource = classificationsType;
                ddlType.DisplayMember = "VALUE";
                ddlType.ValueMember = "VALUE";

                asjcDescriptions = OpportunityDataOperations.GetAsjcDescriptionsAsDetail();

                sci_asjcdescription sci = new sci_asjcdescription();
                sci.CODE = "SelectASJC";
                sci.DESCRIPTION = "--Select Classification--";

                List<sci_asjcdescription> sci_Asjcdescriptions = new List<sci_asjcdescription>();
                sci_Asjcdescriptions.Add(sci);
                sci_Asjcdescriptions.AddRange(asjcDescriptions);

                ddlASJC.DataSource = sci_Asjcdescriptions;
                ddlASJC.DisplayMember = "DESCRIPTION";
                ddlASJC.ValueMember = "CODE";

                subClassifications = OpportunityDataOperations.GetCustomAsjcDescriptions();

                if (subClassifications.Count > 0)
                {
                    dgvSubLevel.AutoGenerateColumns = false;
                    dgvSubLevel.DataSource = subClassifications;
                }
                else
                {
                    NoRecordSubType();
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void NoRecord()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("TYPE");
                dtNoRcrd.Columns.Add("FREQUENCY");
                dtNoRcrd.Columns.Add("CLASSIFICATION_TEXT");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                grdClass.AutoGenerateColumns = false;
                grdClass.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void NoRecordSubType()
        {
            try
            {
                DataTable dtNoRcrd = new DataTable();
                dtNoRcrd.Columns.Add("Code");
                dtNoRcrd.Columns.Add("CLASSIFICATION");
                dtNoRcrd.Columns.Add("SUB LEVEL");
                dtNoRcrd.Columns.Add("Action");

                DataRow dr = dtNoRcrd.NewRow();
                dr[0] = "No Record(s) found.";

                dtNoRcrd.Rows.Add(dr);

                dgvSubLevel.AutoGenerateColumns = false;
                dgvSubLevel.DataSource = dtNoRcrd;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string url = txtFreqncy.Text.TrimStart().TrimEnd();

            if (url.Contains("http://") || url.Contains("https://") || url.Contains("www."))
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (ddlType.SelectedValue.ToString() != "ASJC")
                        MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (txtFreqncy.Text == "")
                        MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                        MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (ddlASJC.SelectedValue.ToString() == "SelectASJC")
                        MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        string classificationType = string.Empty;
                        string[] ddlASJCVal = new string[1];
                        string[] ddlASJCText = new string[1];

                        if (ddlType.SelectedValue.ToString() == "SelectType")
                            classificationType = "";
                        else
                            classificationType = ddlType.SelectedValue.ToString();

                        if (ddlASJC.SelectedValue.ToString() == "SelectASJC")
                        {
                            ddlASJCVal = new string[1];
                            ddlASJCVal[0] = "";

                            ddlASJCText = new string[1];
                            ddlASJCText[0] = "";
                        }
                        else
                        {
                            ddlASJCVal = new string[ddlASJC.SelectedItems.Count];
                            ddlASJCText = new string[ddlASJC.SelectedItems.Count];

                            for (int i = 0; i < ddlASJC.SelectedItems.Count; i++)
                            {
                                ddlASJCVal[i] = ((System.Data.DataRowView)(ddlASJC.SelectedItems[i])).Row.ItemArray[0].ToString();
                                ddlASJCText[i] = ((System.Data.DataRowView)(ddlASJC.SelectedItems[i])).Row.ItemArray[2].ToString();
                            }
                        }

                        classificationLists = OpportunityDataOperations.SaveClassification(SharedObjects.WorkId, classificationType, Convert.ToInt64(txtFreqncy.Text.Trim()), ddlASJCVal, ddlASJCText);

                        if (classificationLists.Count > 0)
                        {
                            grdClass.AutoGenerateColumns = false;
                            grdClass.DataSource = classificationLists;
                            txtFreqncy.Text = "1";
                            ddlASJC.ClearSelected();
                            ddlASJC.SelectedIndex = 0;
                        }
                        else
                        {
                            NoRecord();
                        }

                        if (SharedObjects.TRAN_TYPE_ID == 1)
                            opportunity.GetProcess_update("Classification Group");
                        else
                            opportunity.GetProcess();

                        lblMsg.Visible = true;
                        lblMsg.Text = "Record inserted successfully.";

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    }
                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                }
            }
        }

        private void grdClass_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (classificationLists.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowIndex = e.RowIndex;

                        try
                        {
                            ddlType.SelectedValue = classificationLists[rowIndex].TYPE;
                            txtFreqncy.Text = Convert.ToString(classificationLists[rowIndex].FREQUENCY);
                            ddlASJC.SelectionMode = SelectionMode.One;
                            ddlASJC.ClearSelected();
                            ddlASJC.SelectedValue = classificationLists[rowIndex].CODE;

                            btnSave.Visible = false;
                            btnAddurl.Visible = false;

                            btnupdate.Visible = true;
                            btncancel.Visible = true;

                            flagUpdate = true;
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

        private void grdClass_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;

                if (classificationLists.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var classification = classificationLists[grdClass.SelectedCells[0].RowIndex];

                            classificationLists = OpportunityDataOperations.DeleteAndUpdateClassification(SharedObjects.WorkId, 1, classification.TYPE, classification.FREQUENCY,
                                classification.CODE, classification.CLASSIFICATION_TEXT, classification.CLASSIFICATIONS_ID.ToString());

                            if (classificationLists.Count > 0)
                            {
                                grdClass.AutoGenerateColumns = false;
                                grdClass.DataSource = classificationLists;
                            }
                            else
                            {
                                NoRecord();
                            }

                            opportunity.GetProcess();

                            lblMsg.Visible = true;
                            lblMsg.Text = "Record Deleted Successfully";
                        }
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

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string url = txtFreqncy.Text.TrimStart().TrimEnd();

            if (url.Contains("http://") || url.Contains("https://") || url.Contains("www."))
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (ddlType.SelectedValue.ToString() != "ASJC")
                        MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (txtFreqncy.Text == "")
                        MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                        MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (ddlASJC.SelectedValue.ToString() == "SelectASJC")
                        MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        Int64 frequency = Convert.ToInt64(txtFreqncy.Text.TrimStart().TrimEnd());
                        string type = ddlType.SelectedValue.ToString();

                        string code = string.Empty;
                        string ClassText = string.Empty;

                        if (Convert.ToString(ddlASJC.SelectedValue) != "SelectASJC")
                        {
                            code = ddlASJC.SelectedValue.ToString();
                            ClassText = ddlASJC.Text.Trim();
                        }

                        classificationLists = OpportunityDataOperations.DeleteAndUpdateClassification(SharedObjects.WorkId, 2, type, frequency, code, ClassText, 
                            Convert.ToString(classificationLists[rowIndex].CLASSIFICATIONS_ID), classificationLists[rowIndex].FREQUENCY,
                            classificationLists[rowIndex].CODE);

                        if (classificationLists.Count > 0)
                        {
                            grdClass.AutoGenerateColumns = false;
                            grdClass.DataSource = classificationLists;
                        }
                        else
                        {
                            NoRecord();
                        }

                        btnSave.Visible = true;
                        btnAddurl.Visible = true;
                        btnupdate.Visible = false;
                        btncancel.Visible = false;
                        ddlASJC.SelectionMode = SelectionMode.MultiExtended;
                        ddlASJC.ClearSelected();

                        txtFreqncy.Text = "1";
                        ddlASJC.SelectedIndex = 0;
                        flagUpdate = false;

                        if (SharedObjects.TRAN_TYPE_ID == 1)
                            opportunity.GetProcess_update("Classification Group");
                        else
                            opportunity.GetProcess();

                        lblMsg.Visible = true;
                        lblMsg.Text = "Record Updated Successfully";

                        OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                    }
                }
                catch (Exception ex)
                {
                    errorLog.WriteErrorLog(ex);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                txtFreqncy.Text = "1";
                ddlASJC.ClearSelected();
                ddlASJC.SelectedIndex = 0;

                btnSave.Visible = true;
                btnAddurl.Visible = true;

                btnupdate.Visible = false;
                btncancel.Visible = false;
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void btnAddurl_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            SharedObjects.DefaultLoad = "loadValue";

            PageURL objPage = new PageURL(opportunity);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            PageURL objPage1 = new PageURL(opportunity);
            pnlURL.Controls.Add(objPage);
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string outputInfo = "";
                string[] keyWords = txtSearch.Text.Split(' ');

                foreach (string word in keyWords)
                {
                    string matchstr = string.Empty;
                    if (outputInfo.Length == 0)
                    {
                        matchstr = word.Replace("%", "[%]");
                        outputInfo = "(sub_level_description LIKE '%" + matchstr + "%')";
                    }
                    else
                    {
                        matchstr = word.Replace("%", "[%]");
                        outputInfo += " AND (sub_level_description LIKE '%" + matchstr + "%')";
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }

        private void dgvSubLevel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    int cloCode = Convert.ToInt32(dgvSubLevel.Rows[e.RowIndex].Cells["Code"].Value.ToString());
                    string SublavelVAl = Convert.ToString(dgvSubLevel.Rows[e.RowIndex].Cells["sub_level_description"].Value.ToString());

                    ddlASJC.ClearSelected();
                    ddlASJC.SelectedValue = cloCode;
                    dgvSubLevel.Refresh();

                    if (flagUpdate == false)
                    {
                        // Save classification
                        lblMsg.Visible = false;
                        Regex intRgx = new Regex(@"^[0-9]+");

                        if (ddlType.SelectedValue.ToString() != "ASJC")
                            MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (txtFreqncy.Text == "")
                            MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                            MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (cloCode == 0)
                            MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (SublavelVAl.ToString() == "")
                            MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            string ddlTryp = string.Empty;
                            string[] ddlASJCVal = new string[1];
                            string[] ddlASJCText = new string[1];

                            if (ddlType.SelectedValue.ToString() == "SelectType")
                                ddlTryp = "";
                            else
                                ddlTryp = ddlType.SelectedValue.ToString();

                            ddlASJCVal[0] = Convert.ToString(cloCode);
                            ddlASJCText[0] = SublavelVAl;

                            classificationLists = OpportunityDataOperations.SaveClassification(SharedObjects.WorkId, ddlTryp, Convert.ToInt64(txtFreqncy.Text.Trim()), ddlASJCVal, ddlASJCText);

                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());

                            if (classificationLists.Count > 0)
                            {
                                grdClass.AutoGenerateColumns = false;
                                grdClass.DataSource = classificationLists;
                                txtFreqncy.Text = "1";
                                ddlASJC.ClearSelected();
                                ddlASJC.SelectedIndex = 0;
                            }
                            else
                            {
                                NoRecord();
                            }

                            opportunity.GetProcess();

                            lblMsg.Visible = true;
                            lblMsg.Text = "Record Saved Successfully";
                        }
                    }
                    else
                    {
                        // Update classification
                        lblMsg.Visible = false;
                        Regex intRgx = new Regex(@"^[0-9]+");

                        if (ddlType.SelectedValue.ToString() != "ASJC")
                            MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (txtFreqncy.Text == "")
                            MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                            MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (ddlASJC.SelectedValue.ToString() == "SelectASJC")
                            MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            Int64 frequency = Convert.ToInt64(txtFreqncy.Text);
                            string type = ddlType.SelectedValue.ToString();

                            string code = string.Empty;
                            string ClassText = string.Empty;

                            if (Convert.ToString(ddlASJC.SelectedValue) != "SelectASJC")
                            {
                                code = ddlASJC.SelectedValue.ToString();
                                ClassText = ddlASJC.Text.Trim();
                            }

                            classificationLists = OpportunityDataOperations.DeleteAndUpdateClassification(SharedObjects.WorkId, 2, type, frequency, code, ClassText, 
                                Convert.ToString(classificationLists[rowIndex].CLASSIFICATIONS_ID), classificationLists[rowIndex].FREQUENCY,
                                classificationLists[rowIndex].CODE);

                            if (classificationLists.Count > 0)
                            {
                                grdClass.AutoGenerateColumns = false;
                                grdClass.DataSource = classificationLists;
                            }
                            else
                            {
                                NoRecord();
                            }

                            btnSave.Visible = true;
                            btnAddurl.Visible = true;
                            btnupdate.Visible = false;
                            btncancel.Visible = false;
                            ddlASJC.SelectionMode = SelectionMode.MultiExtended;
                            ddlASJC.ClearSelected();

                            txtFreqncy.Text = "1";
                            ddlASJC.SelectedIndex = 0;
                            flagUpdate = false;

                            lblMsg.Visible = true;
                            lblMsg.Text = "Record Updated Successfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex);
            }
        }
    }
}
