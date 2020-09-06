using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySqlDal;

namespace Scival.Award
{
    public partial class Classification : UserControl
    {
        private Awards m_parent;
        Int64 WfId = 0;
        int rowindex = 0;
        DataTable tempClassi = new DataTable();
        DataTable tempDDLVal = new DataTable();
        ErrorLog oErrorLog = new ErrorLog();
        DataView dvSubClassiFication;
        bool flagUpdate = false;
        public Classification(Awards frm)
        {
            InitializeComponent();
            m_parent = frm;
            LoadInitialValue();

            SharedObjects.DefaultLoad = "";

            PageURL objPage = new PageURL(frm);
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
                txtFreqncy.Focus();
                WfId = Convert.ToInt64(SharedObjects.WorkId);

                DataSet dsClassiFication = AwardDataOperations.GetClassiFicatilList(WfId);
                
                    DataTable DT = dsClassiFication.Tables["DisplayData"];
                    tempClassi = dsClassiFication.Tables["DisplayData"];
                    if (DT.Rows.Count > 0)
                    {
                        grdClass.AutoGenerateColumns = false;
                        grdClass.DataSource = DT;
                    }
                    else
                    {
                        //norecord();
                    }

                    DataTable DT2 = dsClassiFication.Tables["Type"];
                    tempDDLVal = dsClassiFication.Tables["Type"];
                    ddlType.DataSource = DT2;
                    ddlType.DisplayMember = "VALUE";
                    ddlType.ValueMember = "VALUE";


                    DT2 = dsClassiFication.Tables["ASJCDesc"];

                    DataRow dr = DT2.NewRow();
                    dr["CODE"] = "SelectASJC";
                    dr["DESCRIPTION"] = "--Select Classification--";
                    DT2.Rows.InsertAt(dr, 0);

                    ddlASJC.DataSource = DT2;
                    ddlASJC.DisplayMember = "DESCRIPTION";
                    ddlASJC.ValueMember = "CODE";

                    DataSet dsSubClassiFication = AwardDataOperations.GetSubASJCTypeList();
                    dvSubClassiFication = new DataView(dsSubClassiFication.Tables["SubASJCType"].Copy());
                    if (dvSubClassiFication.Count > 0)
                    {
                        dgvSubLevel.AutoGenerateColumns = false;
                        dgvSubLevel.DataSource = dvSubClassiFication;
                    }
                    else
                    {
                        norecordSubType();
                    }
                
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void norecord()
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
                oErrorLog.WriteErrorLog(ex);
            }
        }
        private void norecordSubType()
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
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //pankaj 11 june
            string url = txtFreqncy.Text.TrimStart().TrimEnd();
            if (url.Contains("http://") || url.Contains("https://") || url.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                try
                {
                    DataSet dsresult = new DataSet();
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (ddlType.SelectedValue.ToString() != "ASJC" && ddlType.SelectedValue.ToString().ToLower() != "orgspecific")
                    {
                        MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtFreqncy.Text == "")
                    {
                        MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                    {
                        MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlASJC.SelectedValue.ToString() == "SelectASJC" && ddlType.SelectedValue.ToString().ToLower() != "orgspecific")
                    {
                        MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (1 == 1 && ddlType.SelectedValue.ToString().ToLower() != "orgspecific")
                    {
                        MessageBox.Show("Please Select Classification from grid.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {

                        string ddlTryp = string.Empty;
                        string[] ddlASJCVal = new string[1];
                        string[] ddlASJCText = new string[1];
                        if (ddlType.SelectedValue.ToString() == "SelectType")
                            ddlTryp = "";
                        else
                            ddlTryp = ddlType.SelectedValue.ToString();

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
                                ddlASJCText[i] = ((System.Data.DataRowView)(ddlASJC.SelectedItems[i])).Row.ItemArray[1].ToString();

                            }
                        }

                        if (ddlType.SelectedValue.ToString().ToLower() == "orgspecific")
                        {
                            if (txtClassification.Text.Trim() == "" || txtorg_code.Text.Trim() == "")
                            {
                                MessageBox.Show("Please Select Classification from grid.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                string[] ddlASJCVal_org = new string[1];
                                string[] ddlASJCText_org = new string[1];
                                ddlASJCVal_org[0] = txtorg_code.Text.Trim();
                                ddlASJCText_org[0] = txtClassification.Text.Trim();
                                dsresult = AwardDataOperations.SaveClassiFication(WfId, ddlTryp, Convert.ToInt64(txtFreqncy.Text.Trim()), ddlASJCVal_org, ddlASJCText_org);
                            }
                        }

                        else
                        {

                            dsresult = AwardDataOperations.SaveClassiFication(WfId, ddlTryp, Convert.ToInt64(txtFreqncy.Text.Trim()), ddlASJCVal, ddlASJCText);
                        }

                        if (Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][0]) == "0")
                        {
                            if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                            {
                                tempClassi = dsresult.Tables["DisplayData"];
                                grdClass.AutoGenerateColumns = false;
                                grdClass.DataSource = dsresult.Tables["DisplayData"];
                                txtFreqncy.Text = "1";
                                ddlASJC.ClearSelected();
                                ddlASJC.SelectedIndex = 0;
                            }
                            else
                            {
                                norecord();
                            }
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Classification Group");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
                            lblMsg.Visible = true;
                            lblMsg.Text = Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]);

                            //Pankaj start track TrackUnstoppedAward
                            if (dsresult.Tables["ERRORCODE"].Rows[0][0].ToString() == "0")
                            {
                                OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                            }
                            //End track TrackUnstoppedAward
                        }
                        else
                        {
                            MessageBox.Show(Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                }
                catch (Exception ex)
                {
                    oErrorLog.WriteErrorLog(ex);
                }
            }
        }

        private void grdClass_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (tempClassi.Rows.Count > 0)
                {
                    if (e.RowIndex > -1)
                    {
                        rowindex = e.RowIndex;

                        try
                        {

                            ddlType.SelectedValue = Convert.ToString(tempClassi.Rows[rowindex]["type"]);
                            txtFreqncy.Text = Convert.ToString(tempClassi.Rows[rowindex]["FREQUENCY"]);
                            ddlASJC.SelectionMode = SelectionMode.One;
                            ddlASJC.ClearSelected();
                            ddlASJC.SelectedValue = Convert.ToString(tempClassi.Rows[rowindex]["CODE"]);

                            btnSave.Visible = false;
                            btnAddurl.Visible = false;

                            btnupdate.Visible = true;
                            btncancel.Visible = true;
                            flagUpdate = true;
                        }
                        catch
                        {

                        }
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for edit.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void grdClass_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (tempClassi.Rows.Count > 0)
                {
                    if (e.KeyValue == 46)
                    {
                        if (MessageBox.Show("Do you really  want to delete this record ?", "Scival", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DataSet dsresult = AwardDataOperations.DeleteClassification(SharedObjects.WorkId, 1, Convert.ToString(tempClassi.Rows[grdClass.SelectedCells[0].RowIndex]["TYPE"]), Convert.ToInt64(tempClassi.Rows[grdClass.SelectedCells[0].RowIndex]["FREQUENCY"]), Convert.ToString(tempClassi.Rows[grdClass.SelectedCells[0].RowIndex]["CODE"]), Convert.ToString(tempClassi.Rows[grdClass.SelectedCells[0].RowIndex]["CLASSIFICATION_TEXT"]), Convert.ToString(tempClassi.Rows[grdClass.SelectedCells[0].RowIndex]["CLASSIFICATIONS_ID"]));

                            tempClassi = dsresult.Tables["DisplayData"];
                                if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                                {
                                    grdClass.AutoGenerateColumns = false;
                                    grdClass.DataSource = dsresult.Tables["DisplayData"];
                                }
                                else
                                {
                                    norecord();

                                }

                                m_parent.GetProcess();
                            lblMsg.Visible = true;
                            //lblMsg.Text = Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]);
                        }
                    }
                }
                else
                    MessageBox.Show("There is no record(s) for delete.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {

            //pankaj 11 june
            string url = txtFreqncy.Text.TrimStart().TrimEnd();
            if (url.Contains("http://") || url.Contains("https://") || url.Contains("www."))
            {
                MessageBox.Show("URL is available in text box.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                try
                {
                    lblMsg.Visible = false;
                    Regex intRgx = new Regex(@"^[0-9]+");

                    if (ddlType.SelectedValue.ToString() != "ASJC")
                    {
                        MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtFreqncy.Text == "")
                    {
                        MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                    {
                        MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (ddlASJC.SelectedValue.ToString() == "SelectASJC")
                    {
                        MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

                        DataSet dsresult = AwardDataOperations.UpdateClassification(WfId, 2, type, Convert.ToInt64(Convert.ToString(tempClassi.Rows[rowindex]["FREQUENCY"]).TrimStart().TrimEnd()), Convert.ToString(Convert.ToString(tempClassi.Rows[rowindex]["CODE"])), frequency, code, ClassText, Convert.ToString(Convert.ToString(tempClassi.Rows[rowindex]["CLASSIFICATIONS_ID"])));//dalobj.DeleteClassification(WfId, 2, type, frequency, code, ClassText, HF.Value);

                            tempClassi = dsresult.Tables["DisplayData"];
                            if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                            {
                                grdClass.AutoGenerateColumns = false;
                                grdClass.DataSource = dsresult.Tables["DisplayData"];
                            }
                            else
                            {
                                norecord();
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
                            #region For Changing Colour in case of Update
                            if (SharedObjects.TRAN_TYPE_ID == 1)
                            {
                                m_parent.GetProcess("Classification Group");
                            }
                            else
                            {
                                m_parent.GetProcess();
                            }
                            #endregion
                        
                        //Pankaj start track TrackUnstoppedAward
                       
                            OpportunityDataOperations.TrackUnstoppedFbAwardOpp(SharedObjects.WorkId.ToString(), SharedObjects.User.USERID.ToString(), SharedObjects.ClickPage.ToString());
                       
                        lblMsg.Visible = true;
                        lblMsg.Text = Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]);


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
                oErrorLog.WriteErrorLog(ex);
            }

        }

        private void btnAddurl_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            SharedObjects.DefaultLoad = "loadValue";

            PageURL objPage = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);

            SharedObjects.DefaultLoad = "";
            pnlURL.Controls.Clear();
            PageURL objPage1 = new PageURL(m_parent);
            pnlURL.Controls.Add(objPage);
        }

        private void ddlASJC_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
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
                        {
                            MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtFreqncy.Text == "")
                        {
                            MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                        {
                            MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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

                            DataSet dsresult = AwardDataOperations.SaveClassiFication(WfId, ddlTryp, Convert.ToInt64(txtFreqncy.Text.Trim()), ddlASJCVal, ddlASJCText);

                                if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                                {
                                    tempClassi = dsresult.Tables["DisplayData"];

                                    grdClass.AutoGenerateColumns = false;
                                    grdClass.DataSource = dsresult.Tables["DisplayData"];
                                    txtFreqncy.Text = "1";
                                    ddlASJC.ClearSelected();
                                    ddlASJC.SelectedIndex = 0;
                                }
                                else
                                {
                                    norecord();
                                }

                                m_parent.GetProcess();
                                lblMsg.Visible = true;
                              //  lblMsg.Text = Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]);
                            //  MessageBox.Show(Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]), "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //}
                        }
                    }
                    else
                    {
                        lblMsg.Visible = false;
                        Regex intRgx = new Regex(@"^[0-9]+");

                        if (ddlType.SelectedValue.ToString() != "ASJC")
                        {
                            MessageBox.Show("You can't select type other then 'ASJC'", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtFreqncy.Text == "")
                        {
                            MessageBox.Show("Please enter data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (txtFreqncy.Text != "" && !(intRgx.IsMatch(txtFreqncy.Text)))
                        {
                            MessageBox.Show("Please enter valid data in Frequency.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (ddlASJC.SelectedValue.ToString() == "SelectASJC")
                        {
                            MessageBox.Show("Please Select Classification.", "Scival", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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

                            DataSet dsresult = AwardDataOperations.UpdateClassification(WfId, 2, type, Convert.ToInt64(Convert.ToString(tempClassi.Rows[rowindex]["FREQUENCY"])), Convert.ToString(Convert.ToString(tempClassi.Rows[rowindex]["CODE"])), frequency, code, ClassText, Convert.ToString(Convert.ToString(tempClassi.Rows[rowindex]["CLASSIFICATIONS_ID"])));//dalobj.DeleteClassification(WfId, 2, type, frequency, code, ClassText, HF.Value);

                                tempClassi = dsresult.Tables["DisplayData"];
                                if (dsresult.Tables["DisplayData"].Rows.Count > 0)
                                {
                                    grdClass.AutoGenerateColumns = false;
                                    grdClass.DataSource = dsresult.Tables["DisplayData"];
                                }
                                else
                                {
                                    norecord();
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
                            //lblMsg.Text = Convert.ToString(dsresult.Tables["ERRORCODE"].Rows[0][1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
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
                dvSubClassiFication.RowFilter = outputInfo;
            }
            catch (Exception ex)
            {
                oErrorLog.WriteErrorLog(ex);
            }
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue.ToString().ToLower() == "orgspecific")
            {
                txtClassification.Visible = true;
                txtorg_code.Visible = true;
                lblCode.Visible = true;
                ddlASJC.Visible = false;
                dgvSubLevel.Visible = false;
            }
            else
            {
                txtClassification.Visible = false;
                txtorg_code.Visible = false;
                lblCode.Visible = false;
                ddlASJC.Visible = true;
                dgvSubLevel.Visible = true;
            }
        }

    }
}
